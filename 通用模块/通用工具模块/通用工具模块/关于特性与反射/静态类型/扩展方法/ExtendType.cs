namespace System;

public static partial class ExtendReflection
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
        => !type.IsAbstract && !type.IsInterface && !type.ContainsGenericParameters &&
        type.GetConstructors().Any(static x => x.IsPublic && x.GetParameters().Length is 0);
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
    /// 返回一个类型是否为数字,
    /// 注意：它将可空数字视为数字
    /// </summary>
    /// <param name="type">待检查的类型</param>
    /// <returns></returns>
    public static bool IsNum(this Type type)
        => Nullable.GetUnderlyingType(type) is { } underlyingType ?
        underlyingType.IsNum() :
        !type.IsEnum && Type.GetTypeCode(type) is not (TypeCode.Object or TypeCode.DBNull or
          TypeCode.Empty or TypeCode.DateTime or
          TypeCode.Char or TypeCode.String or TypeCode.Boolean);
    #endregion
    #region 判断一个类型是否为通用类型
    /// <summary>
    /// 判断一个类型是否为通用类型，
    /// 通用类型包括数字，布尔，时间，枚举，Guid类型，以及它们的可空版本，
    /// 它们在很多不同的平台上都得到广泛支持
    /// </summary>
    /// <param name="type">要检查的类型</param>
    /// <returns></returns>
    public static bool IsCommonType(this Type? type)
        => type switch
        {
            null => false,
            { IsEnum: true } => true,
            var t => Type.GetTypeCode(t) switch
            {
                TypeCode.Object when Nullable.GetUnderlyingType(t) is { } underlyingType => underlyingType.IsCommonType(),
                TypeCode.Object => t == typeof(DateTimeOffset) || t == typeof(Guid),
                TypeCode.DBNull => false,
                _ => true
            }
        };
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
                (false, []) :
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
    #region 搜索与指定参数类型匹配的构造函数，并创建实例
    /// <summary>
    /// 搜索与指定参数类型匹配的构造函数，
    /// 并调用它，然后创建这个类型的实例
    /// </summary>
    /// <typeparam name="Obj">要创建实例的类型</typeparam>
    /// <param name="type">要搜索构造函数的类型</param>
    /// <param name="parameters">传递给构造函数的参数数组，
    /// 函数会获取它们的类型，以确定应该调用哪个构造函数，
    /// 警告：这个数组的元素不可为<see langword="null"/>，
    /// 否则函数无法获取它的类型</param>
    /// <returns></returns>
    public static Obj ConstructorsInvoke<Obj>(this Type type, params object[] parameters)
        => (Obj)Activator.CreateInstance(type, parameters)!;
    #endregion
}
