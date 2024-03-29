﻿namespace System.Design;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以将其他对象填充到自身中
/// </summary>
/// <typeparam name="Obj">允许填充的对象</typeparam>
public interface IFill<in Obj>
{
    #region 填充对象
    /// <summary>
    /// 将其他对象填充到自身中
    /// </summary>
    /// <param name="obj">要填充的其他对象</param>
    /// <param name="serviceProvider">一个用来请求服务的对象，
    /// 如果不需要，可以为<see langword="null"/></param>
    void Fill(Obj obj, IServiceProvider? serviceProvider);
    #endregion
}
