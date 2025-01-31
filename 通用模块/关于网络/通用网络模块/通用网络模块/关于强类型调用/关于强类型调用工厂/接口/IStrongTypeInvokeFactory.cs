namespace System.NetFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来创建强类型调用
/// </summary>
public interface IStrongTypeInvokeFactory
{
    #region 创建强类型调用
    /// <summary>
    /// 创建一个强类型调用
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="IStrongTypeInvoke{API}"/>
    IStrongTypeInvoke<API> StrongType<API>()
        where API : class;
    #endregion
}
