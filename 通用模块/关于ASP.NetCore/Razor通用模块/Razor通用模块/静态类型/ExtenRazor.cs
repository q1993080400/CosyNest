using System.Design;
using System.Design.Async;
using System.NetFrancis.Http;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace System
{
    /// <summary>
    /// 有关Razor的扩展方法全部放在这里
    /// </summary>
    public static class ExtenRazor
    {
        #region 有关JS互操作
        #region 调用eval方法
        #region 说明文档
        /*问：JS函数很多，为什么需要为执行eval专门声明一个扩展方法？
          答：因为这个函数赋予了以字符串的方式执行任意JS代码的能力，
          这对于JS互操作来说非常重要且非常灵活，有必要予以特殊待遇*/
        #endregion
        #region 无返回值
        /// <summary>
        /// 通过JS互操作调用eval方法，
        /// 并执行JS代码
        /// </summary>
        /// <param name="js">执行JS代码的运行时</param>
        /// <param name="jsCode">要执行的JS代码</param>
        /// <returns></returns>
        public static ValueTask InvokeCodeVoidAsync(this IJSRuntime js, string jsCode)
            => js.InvokeVoidAsync("eval", $"{jsCode}");
        #endregion
        #region 有返回值
        /// <summary>
        /// 通过JS互操作调用eval方法，
        /// 并执行JS代码，然后返回返回值
        /// </summary>
        /// <typeparam name="Ret">返回值类型</typeparam>
        /// <param name="js">执行JS代码的运行时</param>
        /// <param name="jsCode">要执行的JS代码</param>
        /// <returns></returns>
        public static ValueTask<Ret> InvokeCodeAsync<Ret>(this IJSRuntime js, string jsCode)
            => js.InvokeAsync<Ret>("eval", $"{jsCode}");
        #endregion
        #endregion
        #region 有关JS属性
        #region 读取属性
        /// <summary>
        /// 读取一个JS属性
        /// </summary>
        /// <typeparam name="Property">属性的类型</typeparam>
        /// <param name="js">JS运行时对象</param>
        /// <param name="property">用来访问属性的表达式，它相对于Window对象</param>
        /// <returns></returns>
        public static ValueTask<Property> GetProperty<Property>(this IJSRuntime js, string property)
            => js.InvokeCodeAsync<Property>($"{property}");
        #endregion
        #region 写入属性
        /// <summary>
        /// 写入一个JS属性
        /// </summary>
        /// <param name="js">JS运行时对象</param>
        /// <param name="property">用来访问属性的表达式，它相对于Window对象</param>
        /// <param name="value">待写入的属性的值</param>
        /// <returns></returns>
        public static ValueTask SetProperty(this IJSRuntime js, string property, object? value)
        {
            value = value switch
            {
                string s => $"\"{s}\"",
                null => "null",
                var v => v
            };
            return js.InvokeCodeVoidAsync($"{property}={value}");
        }
        #endregion
        #region 封装JS异步属性
        /// <summary>
        /// 将一个JS属性封装为异步属性
        /// </summary>
        /// <typeparam name="Obj">异步属性的类型</typeparam>
        /// <param name="js">JS运行时对象</param>
        /// <param name="property">用来访问属性的表达式，它相对于Window对象</param>
        /// <returns></returns>
        public static IAsyncProperty<Obj> PackProperty<Obj>(this IJSRuntime js, string property)
            => CreateDesign.AsyncProperty(
                () => js.GetProperty<Obj>(property).AsTask(),
                value => js.SetProperty(property, value).AsTask());
        #endregion
        #endregion
        #endregion
        #region 关于依赖注入
        #region 注入UriFrancis
        /// <summary>
        /// 以范围模式添加一个<see cref="UriAbsolute"/>，
        /// 它的<see cref="UriAbsolute.UriBase"/>已经填入本地主机的URI
        /// </summary>
        /// <param name="services">待注入的服务容器</param>
        /// <returns></returns>
        public static IServiceCollection AddLocalUri(this IServiceCollection services)
            => services.AddScoped(x => new UriAbsolute()
            { UriBase = x.GetRequiredService<NavigationManager>().BaseUri });
        #endregion
        #region 注入前端对象
        /// <summary>
        /// 向服务容器注入常用前端对象
        /// </summary>
        /// <param name="services">待注入的服务容器</param>
        /// <returns></returns>
        public static IServiceCollection AddFront(this IServiceCollection services)
        {
            services.AddJSWindow();
            return services;
        }
        #endregion
        #region 注入IJSWindow
        /// <summary>
        /// 向服务容器注入一个<see cref="IJSWindow"/>
        /// </summary>
        /// <param name="services">待注入的服务容器</param>
        /// <returns></returns>
        public static IServiceCollection AddJSWindow(this IServiceCollection services)
            => services.AddScoped<IJSWindow, JSWindow>();
        #endregion
        #region 注入ISignalRProvide
        /// <summary>
        /// 以暂时模式注册一个<see cref="ISignalRProvide"/>服务
        /// </summary>
        /// <param name="services">要注册的服务容器</param>
        /// <param name="create">通过Uri创建<see cref="HubConnection"/>的委托，
        /// 如果为<see langword="null"/>，则使用默认方式</param>
        /// <returns></returns>
        public static IServiceCollection AddSignalRProvide(this IServiceCollection services, Func<string, HubConnection>? create = null)
        {
            create ??= static x => new HubConnectionBuilder().
               WithUrl(x).
               AddJsonProtocol(x => x.AddFormatterJson()).
               Build();
            return services.AddTransient<ISignalRProvide>(x => new SignalRProvide(create, x.GetRequiredService<NavigationManager>()));
        }
        #endregion
        #endregion
        #region 公开StateHasChanged方法
        private static MethodInfo StateHasChangedMethod { get; }
        = typeof(ComponentBase).GetTypeData().MethodFind("StateHasChanged");

        /// <summary>
        /// 调用一个<see cref="ComponentBase"/>的StateHasChanged方法
        /// </summary>
        /// <param name="component">要调用方法的<see cref="ComponentBase"/></param>
        public static void StateHasChanged(this ComponentBase component)
            => StateHasChangedMethod.Invoke<object>(component);
        #endregion
    }
}
