using System.Diagnostics.CodeAnalysis;

namespace System;

/// <summary>
/// 表示一个支持泛型的弱引用
/// </summary>
/// <typeparam name="Obj">弱引用封装的对象类型，必须为引用类型</typeparam>
sealed class WeakReferenceGen<Obj> : WeakReference
    where Obj : class
{
    #region 说明文档
    /*说明文档：
      #Net基础库中有原生的泛型WeakReference，但仍有必要编写这个类型，原因在于：
      WeakReference<T>没有Target属性，要获取被引用的对象十分麻烦，而且禁止继承，无法扩展

      #这个类型的早期版本允许在弱引用目标丢失时，执行一个事件，
      但是在之后被删除，原因在于：
      弱引用对性能十分重视，因此不宜太过复杂，
      否则很可能弱引用的封装，比被封装的内容还要浪费性能

      #推荐使用隐式类型转换，而不是直接使用构造函数来创建本类型，
      因为如果要封装的对象为null，隐式转换后的结果也为null，
      而在执行构造函数的时候，这种情况会引发异常*/
    #endregion
    #region 隐式转换
    public static implicit operator Obj?(WeakReferenceGen<Obj>? a)
        => a?.Target;

    [return: NotNullIfNotNull(nameof(a))]
    public static implicit operator WeakReferenceGen<Obj>?(Obj? a)
        => a is null ? null : new WeakReferenceGen<Obj>(a);
    #endregion
    #region 获取被封装的对象
    /// <summary>
    /// 获取被弱引用封装的对象，注意：
    /// 读取这个值的时候，会获取对象的强引用
    /// </summary>
    public new Obj? Target
        => (Obj?)base.Target;
    #endregion
    #region 重写ToString
    public override string ToString()
        => Target?.ToString() ?? "此对象已被回收";
    #endregion
    #region 构造函数
    /// <summary>
    /// 将指定的对象封装进弱引用中
    /// </summary>
    /// <param name="obj">被封装的对象</param>
    /// <param name="isLong">如果这个值为<see langword="true"/>，创建长弱引用，否则创建短弱引用</param>
    public WeakReferenceGen(Obj obj, bool isLong = false)
        : base(obj, isLong)
    {
        ArgumentNullException.ThrowIfNull(obj);
    }
    #endregion
}
