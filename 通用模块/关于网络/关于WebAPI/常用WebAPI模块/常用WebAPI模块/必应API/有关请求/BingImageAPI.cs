namespace System.NetFrancis.Api;

/// <summary>
/// 本类型封装了必应图片的API
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <inheritdoc cref="WebApi(IServiceProvider)"/>
sealed class BingImageAPI(IServiceProvider serviceProvider) : WebApi(serviceProvider), IBingImageAPI
{
    #region 获取今日图片
    public async Task<IBingImageDay> ImageToDay(bool isHorizontalScreen)
    {
        var response = await HttpClient.RequestJsonGet(@"https://cn.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1")!;
        var uri = response.GetValue<string>("images[0].url");
        var completeUri = "https://www.bing.com/" + uri;
        var finalUri = isHorizontalScreen ?
            completeUri :
            completeUri.Replace("1920x1080", "1080x1920");
        return new BingImageDay()
        {
            Uri = finalUri
        };
    }
    #endregion
}
