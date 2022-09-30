namespace System.TimeFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个定时器
/// </summary>
public interface ITimer
{
    #region 是否激活
    /// <summary>
    /// 如果该属性返回<see langword="true"/>，
    /// 则计时器处于激活状态，否则处于冻结状态
    /// </summary>
    bool IsActivation { get; set; }

    /*实现本API请遵循以下规范：
      #此属性默认为true*/
    #endregion
    #region 返回等待定时器下一个周期的Task
    /// <summary>
    /// 返回一个<see cref="Task"/>，它可以用来等待定时器的下一个周期，
    /// 如果定时器处于冻结状态，则引发一个异常
    /// </summary>
    /// <param name="cancellationToken">用来取消异步任务的令牌</param>
    /// <returns></returns>
    Task Wait(CancellationToken cancellationToken = default);
    #endregion
}
