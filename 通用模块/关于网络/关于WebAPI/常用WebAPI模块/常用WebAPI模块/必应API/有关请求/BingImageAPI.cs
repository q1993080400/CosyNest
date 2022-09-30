using System.NetFrancis.Http;

namespace System.NetFrancis.Api.Bing;

/// <summary>
/// 本类型封装了必应图片的API
/// </summary>
public sealed class BingImageAPI : WebApi
{
    #region 关于必应每日图片
    #region 获取今日图片
    /// <summary>
    /// 获取必应今日图片
    /// </summary>
    /// <returns></returns>
    public async Task<BingImageDay> ImageToDay()
    {
        var response = await (await HttpClientProvide().Request(@"https://cn.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1")).Content.ToObject();
        return new(response!);
    }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <inheritdoc cref="WebApi(Func{IHttpClient}?)"/>
    public BingImageAPI(Func<IHttpClient>? httpClientProvide = null)
        : base(httpClientProvide)
    {

    }
    #endregion
}
