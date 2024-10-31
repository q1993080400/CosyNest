namespace System.Realize;

/// <summary>
/// 表示一个锁，
/// 它可以利用using语句自动完成某些操作，以避免遗漏
/// </summary>
/// <param name="dispose">在对象被释放的时候，这个委托会被执行</param>
sealed class Lock(Action? dispose) : IDisposable
{
    #region 释放锁
    public void Dispose()
    {
        dispose?.Invoke();
        dispose = null;
    }
    #endregion
}
