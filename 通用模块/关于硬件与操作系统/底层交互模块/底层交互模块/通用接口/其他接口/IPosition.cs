using System.Geography;

namespace System.Underlying;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来确定地理位置
/// </summary>
public interface IPosition
{
    #region 定位
    /// <summary>
    /// 执行定位操作
    /// </summary>
    /// <returns>当前用户所在的地理位置，
    /// 如果定位失败，则为<see langword="null"/></returns>
    Task<ILocation?> Position();
    #endregion
}
