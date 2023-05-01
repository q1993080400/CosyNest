using System.Diagnostics.CodeAnalysis;

namespace System;

/// <summary>
/// 通用工具类
/// </summary>
public static class Tool
{
    #region 拷贝对象
    /// <summary>
    /// 通过反射拷贝对象，并返回它的副本
    /// </summary>
    /// <typeparam name="Ret">拷贝的返回值类型</typeparam>
    /// <param name="obj">被拷贝的对象</param>
    /// <param name="isShallow">如果这个值为真，则执行浅拷贝，否则执行深拷贝</param>
    /// <param name="exception">出现在这个集合中的字段或自动属性名将作为例外，不会被拷贝</param>
    /// <returns></returns>
    [return: NotNullIfNotNull(nameof(obj))]
    public static Ret? Copy<Ret>(Ret? obj, bool isShallow = true, params string[] exception)
    {
        if (obj is null)
            return default;
        var type = obj.GetTypeData();
        var @new = type.ConstructorCreate<Ret>();
        var field = type.Fields.Where(x => !x.IsStatic);               //不拷贝静态属性
        if (exception.Length > 0)
        {
            var fieldName = exception.Concat(exception.Select(x => $"<{x}>k__BackingField").ToArray()).ToHashSet();        //获取属性，以及该自动属性对应的字段名称
            field = field.Where(x => !fieldName.Contains(x.Name));
        }
        field.ForEach(x =>
        {
            var value = x.GetValue(obj);
            x.SetValue(@new,
                isShallow || value is ValueType ? value : Copy(value, isShallow));
        });
        return @new!;
    }

    /*说明文档：
       例外的成员如果是属性，则必须是自动属性才能不被拷贝，
       这是因为自动属性所封装的字段都有固定格式的名称，
       而自己封装的属性的字段名称不确定

       obj必须拥有无参数构造函数，
       但如果Ret是obj的父类，可以没有无参数构造函数，
       这是因为程序实际上是在obj的类型中搜索构造函数*/
    #endregion
    #region 如果为null，则写入，否则抛出异常
    /// <summary>
    /// 如果一个对象为<see langword="null"/>，
    /// 则写入并返回写入后的对象，否则引发一个异常
    /// </summary>
    /// <typeparam name="Obj">对象的类型</typeparam>
    /// <param name="obj">要写入的对象</param>
    /// <param name="set">要写入的新值</param>
    /// <returns></returns>
    public static Obj? IfNotNullSet<Obj>(ref Obj? obj, Obj? set)
        => obj is null ?
        obj = set :
        throw new NotSupportedException("要写入的对象不为null，不允许再次写入");
    #endregion
}
