using System.Underlying;

namespace Microsoft.JSInterop;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为JS中的Window对象的Net封装，
/// 它代表当前浏览器页面
/// </summary>
public interface IJSWindow : IJSRuntime
{
    #region 关于硬件
    #region 返回当前屏幕
    /// <summary>
    /// 返回当前屏幕对象
    /// </summary>
    /// <returns></returns>
    Task<IJSScreen> Screen { get; }
    #endregion
    #region 返回Navigator对象
    /// <summary>
    /// 返回Navigator对象，
    /// 它可以用来获取有关硬件或平台的信息
    /// </summary>
    IJSNavigator Navigator { get; }
    #endregion
    #endregion
    #region 关于存储
    #region 本地存储
    /// <summary>
    /// 返回一个字典，它可以用来索引浏览器本地存储，
    /// 本地存储不会过期
    /// </summary>
    IAsyncDictionary<string, string> LocalStorage { get; }
    #endregion
    #region 会话存储
    /// <summary>
    /// 返回一个字典，
    /// 它可以用来索引浏览器会话存储，
    /// 在关闭页面的时候，会话存储会被清除
    /// </summary>
    IAsyncDictionary<string, string> SessionStorage { get; }
    #endregion
    #endregion
    #region 返回Document对象
    /// <summary>
    /// 返回JS中的Document对象
    /// </summary>
    IJSDocument Document { get; }
    #endregion
    #region 返回Location对象
    /// <summary>
    /// 返回JS中的Location对象，
    /// 它可以用来检查和操作窗体的Uri
    /// </summary>
    IJSLocation Location { get; }
    #endregion
    #region 关于弹窗
    #region 弹出消息窗
    /// <summary>
    /// 弹出一个消息窗
    /// </summary>
    /// <param name="message">消息窗的文本</param>
    /// <param name="cancellation">用来取消异步任务的令牌</param>
    /// <returns></returns>
    ValueTask Alert(string message, CancellationToken cancellation = default);
    #endregion
    #region 弹出确认窗
    /// <summary>
    /// 弹出一个确认窗，并返回其结果
    /// </summary>
    /// <param name="message">确认窗的文本</param>
    /// <param name="cancellation">用来取消异步任务的令牌</param>
    /// <returns>如果点击确认，则返回<see langword="true"/>，
    /// 如果点击取消，则返回<see langword="false"/></returns>
    ValueTask<bool> Confirm(string message, CancellationToken cancellation = default);
    #endregion
    #region 弹出输入框
    /// <summary>
    /// 弹出一个输入框
    /// </summary>
    /// <param name="text">用来提示用户输入文字的字符串</param>
    /// <param name="value">文本输入框中的默认值</param>
    /// <param name="cancellation">用来取消异步任务的令牌</param>
    /// <returns>一个元组，它的第一个项是用户是否点击了确定按钮，
    /// 第二个项是输入的内容，如果直接点确定，没有输入，则为<see langword="null"/></returns>
    ValueTask<(bool IsConfirm, string? Input)> Prompt(string? text = null, string? value = "", CancellationToken cancellation = default);
    #endregion
    #endregion
    #region 关于打开与关闭窗口
    #region 打开新窗口
    /// <summary>
    /// 打开一个新的窗口
    /// </summary>
    /// <param name="strUrl">新窗口的Url</param>
    /// <param name="strWindowName">新窗口的名称，
    /// 如果不指定，默认为打开新窗口</param>
    /// <param name="strWindowFeatures">新窗口的一些特性</param>
    /// <param name="cancellation">用于取消异步操作的令牌</param>
    /// <returns></returns>
    ValueTask Open(string strUrl, string strWindowName = "_blank", string strWindowFeatures = "noopener", CancellationToken cancellation = default);

    /*问：为什么要将strWindowFeatures参数默认传入noopener？
      答：这是为了强制在不同的进程打开新窗口，这可以避免旧窗口卡死的问题，
      但是，这会导致新窗口和旧窗口无法互相访问，
      如果需要这个功能，请将这个参数显式传入null*/
    #endregion
    #region 关闭窗口
    /// <summary>
    /// 关闭浏览器窗口
    /// </summary>
    /// <param name="cancellation">用来取消异步任务的令牌</param>
    /// <returns></returns>
    ValueTask Close(CancellationToken cancellation = default);
    #endregion
    #region 跳转到指定Uri
    /// <summary>
    /// 跳转到指定Uri
    /// </summary>
    /// <param name="uri">跳转的目标Uri</param>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    async ValueTask Jump(string uri, CancellationToken cancellationToken = default)
         => await Location.Href.Set(uri, cancellationToken);
    #endregion
    #endregion
    #region 打印窗口
    /// <summary>
    /// 打印这个窗口
    /// </summary>
    /// <param name="cancellation">用来取消异步任务的令牌</param>
    /// <returns></returns>
    ValueTask Print(CancellationToken cancellation = default);
    #endregion
    #region 关于通知
    #region 请求通知权限
    /// <summary>
    /// 请求通知权限，
    /// 并返回是否请求成功，
    /// 一定要在手势事件中调用本方法，
    /// 否则可能会被直接拒绝
    /// </summary>
    /// <param name="cancellationToken">一个用来取消异步操作的令牌</param>
    /// <returns></returns>
    Task<bool> RequestNotifications(CancellationToken cancellationToken = default);
    #endregion
    #region 返回通知对象
    /// <summary>
    /// 返回一个可用于执行浏览器通知的对象，
    /// 如果不支持此功能，返回<see langword="null"/>
    /// </summary>
    /// <param name="cancellationToken">一个用来取消异步操作的令牌</param>
    /// <returns></returns>
    Task<INotifications<WebNotificationsOptions>?> Notifications(CancellationToken cancellationToken = default);
    #endregion
    #endregion
}
