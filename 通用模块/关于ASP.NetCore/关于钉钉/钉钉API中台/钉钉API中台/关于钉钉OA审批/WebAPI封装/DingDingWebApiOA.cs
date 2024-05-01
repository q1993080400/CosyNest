using System.Design.Direct;
using System.NetFrancis.Http;

using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个类型可以用来管理钉钉OA审批
/// </summary>
/// <inheritdoc cref="DingDingWebApi(DingDingWebApiInfo)"/>
public sealed class DingDingWebApiOA(DingDingWebApiInfo info) : DingDingWebApi(info)
{
    #region 获取所有表单模板
    /// <summary>
    /// 获取一个枚举所有钉钉表单模板的枚举器
    /// </summary>
    /// <param name="userId">用户的ID，
    /// 如果它为<see langword="null"/>，则仅返回该用户可见的表单模板，
    /// 否则返回本公司所有表单模板</param>
    /// <returns></returns>
    public async IAsyncEnumerable<DingDingOAFormTemplate> GetDingDingOAFormTemplate(string? userId = null)
    {
        var accessToken = (await GetCompanyToken()) ??
            throw new NotSupportedException("获取公司访问令牌失败");
        var http = ServiceProvider.GetRequiredService<IHttpClient>();
        long? nextToken = 0L;
        while (nextToken is { })
        {
            var request = new HttpRequestRecording($"https://api.dingtalk.com/v1.0/workflow/processes/userVisibilities/templates",
                [("userId", userId), ("maxResults", "100"), ("nextToken", nextToken.ToString())]);
            var response = await http.Request(request, x => x with
            {
                Header = x.Header.With(x => x.Add("x-acs-dingtalk-access-token", [accessToken]))
            }).Read(x => x.ToObject());
            var list = response.GetValue<object[]>("result.processList")?.Cast<IDirect>().ToArray();
            if (list is null)
                yield break;
            foreach (var item in list)
            {
                var name = item.GetValue<string>("name")!;
                var processCode = item.GetValue<string>("processCode")!;
                yield return new()
                {
                    ID = processCode,
                    Name = name
                };
            }
            if (list.Length < 100)
                yield break;
            nextToken = response.GetValue<string>("result.nextToken", false)?.To<long?>(false);
        }
    }
    #endregion
}
