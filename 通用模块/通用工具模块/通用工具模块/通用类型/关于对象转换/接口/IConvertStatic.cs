namespace System;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以执行一个静态的投影，
/// 将一个类型转换为另一个类型
/// </summary>
/// <typeparam name="From">转换的源类型</typeparam>
/// <typeparam name="To">转换的目标类型</typeparam>
public interface IConvertStatic<in From, out To>
{
    #region 转换目标
    /// <summary>
    /// 将源对象转换为目标类型
    /// </summary>
    /// <param name="from">源对象</param>
    /// <param name="serviceProvider">服务请求者对象，
    /// 如果不需要，可以为<see langword="null"/></param>
    /// <returns></returns>
    abstract static To Convert(From from, IServiceProvider? serviceProvider = null);
    #endregion
}
