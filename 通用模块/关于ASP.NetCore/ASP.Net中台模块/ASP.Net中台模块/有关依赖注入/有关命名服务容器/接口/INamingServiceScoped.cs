namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 凡是实现这个接口的类型，都可以根据名称请求范围服务
/// </summary>
/// <inheritdoc cref="INamingService{Service}"/>
public interface INamingServiceScoped<Service> : INamingService<Service>
    where Service : class
{

}
