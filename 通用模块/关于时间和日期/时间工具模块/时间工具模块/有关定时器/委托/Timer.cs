namespace System.TimeFrancis;

/// <summary>
/// 这个委托是一个定时器，
/// 它返回一个对象，通过它可以等待定时器的下一个周期
/// </summary>
/// <param name="cancellationToken">一个用来取消定时器的令牌</param>
/// <returns>通过这个对象，可以等待定时器的下一个周期，
/// 如果它为<see langword="null"/>，表示定时器已停止</returns>
public delegate TimerInfo? Timer(CancellationToken cancellationToken = default);
