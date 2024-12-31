namespace System;

public static partial class ExtendRazor
{
    //这个部分类专门用来声明有关元素编号对象的扩展方法

    #region 添加元素编号对象服务
    /// <summary>
    /// 以瞬时模式注册一个<see cref="IElementNumber"/>服务，
    /// 它可以为待渲染的元素进行编号
    /// </summary>
    /// <param name="services">待添加的服务容器</param>
    /// <returns></returns>
    public static IServiceCollection AddElementNumber(this IServiceCollection services)
        => services.AddTransient(serviceProvider =>
        {
            var js = serviceProvider.GetRequiredService<IJSRuntime>();
            return CreateRazor.ElementNumber(js);
        });
    #endregion
}
