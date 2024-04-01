namespace System.TimeFrancis;

/// <summary>
/// 这个委托是一个定时器，
/// 它返回一个对象，通过它可以等待定时器的下一个周期
/// </summary>
/// <param name="cancellationToken">一个用来取消定时器的令牌</param>
/// <returns></returns>
public delegate TimerInfo Timer(CancellationToken cancellationToken = default);
