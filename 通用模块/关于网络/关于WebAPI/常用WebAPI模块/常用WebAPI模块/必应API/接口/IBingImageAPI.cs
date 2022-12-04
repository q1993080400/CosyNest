namespace System.NetFrancis.Api.Bing.Image;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来请求必应图片API
/// </summary>
public interface IBingImageAPI
{
    #region 获取今日图片
    /// <summary>
    /// 获取必应今日图片
    /// </summary>
    /// <param name="isHorizontalScreen">如果这个值为<see langword="true"/>，
    /// 则返回一个横向的图片，它适用于PC，否则返回一个竖向的图片，它适用于手机</param>
    /// <returns></returns>
    Task<IBingImageDay> ImageToDay(bool isHorizontalScreen);
    #endregion
}
