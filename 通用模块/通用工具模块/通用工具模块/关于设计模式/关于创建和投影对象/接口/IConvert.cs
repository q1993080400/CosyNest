namespace System;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以将一个类型转换为另一种类型
/// </summary>
/// <typeparam name="Target">转换的目标类型</typeparam>
/// <typeparam name="Source">转换的源类型</typeparam>
public interface IConvert<out Target, in Source>
{
    #region 转换对象
    /// <summary>
    /// 将源类型的对象转换为目标类型的对象
    /// </summary>
    /// <param name="source">目标类型的对象</param>
    /// <returns></returns>
    abstract static Target Convert(Source source);
    #endregion
}
