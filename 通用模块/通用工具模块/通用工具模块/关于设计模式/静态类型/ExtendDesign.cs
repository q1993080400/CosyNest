using Microsoft.Extensions.DependencyInjection;

namespace System.Design;

/// <summary>
/// 有关设计模式的扩展方法全部放在这里
/// </summary>
public static class ExtendDesign
{
    #region 注入事件总线工厂
    /// <summary>
    /// 以单例模式注入一个<see cref="IEventBusFactory"/>，
    /// 它可以用来创建事件总线上下文
    /// </summary>
    /// <param name="services">要注入服务的容器</param>
    /// <returns></returns>
    public static IServiceCollection AddEventBusFactory(this IServiceCollection services)
        => services.AddSingleton<IEventBusFactory, EventBusFactory>();
    #endregion
}
