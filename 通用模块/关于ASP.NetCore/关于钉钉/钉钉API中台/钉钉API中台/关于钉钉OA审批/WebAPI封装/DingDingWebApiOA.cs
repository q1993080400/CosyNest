using System.Design.Direct;
using System.NetFrancis.Http;

using Microsoft.Extensions.DependencyInjection;

namespace System.DingDing;

/// <summary>
/// 这个类型可以用来管理钉钉OA审批
/// </summary>
/// <inheritdoc cref="DingDingWebApi(IServiceProvider)"/>
public sealed class DingDingWebApiOA(IServiceProvider serviceProvider) : DingDingWebApi(serviceProvider)
{
    #region 获取所有表单模板
    /// <summary>
    /// 获取一个枚举所有钉钉表单模板的枚举器
    /// </summary>
    /// <param name="userId">用户的ID，
    /// 如果它为<see langword="null"/>，则仅返回该用户可见的表单模板，
    /// 否则返回本公司所有表单模板</param>
    /// <returns></returns>
    public async IAsyncEnumerable<DingDingOAFormTemplate> GetOAFormTemplate(string? userId = null)
    {
        var accessToken = await GetCompanyToken();
        var http = ServiceProvider.GetRequiredService<IHttpClient>();
        long? nextToken = 0L;
        while (nextToken is { })
        {
            var request = new HttpRequestRecording()
            {
                Uri = new($"https://api.dingtalk.com/v1.0/workflow/processes/userVisibilities/templates")
                {
                    UriParameter = new([("userId", userId), ("maxResults", "100"), ("nextToken", nextToken.ToString())])
                }
            };
            await Delay();
            var response = await http.RequestJson(request, TransformAccessToken(accessToken));
            var list = response.GetValue<object[]>("result.processList")?.Cast<IDirect>().ToArray();
            if (list is null)
                yield break;
            foreach (var item in list)
            {
                #region 本地函数
                string Fun(string key, bool check)
                    => item.GetValue<string>(key, check)!;
                #endregion
                yield return new()
                {
                    FormTemplateID = Fun("processCode", true),
                    Name = Fun("name", true),
                    Uri = Fun("url", true),
                    IconUri = Fun("iconUrl", true),
                    GroupID = Fun("dirId", false),
                    GroupName = Fun("dirName", false)
                };
            }
            if (list.Length < 100)
                yield break;
            nextToken = response.GetValue<string>("result.nextToken", false)?.To<long?>(false);
        }
    }
    #endregion
    #region 获取审批实例ID列表
    /// <summary>
    /// 获取所有符合条件的审批实例的ID
    /// </summary>
    /// <param name="parameter">用来获取审批实例ID的参数</param>
    /// <returns></returns>
    public async IAsyncEnumerable<string> GetOAInstanceID(GetOAInstanceIDParameter parameter)
    {
        var accessToken = await GetCompanyToken();
        var http = ServiceProvider.GetRequiredService<IHttpClient>();
        long? nextToken = 0L;
        while (nextToken is { })
        {
            var postParameter = new
            {
                processCode = parameter.FormTemplateID,
                startTime = parameter.StartDate.ToUnixTimeMilliseconds(),
                endTime = parameter.EndDate.ToUnixTimeMilliseconds(),
                userIds = parameter.UserIDs,
                nextToken,
                maxResults = 20,
                statuses = parameter.States.Select(x => x switch
                {
                    DingDingOAState.UnderApproval => "RUNNING",
                    DingDingOAState.Rescinded => "TERMINATED",
                    DingDingOAState.ApprovalCompleted => "COMPLETED",
                    var state => throw new NotSupportedException($"未能识别{state}类型的审批状态")
                })
            };
            await Delay();
            var response = await http.RequestJsonPost("https://api.dingtalk.com/v1.0/workflow/processes/instanceIds/query",
                postParameter, transformation: TransformAccessToken(accessToken));
            response = VerifyResponse(response);
            var success = response.GetValue<bool>("success", false);
            if (!success)
                yield break;
            var list = (response.GetValue<object[]>("result.list") ?? []).Cast<string>().ToArray();
            foreach (var item in list)
            {
                yield return item;
            }
            if (list.Length is < 20)
                yield break;
            nextToken = response.GetValue<long?>("result.nextToken", false);
        }
    }
    #endregion
    #region 获取审批实例
    #region 正式方法
    /// <summary>
    /// 获取具有指定ID的OA审批实例
    /// </summary>
    /// <param name="instanceID">审批实例的ID</param>
    /// <returns></returns>
    public async Task<DingDingOAInstance> GetOAInstance(string instanceID)
    {
        var accessToken = await GetCompanyToken();
        var http = ServiceProvider.GetRequiredService<IHttpClient>();
        await Delay();
        var response = await http.RequestJsonGet("https://api.dingtalk.com/v1.0/workflow/processInstances",
            [("processInstanceId", instanceID)], transformation: TransformAccessToken(accessToken));
        return ConvertInstance(response, instanceID);
    }
    #endregion
    #region 转换对象
    #region 转换DingDingOAInstance
    /// <summary>
    /// 将动态对象转换为<see cref="DingDingOAInstance"/>
    /// </summary>
    /// <param name="json">待转换的动态对象</param>
    /// <param name="instanceID">审批实例的ID</param>
    /// <returns></returns>
    private static DingDingOAInstance ConvertInstance(IDirect json, string instanceID)
    {
        var result = json.GetValue<IDirect>("result") ??
            throw new NullReferenceException("审批实例的返回值中没有结果");
        #region 本地函数
        Obj Fun<Obj>(string path)
            => result.GetValue<Obj>(path, false)!;
        #endregion
        return new()
        {
            ID = instanceID,
            Title = Fun<string>("title"),
            FinishTime = ConvertDate(Fun<DateTimeOffset?>("finishTime")),
            OriginatorUserID = Fun<string>("originatorUserId"),
            OriginatorDeptID = Fun<string>("originatorDeptId"),
            OriginatorDeptName = Fun<string>("originatorDeptName"),
            State = Fun<string>("status") switch
            {
                "RUNNING" => DingDingOAState.UnderApproval,
                "TERMINATED" => DingDingOAState.Rescinded,
                "COMPLETED" => DingDingOAState.ApprovalCompleted,
                var s => throw new NotSupportedException($"未能识别{s}类型的状态")
            },
            ApproverUserID = Fun<string[]>("approverUserIds") ?? [],
            CCUserID = Fun<string[]>("ccUserIds") ?? [],
            IsAgree = Fun<string>("result") is "agree",
            BusinessID = Fun<string>("businessId"),
            OperationRecords = Fun<object[]>("operationRecords").Cast<IDirect>().
                Select(ConvertOperationRecord).ToArray(),
            Tasks = Fun<object[]>("tasks").Cast<IDirect>().
                Select(ConvertTask).ToArray(),
            BizAction = Fun<string>("bizAction") switch
            {
                "MODIFY" => DingDingOABizAction.Modify,
                "REVOKE" => DingDingOABizAction.Revoke,
                "NONE" => DingDingOABizAction.None,
                var action => throw new NotSupportedException($"未能识别{action}类型的业务动作")
            },
            BizData = Fun<string>("bizData"),
            AttachedInstanceID = Fun<string[]>("attachedProcessInstanceIds"),
            MainProcessInstanceID = Fun<string>("mainProcessInstanceId"),
            FormComponentValues = Fun<object[]>("formComponentValues").Cast<IDirect>().
                Select(ConvertComponentValue).ToArray(),
            CreateTime = ConvertDate(Fun<DateTimeOffset>("createTime"))
        };
    }
    #endregion
    #region 转换操作记录
    /// <summary>
    /// 将动态对象转换为<see cref="DingDingOAOperationRecord"/>
    /// </summary>
    /// <param name="json">待转换的动态对象</param>
    /// <returns></returns>
    private static DingDingOAOperationRecord ConvertOperationRecord(IDirect json)
    {
        #region 本地函数
        Obj Fun<Obj>(string path)
            => json.GetValue<Obj>(path, false)!;
        #endregion
        return new()
        {
            OriginatorUserID = Fun<string>("userId"),
            Date = ConvertDate(Fun<DateTimeOffset>("date")),
            OperationType = Fun<string>("type") switch
            {
                "EXECUTE_TASK_NORMAL" => DingDingOAOperationType.Normal,
                "EXECUTE_TASK_AGENT" => DingDingOAOperationType.Agent,
                "APPEND_TASK_BEFORE" => DingDingOAOperationType.PreSigningTask,
                "APPEND_TASK_AFTER" => DingDingOAOperationType.PostSigningTask,
                "REDIRECT_TASK" => DingDingOAOperationType.Transfer,
                "START_PROCESS_INSTANCE" => DingDingOAOperationType.Launch,
                "TERMINATE_PROCESS_INSTANCE" => DingDingOAOperationType.Termination,
                "FINISH_PROCESS_INSTANCE" => DingDingOAOperationType.Finish,
                "ADD_REMARK" => DingDingOAOperationType.AddComments,
                "REDIRECT_PROCESS" => DingDingOAOperationType.ReturnComment,
                "PROCESS_CC" => DingDingOAOperationType.CC,
                "EXECUTE_TASK_AUTO" => DingDingOAOperationType.ExexuteTaskAuto,
                var type => throw new NotSupportedException($"未能识别{type}类型的操作")
            },
            Result = Fun<string>("result") switch
            {
                "AGREE" => DingDingOAOperationResult.Agree,
                "REFUSE" => DingDingOAOperationResult.Refuse,
                "NONE" => DingDingOAOperationResult.None,
                var type => throw new NotSupportedException($"未能识别{type}类型的操作结果")
            },
            Remark = Fun<string>("remark") ?? "",
            Attachments = Fun<object[]>("attachments")?.Cast<IDirect>().
                Select(ConvertAttachment).ToArray() ?? [],
            CCUserID = Fun<string[]>("ccUserIds") ?? []
        };
    }
    #endregion
    #region 转换附件
    /// <summary>
    /// 将动态对象转换为<see cref="DingDingOACommentAttachment"/>
    /// </summary>
    /// <param name="json">待转换的动态对象</param>
    /// <returns></returns>
    private static DingDingOACommentAttachment ConvertAttachment(IDirect json)
    {
        #region 本地函数
        Obj Fun<Obj>(string path)
            => json.GetValue<Obj>(path, false)!;
        #endregion
        return new()
        {
            FileName = Fun<string>("fileName"),
            FileSize = Fun<string>("fileSize"),
            FileID = Fun<string>("fileId"),
            FileType = Fun<string>("fileType")
        };
    }
    #endregion
    #region 转换审批任务
    /// <summary>
    /// 将动态对象转换为<see cref="DingDingOATask"/>
    /// </summary>
    /// <param name="json">待转换的动态对象</param>
    /// <returns></returns>
    private static DingDingOATask ConvertTask(IDirect json)
    {
        #region 本地函数
        Obj Fun<Obj>(string path)
            => json.GetValue<Obj>(path, false)!;
        #endregion
        return new()
        {
            TaskID = Fun<string>("taskId"),
            UserID = Fun<string>("userId"),
            State = Fun<string>("status") switch
            {
                "NEW" => DingDingOATaskState.NotStarted,
                "RUNNING" => DingDingOATaskState.Processing,
                "PAUSED" => DingDingOATaskState.Suspend,
                "CANCELED" => DingDingOATaskState.Cancel,
                "COMPLETED" => DingDingOATaskState.Complete,
                "TERMINATED" => DingDingOATaskState.Termination,
                var state => throw new NotSupportedException($"无法识别{state}类型的钉钉任务状态")
            },
            Result = Fun<string>("result") switch
            {
                "AGREE" => DingDingOATaskResult.Agree,
                "REFUSE" => DingDingOATaskResult.Refuse,
                "REDIRECTED" => DingDingOATaskResult.Transfer,
                "NONE" => DingDingOATaskResult.None,
                var state => throw new NotSupportedException($"无法识别{state}类型的钉钉任务结果")
            },
            CreateTime = ConvertDate(Fun<DateTimeOffset>("createTime")),
            FinishTime = ConvertDate(Fun<DateTimeOffset?>("finishTime")),
            MobileUri = Fun<string>("mobileUrl"),
            PCUri = Fun<string>("pcUrl"),
            InstanceID = Fun<string>("processInstanceId"),
            ActivityID = Fun<string>("activityId")
        };
    }
    #endregion
    #region 转换组件详情
    /// <summary>
    /// 将动态对象转换为<see cref="DingDingOAFormComponentValue"/>
    /// </summary>
    /// <param name="json">待转换的动态对象</param>
    /// <returns></returns>
    private static DingDingOAFormComponentValueExtend ConvertComponentValue(IDirect json)
    {
        #region 本地函数
        Obj Fun<Obj>(string path)
            => json.GetValue<Obj>(path, false)!;
        #endregion
        return new()
        {
            ID = Fun<string>("id"),
            Name = Fun<string>("name"),
            Value = Fun<string>("value"),
            ExtValue = Fun<string>("extValue"),
            BizAlias = Fun<string>("bizAlias"),
            ComponentType = Fun<string>("componentType").To<DingDingOAComponentType>()
        };
    }
    #endregion
    #endregion
    #endregion
    #region 下载审批附件
    /// <summary>
    /// 获取一个对象，它可以用来下载审批附件，
    /// 如果因为员工离职无法下载，则返回<see langword="null"/>
    /// </summary>
    /// <param name="instanceID">审批实例的ID</param>
    /// <param name="fileID">文件的ID</param>
    /// <returns></returns>
    public async Task<DingDingOAAttachmentDownloadInfo?> DownloadOAAttachment(string instanceID, string fileID)
    {
        var accessToken = await GetCompanyToken();
        var http = ServiceProvider.GetRequiredService<IHttpClient>();
        await Delay();
        var response = await http.RequestJsonPost("https://api.dingtalk.com/v1.0/workflow/processInstances/spaces/files/urls/download",
            new
            {
                processInstanceId = instanceID,
                fileId = fileID,
            }, transformation: TransformAccessToken(accessToken));
        var result = response.GetValue<IDirect>("result", false);
        if (result is null && response.GetValue<string>("code", false) is "userNotExist")
            return null;
        #region 本地函数
        Obj Fun<Obj>(string key)
            => result!.GetValue<Obj>(key)!;
        #endregion
        return new()
        {
            SpaceID = Fun<string>("spaceId"),
            FileID = Fun<string>("fileId"),
            DownloadUri = Fun<string>("downloadUri"),
            ExpirationDate = DateTimeOffset.Now.AddMinutes(15)
        };
    }
    #endregion
}
