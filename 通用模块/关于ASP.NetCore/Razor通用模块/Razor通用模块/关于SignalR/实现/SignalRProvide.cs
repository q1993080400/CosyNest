using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace Microsoft.AspNetCore.SignalR
{
    /// <summary>
    /// 该类型是<see cref="ISignalRProvide"/>的实现
    /// </summary>
    class SignalRProvide : ISignalRProvide
    {
        #region 有关获取连接
        #region 缓存
        /// <summary>
        /// 该属性按照Uri缓存已经创建的连接
        /// </summary>
        private Dictionary<string, HubConnection> Cache { get; } = new();
        #endregion
        #region 用于创建连接的委托
        /// <summary>
        /// 该委托传入中心的绝对Uri，
        /// 然后创建一个新的<see cref="HubConnection"/>
        /// </summary>
        private Func<string, HubConnection> Create { get; }
        #endregion
        #region 用于转换Uri的对象
        /// <summary>
        /// 这个对象可以将相对和绝对Uri互相转换
        /// </summary>
        private NavigationManager NavigationManager { get; set; }
        #endregion
        #region 正式方法
        public async Task<HubConnection> GetConnection(string uri)
        {
            uri = NavigationManager.ToAbsoluteUri(uri).AbsoluteUri;
            var (exist, value) = Cache.TrySetValue(uri, Create);
            if (!exist)
            {
                if (Configuration is { })
                    Configuration(value, uri, NavigationManager);
                await value.StartAsync();
            }
            return value;
        }
        #endregion
        #endregion
        #region 用于注册客户端方法的委托
        private Action<HubConnection, string, NavigationManager>? Configuration { get; set; }

        public void SetConfiguration(Action<HubConnection, string, NavigationManager> configuration)
        {
            if (this.Configuration is null)
                this.Configuration = configuration;
        }
        #endregion
        #region 释放对象
        public async ValueTask DisposeAsync()
        {
            foreach (var item in Cache.Values)
            {
                await item.DisposeAsync();
            }
        }
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="create">该委托传入中心的绝对Uri，
        /// 然后创建一个新的<see cref="HubConnection"/></param>
        /// <param name="navigationManager">这个对象可以将相对和绝对Uri互相转换</param>
        public SignalRProvide(Func<string, HubConnection> create, NavigationManager navigationManager)
        {
            this.Create = create;
            this.NavigationManager = navigationManager;
        }
        #endregion
    }
}
