using System.Design.Async;
using System.Threading.Tasks;

namespace Microsoft.JSInterop
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个JS中的Location对象
    /// </summary>
    public interface IJSLocation
    {
        #region 关于Uri
        #region 获取或设置当前Uri
        /// <summary>
        /// 通过这个异步属性，
        /// 可以获取或设置当前Uri，
        /// 在写入这个属性时，可以写入相对或绝对Uri
        /// </summary>
        IAsyncProperty<string> Href { get; }
        #endregion
        #region 获取主机名称
        /// <summary>
        /// 获取主机名称和端口号
        /// </summary>
        ValueTask<string> Host { get; }
        #endregion
        #region 获取协议部分
        /// <summary>
        /// 获取Uri的协议部分
        /// </summary>
        ValueTask<string> Protocol { get; }
        #endregion
        #endregion
        #region 刷新页面
        /// <summary>
        /// 刷新当前页面
        /// </summary>
        /// <param name="forceGet">如果该参数为<see langword="true"/>，
        /// 则绕过缓存，直接从服务器下载页面</param>
        /// <returns></returns>
        ValueTask Reload(bool forceGet = false);
        #endregion
    }
}
