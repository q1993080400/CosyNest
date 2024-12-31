
namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 当这个组件存在的时候，
/// 它会阻止屏幕变暗
/// </summary>
public sealed partial class WakeLock : ComponentBase, IAsyncDisposable
{
    #region 公开成员
    public async ValueTask DisposeAsync()
    {
        if (JSWakeLock is null)
            return;
        try
        {
            await JSWakeLock.DisposeAsync();
        }
        catch (JSDisconnectedException)
        {
        }
    }
    #endregion
    #region 内部成员
    #region 唤醒锁
    /// <summary>
    /// 获取封装的JS唤醒锁对象
    /// </summary>
    private IAsyncDisposable? JSWakeLock { get; set; }
    #endregion
    #region 重写的OnAfterRenderAsync方法
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
            JSWakeLock = await JSWindow.Navigator.WakeLock;
    }
    #endregion
    #endregion
}
