using System.Diagnostics.CodeAnalysis;

namespace System;

/// <summary>
/// 强化的延迟计算对象，能够自动进行一些转换，
/// 比原生版本兼容性更高
/// </summary>
/// <typeparam name="Values">延迟计算对象所封装的对象类型</typeparam>
public sealed class LazyPro<Values>
{
    #region 说明文档
    /*对使用本类型的说明：
      凡是方法中只在部分分支使用的参数，
      都建议使用本类型进行封装*/
    #endregion
    #region 隐式转换
    public static implicit operator LazyPro<Values>(Func<Values> @delegate)
        => new(@delegate);

    [return: NotNullIfNotNull("value")]
    public static implicit operator LazyPro<Values>?(Values? value)
        => value is null ? null : new(value);

    public static implicit operator Values?(LazyPro<Values>? lazy)
        => lazy is null ? default : lazy.Value;
    #endregion
    #region 延迟获取返回值
    #region 缓存延迟对象的返回值
    /// <summary>
    /// 缓存延迟对象的返回值
    /// </summary>
    private Values? CacheValue { get; set; }
    #endregion
    #region 用来初始化延迟对象的委托
    /// <summary>
    /// 调用这个委托以获取延迟对象的返回值
    /// </summary>
    private Func<Values>? GetLazy { get; set; }
    #endregion
    #region 正式属性
    /// <summary>
    /// 获取延迟对象的返回值
    /// </summary>
    public Values Value
    {
        get
        {
            if (GetLazy is { })
            {
                lock (GetLazy)
                {
                    CacheValue = GetLazy();
                    GetLazy = null;
                }
            }
            return CacheValue!;
        }
    }
    #endregion
    #endregion
    #region 构造方法
    #region 使用委托
    /// <summary>
    /// 使用指定的委托创建延迟对象
    /// </summary>
    /// <param name="delegate">指定的委托，它用来获取延迟计算的值</param>
    public LazyPro(Func<Values> @delegate)
    {
        GetLazy = @delegate;
    }
    #endregion
    #region 使用值
    /// <summary>
    /// 使用指定的值创建延迟对象
    /// </summary>
    /// <param name="value">指定的值</param>
    public LazyPro(Values value)
    {
        CacheValue = value;
    }

    /*问：这个构造方法看上去有些多余，为什么需要保留？
      答：如果一个方法有一个Lazy参数，
      那么这个参数既可以填入一个委托，也可以直接填入一个值，
      这样能够提高兼容性*/
    #endregion
    #endregion
}
#region 辅助类型
/// <summary>
/// 这个类型用来帮助创建延迟对象
/// </summary>
public static class LazyPro
{
    #region 通过委托创建一个延迟计算
    /// <summary>
    /// 通过委托创建一个延迟对象，
    /// 并可以自动推断出这个延迟对象的泛型参数
    /// </summary>
    /// <typeparam name="Values">延迟对象封装的值的类型</typeparam>
    /// <param name="delegate">用来获取延迟值的委托</param>
    /// <returns></returns>
    public static LazyPro<Values> Create<Values>(Func<Values> @delegate)
        => @delegate;
    #endregion
}
#endregion