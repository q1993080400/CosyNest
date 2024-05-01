using System.Design;
using System.Design.Direct;
using System.Net;
using System.NetFrancis.Api;
using System.NetFrancis.Http;
using System.Text.Json;

namespace System.AI;

/// <summary>
/// 本类型是使用阿里云API实现的<see cref="IOCR"/>接口
/// </summary>
/// <param name="appCode">用于身份验证的AppCode</param>
/// <inheritdoc cref="WebApi(Func{IHttpClient}?)"/>
sealed class OCRAliYun(string appCode, Func<IHttpClient>? httpClientProvide) :
    WebApi(httpClientProvide), IOCR
{

#pragma warning disable SYSLIB0014

    #region 说明文档
    /*如果有问题，请查看文档：
      https://market.aliyun.com/apimarket/detail/cmapi028554?spm=5176.730005.result.54.9b963524WQv1rU

      问：为什么要使用非常古老的HttpWebRequest进行Http请求？
      答：这是因为HttpClient缺乏发起Post请求，并返回Stream的能力，
      实属不得已而为之，如果HttpClient后续将这个能力补上，请重构本方法
     */
    #endregion
    #region 公开成员
    #region 文本识别
    public async Task<string?> Identify(string imagePath)
    {
        const string uri = "https://gjbsb.market.alicloudapi.com/ocrservice/advanced";
        using var file = new FileStream(imagePath, FileMode.Open);
        var base64 = Convert.ToBase64String(await file.ReadAll());
        var obj = new
        {
            img = base64,
            paragraph = true,
        };
        var objJson = JsonSerializer.Serialize(obj)!.ToBytes();
        var httpRequest = WebRequest.CreateHttp(new Uri(uri));
        httpRequest.Method = "Post";
        httpRequest.Headers.Add("Authorization", "APPCODE " + appCode);
        httpRequest.ContentType = "application/json; charset=UTF-8";
        using var requestStream = await httpRequest.GetRequestStreamAsync();
        requestStream.Write(objJson, 0, objJson.Length);
        requestStream.Close();
        using var httpResponse = (HttpWebResponse)await httpRequest.GetResponseAsync();
        if (httpResponse.StatusCode is HttpStatusCode.MisdirectedRequest)
            throw new WebException("图片短边不能小于15px，长边不能大于8192px，且长宽比不能大于50");
        using var st = httpResponse.GetResponseStream();
        var json = (await st.ReadAll()).ToText();
        var response = JsonSerializer.Deserialize<IDirect>(json, CreateDesign.JsonCommonOptions)!;
        var info = response.GetValue<object[]>("prism_paragraphsInfo", false)?.Cast<IDirect>() ??
            throw new APIException($"""
                请求阿里OCR接口出现异常，错误码：{response["error_code"]}，错误描述：{response["error_msg"]}
                """);
        return info.Join(x => x["word"]?.ToString() ?? "", Environment.NewLine);
    }
    #endregion
    #endregion 
}
