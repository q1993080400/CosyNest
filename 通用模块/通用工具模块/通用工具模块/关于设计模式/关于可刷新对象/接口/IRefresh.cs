namespace System;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来刷新对象
/// </summary>
public interface IRefresh
{
    #region 刷新对象
    /// <summary>
    /// 刷新这个对象
    /// </summary>
    /// <returns></returns>
    Task Refresh();
    #endregion
}
