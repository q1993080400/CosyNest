namespace System;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以将自身克隆，
/// 这里的克隆指的是深拷贝
/// </summary>
/// <typeparam name="Obj">要克隆的目标类型</typeparam>
public interface ICloneable<out Obj>
{
    #region 执行克隆
    /// <summary>
    /// 执行克隆，并返回结果
    /// </summary>
    /// <returns></returns>
    Obj Cloneable();
    #endregion
}
