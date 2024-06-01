namespace System.Design;

/// <summary>
/// 这个类型实现了令牌桶限流算法
/// </summary>
/// <param name="maxToken">最大令牌的数量</param>
/// <param name="resetInterval">重置令牌的周期</param>
sealed class TokenBucket(int maxToken, TimeSpan resetInterval) : IBlock
{
    #region 公开成员
    #region 阻塞线程
    /// <inheritdoc cref="IBlock"/>
    public (bool Complete, Task Wait) Block()
    {
        if (Interlocked.Decrement(ref SurplusToken) >= 0)
            return (true, Task.CompletedTask);
        #region 本地函数
        async Task Fun()
        {
            var now = DateTimeOffset.Now;
            Lock.EnterReadLock();
            var resetTime = LastResetTime + resetInterval;
            Lock.ExitReadLock();
            if (resetTime <= now)
            {
                Interlocked.Exchange(ref SurplusToken, MaxToken - 1);
                Lock.EnterWriteLock();
                LastResetTime = now;
                Lock.ExitWriteLock();
                return;
            }
            await Task.Delay(resetTime - now);
            await Block().Wait;
        }
        #endregion
        return (false, Fun());
    }
    #endregion
    #region 释放对象
    public void Dispose()
    {
        Lock.Dispose();
    }
    #endregion
    #endregion
    #region 内部成员
    #region 剩余令牌数量
    /// <summary>
    /// 获取当前剩余令牌的数量
    /// </summary>
    private int SurplusToken = maxToken;
    #endregion
    #region 最大令牌的数量
    /// <summary>
    /// 获取最大令牌的数量
    /// </summary>
    private int MaxToken { get; } = maxToken;
    #endregion
    #region 上次重置令牌的时间
    /// <summary>
    /// 获取上次重置令牌的时间
    /// </summary>
    private DateTimeOffset LastResetTime { get; set; }
    #endregion
    #region 线程锁
    /// <summary>
    /// 获取一个线程锁，它用来维护线程安全
    /// </summary>
    private ReaderWriterLockSlim Lock { get; } = new();
    #endregion
    #endregion
}
