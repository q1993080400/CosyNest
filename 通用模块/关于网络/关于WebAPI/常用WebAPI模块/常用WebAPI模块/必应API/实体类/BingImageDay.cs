using System.Design.Direct;

namespace System.NetFrancis.Api.Bing;

/// <summary>
/// 这个记录封装了必应每日图片的请求结果
/// </summary>
public sealed record BingImageDay
{
    #region 获取图片Uri
    /// <summary>
    /// 获取图片的Uri
    /// </summary>
    public string Uri { get; }
    #endregion
    #region 构造函数
    internal BingImageDay(IDirect data)
    {
        var uri = data.GetValue<object[]>("images")!.Cast<IDirect>().First().GetValue<string>("url");
        this.Uri = "https://www.bing.com/" + uri;
    }
    #endregion
}
