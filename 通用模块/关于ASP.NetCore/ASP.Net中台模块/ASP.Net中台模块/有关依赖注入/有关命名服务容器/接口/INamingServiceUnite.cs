namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 凡是实现这个接口的类型，作为适用于任何作用域的命名服务容器
/// </summary>
/// <inheritdoc cref="INamingService{Service}"/>
public interface INamingServiceUnite<Service> : INamingServiceTransient<Service>, INamingServiceScoped<Service>, INamingServiceSingleton<Service>
    where Service : class
{
    #region 说明文档
    /*问：本接口同时实现三个服务作用域的接口，
      那么，它是否违反对象职责明确原则？
      答：不会，本接口的目的在于：
      需要将命名服务提供接口按照作用域划分，以避免混乱，
      但是，它们之间也就只有这一个区别了，
      这个区别在开发服务的时候没有意义，只在注入和请求服务的时候有意义，
      因此作者建议的方式是：
      所有实现INamingService接口的对象，同时也实现本接口，
      这样一来，它可以作为任何生存期服务注册到服务容器中，
      但是注入服务的时候使用具体类型，举例说明：
      某一INamingServiceUnite作为瞬时服务注册，它注册的服务类型就是INamingServiceTransient，
      作为单一服务注册，它的服务类型就是INamingServiceSingleton*/
    #endregion
}
