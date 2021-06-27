using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace Microsoft.AspNetCore.SignalR
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以用来提供SignalR连接
    /// </summary>
    public interface ISignalRProvide : IAsyncDisposable
    {
        #region 获取SignalR连接
        /// <summary>
        /// 获取连接到指定Uri的SignalR连接，
        /// 当<see cref="Task{TResult}"/>被等待完毕时，
        /// 连接已经启动完成，可以直接使用
        /// </summary>
        /// <param name="uri">SignalR中心的Uri，
        /// 它可以是相对的，也可以是绝对的</param>
        /// <returns></returns>
        Task<HubConnection> GetConnection(string uri);
        #endregion
        #region 设置用于配置SignalR连接的委托
        /// <summary>
        /// 设置用于配置SignalR连接的委托，
        /// 这个方法只能调用一次
        /// </summary>
        /// <param name="configuration">用于配置连接的委托，
        /// 它的第一个参数是待配置的连接，第二个参数是该连接所对应的中心的Uri，
        /// 第三个参数是一个可以将相对Uri和绝对Uri互相转换的对象，该委托一般用来注册客户端方法</param>
        void SetConfiguration(Action<HubConnection, string, NavigationManager> configuration);
        #endregion
    }
}
