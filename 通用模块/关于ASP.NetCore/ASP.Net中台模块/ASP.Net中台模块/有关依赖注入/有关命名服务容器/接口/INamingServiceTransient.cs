namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 凡是实现这个接口的类型，都可以根据名称请求瞬时服务
/// </summary>
/// <inheritdoc cref="INamingService{Service}"/>
public interface INamingServiceTransient<Service> : INamingService<Service>
    where Service : class
{

}
