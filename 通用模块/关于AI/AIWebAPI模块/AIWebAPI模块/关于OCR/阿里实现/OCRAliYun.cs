using System.Design;
using System.Design.Direct;
using System.Net;
using System.Net.Http.Json;
using System.NetFrancis;
using System.NetFrancis.Api;
using System.Text.Json;

namespace System.AI;

/// <summary>
/// 本类型是使用阿里云API实现的<see cref="IOCR"/>接口
/// </summary>
/// <param name="appCode">用于身份验证的AppCode</param>
/// <inheritdoc cref="WebApi(IServiceProvider)"/>
sealed class OCRAliYun(string appCode, IServiceProvider serviceProvider) :
    WebApi(serviceProvider), IOCR
{
    #region 说明文档
    /*如果有问题，请查看文档：
      https://market.aliyun.com/apimarket/detail/cmapi028554?spm=5176.730005.result.54.9b963524WQv1rU
     */
    #endregion
    #region 公开成员
    #region 文本识别
    public async Task<string> Identify(string imagePath)
    {
        var base64 = Convert.ToBase64String(await File.ReadAllBytesAsync(imagePath));
        var obj = new
        {
            img = base64,
            paragraph = true,
        };
        var http = HttpClient;
        var httpRequest = new HttpRequestRecording()
        {
            Uri = "https://gjbsb.market.alicloudapi.com/ocrservice/advanced",
            HttpMethod = HttpMethod.Post,
            Content = JsonContent.Create(obj),
            Header = new()
            {
                Authorization = new("APPCODE", appCode)
            }
        };
        using var httpResponse = await http.Request(httpRequest);
        if (httpResponse.StatusCode is HttpStatusCode.MisdirectedRequest)
            throw new WebException("图片短边不能小于15px，长边不能大于8192px，且长宽比不能大于50");
        var json = await httpResponse.Content.ReadAsStringAsync();
        var response = JsonSerializer.Deserialize<IDirect>(json, CreateDesign.JsonCommonOptions())!;
        var info = response.GetValue<object[]>("prism_paragraphsInfo", false)?.Cast<IDirect>() ??
            throw new APIException($"""
                请求阿里OCR接口出现异常，错误码：{response["error_code"]}，错误描述：{response["error_msg"]}
                """);
        return info.Join(x => x["word"]?.ToString() ?? "", Environment.NewLine);
    }
    #endregion
    #endregion 
}
