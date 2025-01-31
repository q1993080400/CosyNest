namespace System;

/// <summary>
/// 表示一个异步版本的锁，
/// 它可以利用using语句自动完成某些操作，以避免遗漏
/// </summary>
/// <param name="disposable">在对象被释放的时候，这个委托会被执行</param>
sealed class LockAsync(Func<ValueTask>? disposable) : IAsyncDisposable
{
    #region 释放锁
    public async ValueTask DisposeAsync()
    {
        if (disposable is null)
            return;
        var disposableCache = disposable;
        disposable = null;
        await disposableCache();
    }
    #endregion
}
