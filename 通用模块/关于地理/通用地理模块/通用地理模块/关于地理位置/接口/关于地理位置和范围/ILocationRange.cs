namespace System.Geography;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来表示一个地理范围
/// </summary>
public interface ILocationRange
{
    #region 地理范围的大致位置
    /// <summary>
    /// 返回地理范围的大致位置
    /// </summary>
    ILocation Position { get; }
    #endregion
    #region 返回地理位置是否位于范围内
    /// <summary>
    /// 返回某一地理位置是否位于范围内
    /// </summary>
    /// <param name="location">要检查的地理位置</param>
    /// <returns></returns>
    bool InRange(ILocation location);
    #endregion
}
