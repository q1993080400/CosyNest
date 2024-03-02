namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 当释放这个对象的时候，也会释放唤醒锁
/// </summary>
/// <param name="runtime">JS运行时对象</param>
/// <param name="id">唤醒锁的ID</param>
sealed class JSWakeLock(IJSRuntime runtime, string id) : IAsyncDisposable
{
    #region 公开成员
    #region 释放对象
    public async ValueTask DisposeAsync()
    {
        await runtime.InvokeVoidAsync("ReleaseWakeLock", id);
    }
    #endregion
    #endregion
}
