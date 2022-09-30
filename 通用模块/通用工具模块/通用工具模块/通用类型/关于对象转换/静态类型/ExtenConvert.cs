using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace System;

public static partial class ExtenTool
{

    //本部分类专门用于储存与对象转换有关的扩展方法

    #region 转换任意对象
    #region 非泛型方法
    #region 缓存方法属性
    private static MethodInfo? ToMethodField;

    /// <summary>
    /// 该方法缓存<see cref="To{Ret}(object?, bool, LazyPro{Ret}?)"/>的方法对象，
    /// 警告：本方法有关寻找<see cref="MethodInfo"/>的逻辑存在缺陷，可能会产生潜在BUG
    /// </summary>
    private static MethodInfo ToMethod
        => ToMethodField ??= typeof(ExtenTool).GetMethods().First
        (x => x.Name is nameof(To) &&
        x.GetParameters().Length is 3 &&
        x.IsGenericMethod);
    #endregion
    #region 正式方法
    /// <param name="targetType">要转换的对象的类型</param>
    /// <inheritdoc cref="To{Ret}(object?, bool, LazyPro{Ret}?)"/>
    [return: NotNullIfNotNull("obj")]
    public static object? To(this object? obj, Type targetType, bool @throw = true)
        => ToMethod.MakeGenericMethod(targetType).Invoke<object>(null, obj, @throw, null);
    #endregion
    #endregion
    #region 泛型方法
    #region 说明文档
    /*说明文档
      问：为什么这个switch语句不简化为表达式？
      答：这个问题有可能是C#编译器的Bug，
      在某些情况下，它们不是等价的，
      当执行以下代码时，switch表达式会出现问题，但是switch语句不会

      object a = new T1();
      a.To<T1>();

      To方法的简要声明如下，它实际更加复杂，
      但是为了演示方便，在这里去掉了所有无关部分：

      public static Ret To<Ret>(this object obj)
         => obj switch
         {
            Ret r => r,
            var o => (dynamic?)o
         };

      还需满足以下条件：
      T1是一个私有的类型，而且与扩展方法To不在同一个程序集

      函数实际上没有执行到表达式的第二个匹配项，但是仍然会报错，
      这应该与dynamic类型有关，它已经被验证存在类似的问题，
      当去掉第二个匹配项以后，函数不会出现异常*/
    #endregion
    #region 正式方法
    /// <summary>
    /// 将一个对象转换为其他类型，可选是否在转换失败时抛出异常
    /// </summary>
    /// <typeparam name="Ret">返回值类型</typeparam>
    /// <param name="obj">要转换的对象</param>
    /// <param name="throw">如果这个值为<see langword="true"/>，在转换失败时会抛出异常</param>
    /// <param name="notConvert">如果转换失败，而且不抛出异常，则返回这个值，默认为类型默认值</param>
    /// <returns></returns>
    [return: NotNullIfNotNull("obj")]
    public static Ret? To<Ret>(this object? obj, bool @throw = true, LazyPro<Ret>? notConvert = default)
    {
        var type = typeof(Ret);
        try
        {
            switch (obj)
            {
                case Ret o:
                    return o;
                case null when type.CanNull():
                    return default;
                case var o when type == typeof(string):         //为string做特殊处理，因为任何对象都有ToString方法
                    return (dynamic?)o?.ToString()!;
                case { } o when ConvertObject.Conversion.TryGetValue((o.GetType(), type), out var convert) &&       //尝试使用注册的特殊转换方法
                convert.TryConvert(o) is (true, var result):
                    return (Ret)result;
                case { } o when type.IsEnum:                //如果是枚举，则转换它的值或字面量
                    return o is string text ?
                        (Ret)Enum.Parse(type, text) :
                        (Ret)Enum.ToObject(type, o.To(Enum.GetUnderlyingType(type)));
                case IConvertible o:
                    type = type.IsGenericRealize(typeof(Nullable<>)) ?              //如果目标类型是可空值类型，则函数知道应该转换为它的实际类型
                        type.GenericTypeArguments[0] : type;
                    return (Ret)Convert.ChangeType(o, type);
                case var o:                                                         //如果以上转换全部失败，则尝试调用类型自身声明的转换运算符
                    return (Ret?)(dynamic?)o;
            }
        }
        catch (Exception) when (!@throw)
        {
            return notConvert;
        }
    }
    #endregion
    #endregion
    #endregion
    #region 将同步对象转换为异步对象
    #region 转换为Task
    /// <summary>
    /// 将同步对象封装为异步对象
    /// </summary>
    /// <typeparam name="Obj">同步对象的类型</typeparam>
    /// <param name="obj">待封装的同步对象</param>
    /// <returns></returns>
    public static Task<Obj> ToTask<Obj>(this Obj obj)
        => Task.FromResult(obj);
    #endregion
    #region 转换为ValueTask
    /// <inheritdoc cref="ToTask{Obj}(Obj)"/>
    public static ValueTask<Obj> ToValueTask<Obj>(this Obj obj)
        => ValueTask.FromResult(obj);
    #endregion
    #endregion
    #region 转换委托类型
    #region 直接传入新委托的类型
    /// <summary>
    /// 将一个委托转换为另一种委托类型，
    /// 前提条件是这两个委托签名必须相同
    /// </summary>
    /// <param name="oldDelegate">要转换的旧委托</param>
    /// <param name="newDelegateType">新委托的类型</param>
    /// <returns></returns>
    public static Delegate To(this Delegate oldDelegate, Type newDelegateType)
        => Delegate.CreateDelegate(newDelegateType, oldDelegate.Target, oldDelegate.Method);
    #endregion
    #region 使用泛型标明新委托类型
    /// <summary>
    /// 将一个委托转换为另一种委托类型，
    /// 前提条件是这两个委托签名必须相同
    /// </summary>
    /// <typeparam name="NewDel">新委托的类型</typeparam>
    /// <param name="oldDelegate">要转换的旧委托</param>
    /// <returns></returns>
    public static NewDel To<NewDel>(this Delegate oldDelegate)
        where NewDel : Delegate
        => (NewDel)oldDelegate.To(typeof(NewDel));
    #endregion
    #endregion
    #region 将一个枚举转换为和它等效的另一个枚举
    /// <summary>
    /// 将一个枚举转换为和它等效的另一个枚举
    /// </summary>
    /// <typeparam name="To">返回值类型，必须是一个枚举</typeparam>
    /// <param name="fromEnum">要转换的枚举</param>
    /// <returns></returns>
    public static To To<To>(this Enum fromEnum)
        where To : Enum
        => (To)Enum.ToObject(typeof(To), fromEnum);
    #endregion
}
