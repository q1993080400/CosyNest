namespace System;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以指示某个操作是否成功，
/// 如果成功，还可以获取它的返回值
/// </summary>
/// <typeparam name="Return">操作的返回值的类型</typeparam>
public interface IHasResultValue<Return> : IHasResult
{
    #region 操作的值
    /// <summary>
    /// 获取操作的返回值的类型
    /// </summary>
    Return? Value { get; }
    #endregion
}
