namespace System;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都包含了一个属性，它指示某个操作是否成功
/// </summary>
public interface IHasResult
{
    #region 某个操作是否成功
    /// <summary>
    /// 指示某个操作是否成功
    /// </summary>
    bool Success { get; }
    #endregion
}
