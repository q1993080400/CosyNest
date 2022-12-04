using System.NetFrancis.Http;

namespace System.NetFrancis.Api.Bing.Image;

/// <summary>
/// 本类型封装了必应图片的API
/// </summary>
sealed class BingImageAPI : WebApi, IBingImageAPI
{
    #region 关于必应每日图片
    #region 获取今日图片
    /// <summary>
    /// 获取必应今日图片
    /// </summary>
    /// <param name="isHorizontalScreen">如果这个值为<see langword="true"/>，
    /// 则返回一个横向的图片，它适用于PC，否则返回一个竖向的图片，它适用于手机</param>
    /// <returns></returns>
    public async Task<IBingImageDay> ImageToDay(bool isHorizontalScreen)
    {
        var response = await (await HttpClientProvide().Request(@"https://cn.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1")).Content.ToObject();
        var result = new BingImageDay(response!);
        return isHorizontalScreen ?
            result :
            new()
            {
                Uri = result.Uri.Replace("1920x1080", "1080x1920")          //此处有隐患，但是尚未发现更好的方法，因为作者不知道这个API的官方文档在哪里
            };
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
