namespace System.NetFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个使用SignalR实现的强类型调用
/// </summary>
/// <inheritdoc cref="IStrongTypeInvoke{API}"/>
public interface IStrongTypeSignalRInvoke<API> : IStrongTypeStreamInvoke<API>
    where API : class
{
    #region 激活连接
    /// <summary>
    /// 当连接未创建的时候，创建连接，
    /// 当连接断开的时候，恢复连接
    /// </summary>
    /// <returns></returns>
    Task Activation();
    #endregion
}
