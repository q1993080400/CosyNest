using System.Reflection;

namespace System;

public static partial class ExtenReflection
{
    //该部分类专门声明有关类型的反射扩展方法

    #region 获取类型的所有基类
    /// <summary>
    /// 按照继承树中从下往上的顺序，枚举一个类型的所有基类
    /// </summary>
    /// <param name="type">要枚举基类的类型</param>
    /// <param name="stop">枚举将在到达这个类型时停止，
    /// 如果为<see langword="null"/>，则默认为到达<see cref="object"/>时停止</param>
    /// <returns></returns>
    public static IEnumerable<Type> BaseTypeAll(this Type type, Type? stop = null)
    {
        while (true)
        {
            type = type.BaseType!;
            if (type is null || type == stop)
                yield break;
            yield return type;
        }
    }
    #endregion
    #region 对类型的判断
    #region 判断一个类型是否具有无参数构造函数
    /// <summary>
    /// 如果一个类型具有公开的无参数构造函数，且可以实例化，
    /// 则返回<see langword="true"/>，否则返回<see langword="false"/>
    /// </summary>
    /// <param name="type">要判断的类型</param>
    /// <returns></returns>
    public static bool CanNew(this Type type)
        => type.IsValueType ||
        (!type.IsAbstract && !type.IsInterface && !type.ContainsGenericParameters &&
        type.GetConstructors().Any(x => x.IsPublic && x.GetParameters().Length is 0));
    #endregion
    #region 判断一个类型是否为静态类
    /// <summary>
    /// 判断一个类型是否为静态类
    /// </summary>
    /// <param name="type">待判断的类型</param>
    /// <returns></returns>
    public static bool IsStatic(this Type type)
        => type.IsSealed && type.IsAbstract;
    #endregion
    #region 判断一个对象是否能够赋值给一个类型
    /// <summary>
    /// 确定指定的实例是否能赋值给当前类型的变量
    /// </summary>
    /// <param name="type">被赋值的类型</param>
    /// <param name="obj">赋值给指定类型变量的对象</param>
    /// <returns></returns>
    public static bool IsAssignableFrom(this Type type, object? obj)
        => obj is null ? type.CanNull() : type.IsAssignableFrom(obj.GetType());
    #endregion
    #region 判断一个类型是否可空
    /// <summary>
    /// 判断一个类型是否可空
    /// </summary>
    /// <param name="type">待判断的类型</param>
    /// <returns></returns>
    public static bool CanNull(this Type type)
        => (type.IsClass && !type.IsStatic()) || type.IsInterface ||        //注意：如果某类型为引用类型，但它是静态类，则仍然认为它不可空
            type.IsGenericRealize(typeof(Nullable<>));
    #endregion
    #region 判断类型是否为数字
    /// <summary>
    /// 返回一个类型是否为数字
    /// </summary>
    /// <param name="type">待检查的类型</param>
    /// <returns></returns>
    public static bool IsNum(this Type type)
        => Type.GetTypeCode(type) is not (TypeCode.Object or TypeCode.DBNull or
        TypeCode.Empty or TypeCode.DateTime or
        TypeCode.Char or TypeCode.String or TypeCode.Boolean);
    #endregion
    #region 关于泛型类型
    #region 判断泛型类型实现和定义之间的关系
    /// <summary>
    /// 判断一个泛型类型，是否为另一个泛型类型定义的实现，
    /// 类似List&lt;int&gt;和<see cref="List{T}"/>的关系
    /// </summary>
    /// <param name="type">泛型类型实现</param>
    /// <param name="definition">要检查的泛型类型定义</param>
    /// <returns></returns>
    public static bool IsGenericRealize(this Type type, Type definition)
       => type.IsConstructedGenericType &&
        definition.IsGenericTypeDefinition &&
        type.GetGenericTypeDefinition() == definition;
    #endregion
    #region 判断是否实现泛型
    /// <summary>
    /// 返回类型<paramref name="type"/>是否继承或实现了另一个泛型类型<paramref name="generic"/>，
    /// <paramref name="generic"/>可以为开放式泛型
    /// </summary>
    /// <param name="type">要检查的类型</param>
    /// <param name="generic">要检查是否继承或实现的泛型类型</param>
    /// <returns>一个元组，它的项分别是是否继承或实现泛型类型，
    /// 以及如果继承或实现了，该泛型类型的泛型参数</returns>
    public static (bool IsRealize, Type[] GenericParameter) IsRealizeGeneric(this Type type, Type generic)
    {
        #region 本地函数
        (bool IsRealize, Type[] GenericParameter) Fun(IEnumerable<Type> types)
        {
            var realize = types.Prepend(type).FirstOrDefault(x => x == generic || x.IsGenericRealize(generic));
            return realize is null ?
                (false, Array.Empty<Type>()) :
                (true, realize.GetGenericArguments());
        }
        #endregion
        return generic switch
        {
            { IsGenericType: true } => Fun(type.BaseTypeAll()) switch
            {
                (true, { } parameter) => (true, parameter),
                _ => Fun(type.GetInterfaces())
            },
            _ => throw new ArgumentException($"{generic}不是泛型类型")
        };
    }
    #endregion
    #region 返回集合的元素类型
    /// <summary>
    /// 如果一个类型是集合，则返回它的元素类型，
    /// 否则返回<see langword="null"/>
    /// </summary>
    /// <param name="type">要检查的类型</param>
    /// <returns></returns>
    public static Type? GetCollectionElementType(this Type type)
    {
        if (type.IsArray)
            return type.GetElementType();
        if (type.IsRealizeGeneric(typeof(IEnumerable<>)) is (true, { } elementType))
            return elementType.First();
        if (typeof(Collections.IEnumerable).IsAssignableFrom(type))
            return typeof(object);
        return null;
    }
    #endregion
    #endregion
    #endregion
    #region 获取一个对象的类型数据
    /// <summary>
    /// 对一个对象或类型进行反射，返回它的<see cref="ITypeData"/>
    /// </summary>
    /// <param name="obj">如果这个对象是<see cref="Type"/>，则获取它本身，
    /// 如果不是，则调用它的<see cref="object.GetType"/>方法，获取它的<see cref="Type"/></param>
    /// <returns></returns>
    public static ITypeData GetTypeData(this object obj)
        => TypeData.TypeCache[obj is Type t ? t : obj.GetType()];
    #endregion
    #region 增强版获取对象类型
    /// <summary>
    /// 如果一个对象是<see cref="Type"/>，则返回它本身，
    /// 如果不是，则调用<see cref="object.GetType"/>获取它的类型
    /// </summary>
    /// <param name="obj">要获取类型的对象</param>
    /// <returns></returns>
    internal static Type GetTypeObj(this object obj)
        => obj is Type a ? a : obj.GetType();
    #endregion
    #region 获取是否存在某个特性
    /// <summary>
    /// 获取一个成员是否存在某个特性
    /// </summary>
    /// <typeparam name="Attribute">特性的类型</typeparam>
    /// <param name="member">要搜索特性的成员</param>
    /// <returns></returns>
    /// <inheritdoc cref="MemberInfo.IsDefined(Type, bool)"/>
    public static bool IsDefined<Attribute>(this MemberInfo member, bool inherit = true)
        where Attribute : System.Attribute
        => member.IsDefined(typeof(Attribute), inherit);
    #endregion
}
