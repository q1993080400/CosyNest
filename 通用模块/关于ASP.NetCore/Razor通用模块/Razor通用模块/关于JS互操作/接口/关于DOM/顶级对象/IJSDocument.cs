using System.IOFrancis.Bit;
using System.Text.Json;

using Microsoft.AspNetCore;

namespace Microsoft.JSInterop;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为JS中的Document对象的Net封装，
/// 它可以用来检查和修改DOM
/// </summary>
public interface IJSDocument
{
    #region 返回Cookie对象
    /// <summary>
    /// 返回一个字典，它可以用来索引Cookie
    /// </summary>
    ICookie Cookie { get; }

    /*说明文档
      设置Cookie时需要特别小心，它必须遵循以下原则，
      否则无法被服务器上的HttpRequest.Cookies所读取：
      #Cookie的值必须遵循ASCII编码，但是实际允许的值范围似乎更小，
      因为作者观察到，!无法被识别
      奇怪的是，浏览器本身支持Unicode编码的Cookie，
      无法理解ASP为什么会这么设计*/
    #endregion
    #region 获取或设置标题
    /// <summary>
    /// 通过这个异步属性，
    /// 可以获取或设置窗口的标题
    /// </summary>
    IAsyncProperty<string> Title { get; }
    #endregion
    #region 关于具有ID的控件
    #region 跳转到具有指定ID的元素
    /// <summary>
    /// 跳转到具有指定ID的元素
    /// </summary>
    /// <param name="id">指定的ID</param>
    /// <param name="cancellation">用于取消异步操作的令牌</param>
    /// <returns></returns>
    ValueTask ScrollIntoView(string id, CancellationToken cancellation = default);
    #endregion
    #region 获取具有指定ID的元素
    #region 如果找不到，则返回null
    /// <summary>
    /// 获取具有指定ID的元素，
    /// 如果找不到，返回<see langword="null"/>
    /// </summary>
    /// <inheritdoc cref="ScrollIntoView(string, CancellationToken)"/>
    ValueTask<IElementJS?> GetElementById(string id, CancellationToken cancellation = default);
    #endregion
    #region 如果找不到，则一直寻找，直到超时
    /// <summary>
    /// 获取具有指定ID的元素，
    /// 如果找不到，则一直查找，直到找到或超时
    /// </summary>
    /// <param name="timeOut">超时间隔，
    /// 如果为<see langword="null"/>，默认为1秒</param>
    /// <returns></returns>
    /// <inheritdoc cref="GetElementById(string, CancellationToken)"/>
    async ValueTask<IElementJS> GetElementByIdContinued(string id, TimeSpan? timeOut = null, CancellationToken cancellation = default)
    {
        var @out = timeOut ?? TimeSpan.FromSeconds(1);
        var interval = TimeSpan.FromMilliseconds(50);
        for (int i = 0, max = (int)(@out / interval); i <= max; i++)
        {
            await Task.Delay(interval, cancellation);
            var element = await GetElementById(id, cancellation);
            if (element is { })
                return element;
        }
        throw new TimeoutException("查找元素超时");
    }
    #endregion
    #endregion
    #region 获取具有焦点的元素
    /// <summary>
    /// 获取具有焦点的元素（如果有）
    /// </summary>
    ValueTask<IElementJS?> ActiveElement { get; }
    #endregion
    #endregion
    #region video截图
    /// <summary>
    /// 对一个video进行截图，
    /// 并以管道的形式返回截图结果，
    /// 如果截图未能成功，返回<see langword="null"/>
    /// </summary>
    /// <param name="id">video的id</param>
    /// <param name="format">截图格式</param>
    /// <param name="cancellation">一个用来取消异步操作的令牌</param>
    /// <returns></returns>
    ValueTask<IBitRead?> VideoScreenshot(string id, string format = "png", CancellationToken cancellation = default);
    #endregion
    #region 关于JS调用Net方法
    #region 为JS对象添加Net事件
    /// <summary>
    /// 为JS对象添加使用Net方法实现的事件
    /// </summary>
    /// <param name="id">要添加事件的元素ID，
    /// 如果为<see langword="null"/>，则将其注册到document中，
    /// 也就是它是个全局事件</param>
    /// <param name="eventName">事件的名称</param>
    /// <param name="action">实现事件的Net方法，
    /// 它的参数是一个Json对象，可以用来封装方法参数，
    /// 无论方法参数有几个，该<see cref="JsonElement"/>对象都将被反序列化为数组</param>
    /// <param name="getParameter">这个委托的参数是JS事件方法中事件信息对象的名称，
    /// 返回值是一个迭代器，它的每个元素是一段脚本，用来获取方法的每个参数，
    /// 如果为<see langword="null"/>，表示直接使用JS事件参数</param>
    /// <param name="cancellation">一个用于取消异步操作的令牌</param>
    /// <returns>一个用来释放资源的对象，不要忘记调用它的<see cref="IDisposable.Dispose"/>方法</returns>
    ValueTask<IDisposable> AddEvent(string? id, string eventName, Action<JsonElement> action, Func<string, IEnumerable<string>>? getParameter = null, CancellationToken cancellation = default);
    #endregion
    #region 将Net方法注册为JS方法
    /// <summary>
    /// 将Net方法注册为一个JS方法
    /// </summary>
    /// <typeparam name="Obj">方法的参数类型，
    /// 它只支持<see cref="JsonElement"/>和<see cref="IJSStreamReference"/></typeparam>
    /// <param name="action">待包装的Net方法</param>
    /// <param name="jsMethodName">指定要创建的JS的方法的名字，
    /// 如果为<see langword="null"/>，则自动生成一个不重复的名称</param>
    /// <param name="cancellation">一个用于取消异步操作的令牌</param>
    /// <returns>一个元组，它的项分别是封装完成后的JS方法的名称，以及一个用来释放封装的Net对象的对象</returns>
    ValueTask<(string MethodName, IDisposable Freed)> PackNetMethod<Obj>(Action<Obj> action, string? jsMethodName = null, CancellationToken cancellation = default);
    #endregion
    #endregion
}
