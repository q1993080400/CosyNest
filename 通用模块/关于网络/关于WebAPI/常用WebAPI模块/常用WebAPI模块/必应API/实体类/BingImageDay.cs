using System.Design.Direct;
using System.Diagnostics.CodeAnalysis;

namespace System.NetFrancis.Api.Bing.Image;

/// <summary>
/// 这个记录封装了必应每日图片的请求结果
/// </summary>
sealed record BingImageDay : IBingImageDay
{
    #region 获取图片Uri
    public required string Uri { get; init; }
    #endregion
    #region 构造函数
    #region 指定Json数据
    /// <summary>
    /// 解析必应每日图片API的返回结果，
    /// 并初始化对象
    /// </summary>
    /// <param name="data"></param>
    [SetsRequiredMembers]
    public BingImageDay(IDirect data)
    {
        var uri = data.GetValueRecursion<string>("images[0].url");
        this.Uri = "https://www.bing.com/" + uri;
    }
    #endregion
    #region 无参数构造函数
    public BingImageDay()
    {

    }
    #endregion
    #endregion
}
