namespace System.NetFrancis.Api.Bing.Image;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来作为必应每日图片API的响应
/// </summary>
public interface IBingImageDay
{
    #region 获取图片Uri
    /// <summary>
    /// 获取图片的Uri
    /// </summary>
    string Uri { get; }
    #endregion
}
