namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 本类型是<see cref="IHandleEvent"/>的实现，
/// 它不会刷新组件，在某些时候使用本类型可以提高性能
/// </summary>
sealed class HandleEventInvalid : IHandleEvent
{
    #region 通知组件需要刷新
    public Task HandleEventAsync(EventCallbackWorkItem item, object? arg)
        => item.InvokeAsync(arg);
    #endregion 
}
