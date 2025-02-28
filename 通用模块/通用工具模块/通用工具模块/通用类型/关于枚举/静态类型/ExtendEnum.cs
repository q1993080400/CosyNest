using System.Reflection;

namespace System;

public static partial class ExtendTool
{
    //这个部分类专门用于储存与对象转换有关的扩展方法

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
    #region 关于枚举
    #region 获取枚举的值和描述
    /// <summary>
    /// 获取一个迭代器，它枚举枚举的值以及描述
    /// </summary>
    /// <param name="type">枚举的类型，
    /// 如果它不是枚举，则返回一个空集合</param>
    /// <returns></returns>
    public static IEnumerable<(Enum Value, string Describe)> GetEnumDescription(this Type type)
    {
        if (!type.IsEnum)
            yield break;
        foreach (var field in type.GetFields().Where(static x => x.IsStatic))
        {
            var @enum = (Enum)field.GetValue(null)!;
            var renderDataName = field.GetCustomAttribute<EnumDescribeAttribute>()?.Describe;
            yield return (@enum, renderDataName ?? @enum.ToString());
        }
    }
    #endregion
    #region 获取枚举的描述
    /// <summary>
    /// 获取枚举的描述，
    /// 如果没有描述，返回枚举的字面量
    /// </summary>
    /// <param name="enum">待返回描述的枚举</param>
    /// <returns></returns>
    public static string GetDescription(this Enum @enum)
    {
        var description = @enum.GetType().GetField(@enum.ToString())?.
            GetCustomAttribute<EnumDescribeAttribute>();
        return description?.Describe ?? @enum.ToString();
    }
    #endregion
    #endregion
}
