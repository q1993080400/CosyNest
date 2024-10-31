using System.Design.Direct;
using System.NetFrancis.Http;

using Microsoft.Extensions.DependencyInjection;

namespace System.DingDing;

/// <summary>
/// 本类型可以用来请求基本的钉钉API，例如通讯录
/// </summary>
/// <inheritdoc cref="DingDingWebApi(IServiceProvider)"/>
public sealed class DingDingWebApiBasis(IServiceProvider serviceProvider) :
    DingDingWebApi(serviceProvider), IGetAuthenticationDingDingState
{
    #region 获取子部门ID
    /// <summary>
    /// 获取所有子部门的ID
    /// </summary>
    /// <param name="departmentID">父部门的ID，
    /// 如果为1，表示根部门</param>
    /// <returns></returns>
    public async Task<string[]> GetSonDepartmentID(string departmentID = "1")
    {
        var accessToken = await GetCompanyToken();
        var http = HttpClient;
        var response = await http.RequestJsonPost("https://oapi.dingtalk.com/topapi/v2/department/listsubid",
            new
            {
                dept_id = departmentID
            }, parameters: [("access_token", accessToken)],
            transformation: TransformAccessToken(accessToken));
        return response.GetValue<double[]>("result.dept_id_list")?.Select(x => x.ToString()).ToArray() ?? [];
    }
    #endregion
    #region 获取所有子部门和员工
    /// <summary>
    /// 获取所有子部门和员工
    /// </summary>
    /// <returns></returns>
    public async IAsyncEnumerable<DingDingDepartmentInfo> GetAllSonDepartment()
    {
        var accessToken = await GetCompanyToken();
        var http = HttpClient;
        #region 递归获取所有子部门的本地函数
        async IAsyncEnumerable<DingDingDepartmentInfo> GetSonDepartmentID(string fatherDepartmentID)
        {
            await Delay();
            var response = await http.RequestJsonPost("https://oapi.dingtalk.com/topapi/v2/department/listsub",
            new
            {
                dept_id = fatherDepartmentID
            }, parameters: [("access_token", accessToken)],
            transformation: TransformAccessToken(accessToken));
            var departments = response.GetValue<IDirect[]>("result")!;
            foreach (var department in departments)
            {
                var departmentID = department.GetValue<string>("dept_id")!;
                var name = department.GetValue<string>("name")!;
                var member = await GetDepartmentMember(departmentID).ToArrayAsync();
                var son = await GetSonDepartmentID(departmentID).ToArrayAsync();
                yield return new()
                {
                    ID = departmentID,
                    Member = member,
                    Name = name,
                    Son = son
                };
            }
        }
        #endregion
        #region 获取部门所有员工
        async IAsyncEnumerable<DingDingDepartmentRole> GetDepartmentMember(string departmentID)
        {
            var cursor = 0;
            while (true)
            {
                await Delay();
                var response = await http.RequestJsonPost("https://oapi.dingtalk.com/topapi/v2/user/list",
                    new
                    {
                        dept_id = departmentID,
                        cursor,
                        size = 20
                    }, parameters: [("access_token", accessToken)],
                    transformation: TransformAccessToken(accessToken));
                response = VerifyResponse(response);
                var result = response.GetValue<IDirect>("result")!;
                var list = result.GetValue<object[]>("list")!.Cast<IDirect>().ToArray();
                foreach (var item in list)
                {
                    var avatarUri = item.GetValue<string>("avatar");
                    var info = new DingDingDepartmentRole()
                    {
                        DingDingUserInfo = new()
                        {
                            Name = item.GetValue<string>("name")!,
                            UserID = item.GetValue<string>("userid")!,
                            AvatarUri = avatarUri.IsVoid() ? null : avatarUri,
                        },
                        IsLeader = item.GetValue<bool>("leader")
                    };
                    yield return info;
                }
                var hasMore = result.GetValue<bool>("has_more");
                if (!hasMore)
                    yield break;
                cursor = result.GetValue<int>("next_cursor");
            }
        }
        #endregion
        await foreach (var department in GetSonDepartmentID("1"))
        {
            yield return department;
        }
    }
    #endregion
    #region 返回钉钉身份验证状态
    public async Task<APIPackDingDing> GetAuthenticationDingDingState(AuthenticationDingDingRequest parameter)
    {
        var http = HttpClient;
        var configuration = ServiceProvider.GetRequiredService<DingDingConfiguration>();
        var token = await GetUserToken(parameter);
        if (token is null or { IsToken: false } or { RefreshToken: null })
            return new()
            {
                AuthenticationState = new()
                {
                    AuthenticationResult = null
                },
            };
        var (accessToken, refreshToken) = (token.Code, token.RefreshToken);
        var uri = $"https://api.dingtalk.com/v1.0/contact/users/me";
        var transform = ServiceProvider.GetService<HttpRequestTransform>();
        var response = await http.RequestJsonGet(uri, transformation:
           x => x with
           {
               Header = x.Header.With(x => x.Add("x-acs-dingtalk-access-token", [accessToken]))
           });
        var companyToken = await GetCompanyToken();
        var unionId = response["unionId"]?.ToString() ?? "";
        var userIDResponse = await http.RequestJsonPost("https://oapi.dingtalk.com/topapi/user/getbyunionid",
            new
            {
                unionid = unionId,
            },
            parameters: [("access_token", companyToken)]);
        var authenticationResult = new AuthenticationDingDingResult()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            IsEncryption = false,
            UserInfo = new()
            {
                Name = response["nick"]?.ToString() ?? "",
                UserID = userIDResponse.GetValue<string>("result.userid") ??
                            throw new NotSupportedException("未能获取UserID"),
                AvatarUri = response.TryGetValue("avatarUrl").Value?.ToString()
            }
        }.Encryption(GetDataProtection());
        return new()
        {
            AuthenticationState = new()
            {
                AuthenticationResult = authenticationResult
            },
        };
    }
    #endregion
    #region 获取App信息
    public Task<DingDingAppInfo> GetAppInfo()
    {
        var configuration = ServiceProvider.GetRequiredService<DingDingConfiguration>();
        return new DingDingAppInfo()
        {
            ClientID = configuration.ClientID,
            OrganizationID = configuration.OrganizationID
        }.ToTask();
    }
    #endregion
}
