using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

using System.Design.Direct;
using System.NetFrancis.Http;

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
        var http = ServiceProvider.GetRequiredService<IHttpClient>();
        var response = await http.RequestPost("https://oapi.dingtalk.com/topapi/v2/department/listsubid",
            new
            {
                dept_id = departmentID
            }, parameters: [("access_token", accessToken)],
            transformation: TransformAccessToken(accessToken)).
            Read(x => x.ToObject());
        return response.GetValue<Num[]>("result.dept_id_list")?.Select(x => x.ToString()).ToArray() ?? [];
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
        var http = ServiceProvider.GetRequiredService<IHttpClient>();
        #region 递归获取所有子部门的本地函数
        async IAsyncEnumerable<DingDingDepartmentInfo> GetSonDepartmentID(string fatherDepartmentID)
        {
            var response = await http.RequestPost("https://oapi.dingtalk.com/topapi/v2/department/listsub",
            new
            {
                dept_id = fatherDepartmentID
            }, parameters: [("access_token", accessToken)],
            transformation: TransformAccessToken(accessToken)).
            Read(x => x.ToObject());
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
                var response = await http.RequestPost("https://oapi.dingtalk.com/topapi/v2/user/list",
                    new
                    {
                        dept_id = departmentID,
                        cursor,
                        size = 20
                    }, parameters: [("access_token", accessToken)],
                    transformation: TransformAccessToken(accessToken)).
                    Read(x => x.ToObject());
                var result = response.GetValue<IDirect>("result")!;
                var list = result.GetValue<object[]>("list")!.Cast<IDirect>().ToArray();
                foreach (var item in list)
                {
                    var info = new DingDingDepartmentRole()
                    {
                        DingDingUserInfo = new()
                        {
                            Name = item.GetValue<string>("name")!,
                            ID = item.GetValue<string>("userid")!,
                            IsUnionID = false
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
        var http = ServiceProvider.GetRequiredService<IHttpClient>();
        var configuration = ServiceProvider.GetRequiredService<DingDingConfiguration>();
        var dataProtection = GetDataProtection();
        var token = await GetUserToken(parameter);
        if (token is null or { IsToken: false } or { RefreshToken: null })
            return new()
            {
                AuthorizationState = new()
                {
                    AuthenticationState = new()
                    {
                        UserInfo = null,
                        NextRequest = parameter
                    },
                    AuthorizationPassed = false
                }
            };
        var (accessToken, refreshToken) = (token.Code, token.RefreshToken);
        var uri = $"https://api.dingtalk.com/v1.0/contact/users/me";
        var transform = ServiceProvider.GetService<HttpRequestTransform>();
        var response = await http.Request(uri, transformation:
            x => x with
            {
                Header = x.Header.With(x => x.Add("x-acs-dingtalk-access-token", [accessToken]))
            }).Read(x => x.ToObject());
        return new()
        {
            AuthorizationState = new()
            {
                AuthenticationState = new()
                {
                    UserInfo = new()
                    {
                        Name = response["nick"]?.ToString() ?? "",
                        ID = response["unionId"]?.ToString() ?? "",
                        IsUnionID = true
                    },
                    NextRequest = new()
                    {
                        Code = dataProtection.Protect(accessToken),
                        IsToken = true,
                        RefreshToken = dataProtection.Protect(refreshToken),
                    }
                },
                AuthorizationPassed = true,
            }
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
