using System.Reflection;
using System.Runtime.CompilerServices;

namespace System;

/// <summary>
/// 关于反射和程序集的扩展方法，通常无需专门调用
/// </summary>
public static partial class ExtendReflection
{
    //这个部分类专门声明有关字段，属性和成员的扩展方法

    #region 关于字段
    #region 返回一个字段的值
    /// <summary>
    /// 返回一个字段的值，返回值已经经过类型转换
    /// </summary>
    /// <typeparam name="Ret">返回值类型</typeparam>
    /// <param name="field">要返回值的字段</param>
    /// <param name="obj">字段所依附的对象，如果是静态字段或枚举，直接忽略</param>
    /// <returns></returns>
    public static Ret? GetValue<Ret>(this FieldInfo field, object? obj = null)
    {
        var declaringType = field.DeclaringType;
        var value = declaringType is { IsEnum: true } ?
            Enum.Parse(declaringType, field.Name) : field.GetValue(obj);
        return (Ret?)value;
    }
    #endregion
    #endregion
    #region 关于属性
    #region 关于可见性与访问权限
    #region 返回属性的访问权限
    /// <summary>
    /// 如果一个属性只能读取，返回<see langword="true"/>，
    /// 只能写入，返回<see langword="false"/>，既能读取又能写入，返回<see langword="null"/>
    /// </summary>
    /// <param name="property">待获取访问权限的属性</param>
    /// <returns></returns>
    public static bool? GetPermissions(this PropertyInfo property)
    {
        var canRead = property.CanRead;
        var canWrite = property.CanWrite;
        return canRead && canWrite ? null : canRead;
    }
    #endregion
    #region 返回属性是否为Init
    /// <summary>
    /// 返回属性是否为Init，
    /// 如果属性不可写，则返回<see langword="false"/>
    /// </summary>
    /// <param name="property">待判断的属性</param>
    /// <returns></returns>
    public static bool IsInitOnly(this PropertyInfo property)
    {
        if (property.SetMethod is not { } setMethod)
            return false;
        var customModifiers = setMethod.ReturnParameter.GetRequiredCustomModifiers();
        return customModifiers.Contains(typeof(IsExternalInit));
    }
    #endregion
    #region 返回属性的访问修饰符
    /// <summary>
    /// 获取一个属性读写访问器的访问修饰符，
    /// 如果不支持读或写，则该访问器返回<see langword="null"/>
    /// </summary>
    /// <param name="property">待返回访问修饰符的属性</param>
    /// <returns></returns>
    public static (AccessPermissions? Get, AccessPermissions? Set) GetAccess(this PropertyInfo property)
        => (property.GetMethod?.GetAccess(), property.SetMethod?.GetAccess());
    #endregion
    #region 返回属性是否公开
    /// <summary>
    /// 返回一个属性的所有访问器是否全部为Public
    /// </summary>
    /// <param name="property">待检查的属性</param>
    /// <returns></returns>
    public static bool IsPublic(this PropertyInfo property)
    {
        var (get, set) = property.GetAccess();
        static bool Check(AccessPermissions? access)
            => access is null or AccessPermissions.Public;
        return Check(get) && Check(set);
    }
    #endregion
    #endregion
    #region 关于属性的性质
    #region 返回一个属性是否为静态
    /// <summary>
    /// 返回一个属性是否为静态
    /// </summary>
    /// <param name="property">要检查的属性</param>
    /// <returns></returns>
    public static bool IsStatic(this PropertyInfo property)
        => (property.GetMethod?.IsStatic ?? false) ||
            (property.SetMethod?.IsStatic ?? false);
    #endregion
    #region 返回一个属性是否为索引器
    /// <summary>
    /// 返回一个属性是否为索引器
    /// </summary>
    /// <param name="property">要判断的属性</param>
    /// <returns></returns>
    public static bool IsIndexing(this PropertyInfo property)
        => property.GetIndexParameters().Length != 0;
    #endregion
    #region 是否为全能属性
    /// <summary>
    /// 返回某一属性是否为全能属性，
    /// 全能属性指的是可读，可写，公开，且非静态，非索引器的属性，
    /// 注意：init属性不是全能属性
    /// </summary>
    /// <param name="property">待检查的属性</param>
    /// <returns></returns>
    public static bool IsAlmighty(this PropertyInfo property)
        => property.GetPermissions() is null &&
        property.IsPublic() &&
        !property.IsStatic() &&
        !property.IsInitOnly() &&
        !property.IsIndexing();
    #endregion
    #region 返回属性的可为空信息
    /// <summary>
    /// 返回属性的可为空信息
    /// </summary>
    /// <param name="property">要检查的属性</param>
    /// <returns></returns>
    public static NullabilityInfo GetNullabilityInfo(this PropertyInfo property)
        => new NullabilityInfoContext().Create(property);
    #endregion
    #endregion
    #region 返回一个属性的值
    /// <summary>
    /// 通过Get访问器，返回一个属性的值，如果属性不可读，直接报错
    /// </summary>
    /// <typeparam name="Ret">返回值类型，会自动进行转换</typeparam>
    /// <param name="property">要返回值的属性</param>
    /// <param name="source">属性所依附的对象，如果是静态属性，直接忽略</param>
    /// <param name="parameters">属性的参数，如果这个属性不是索引器，则应忽略</param>
    /// <returns></returns>
    public static Ret GetValue<Ret>(this PropertyInfo property, object? source = null, params object[] parameters)
        => (Ret)property.GetValue(source, parameters)!;
    #endregion
    #region 递归获取属性
    /// <summary>
    /// 递归获取类型的属性，
    /// 如果该类型是一个接口，
    /// 它可以保证接口能够正常获取基接口的所有属性
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="GetMemberInfoRecursion{Member}(Type, Func{Type, BindingFlags, Member[]}, BindingFlags)"/>
    public static PropertyInfo[] GetPropertyInfoRecursion(this Type type, BindingFlags bindingFlags = CreateReflection.BindingFlagsAll)
        => type.GetMemberInfoRecursion(static (type, bindingFlags) => type.GetProperties(bindingFlags), bindingFlags);
    #endregion
    #region 获取所有全能属性
    /// <summary>
    /// 获取一个类型中的所有全能属性，
    /// 全能属性指的是可读，可写，公开，且非静态，非索引器的属性
    /// </summary>
    /// <param name="type">要获取全能属性的类型</param>
    /// <param name="includeInit">如果这个值为<see langword="true"/>，
    /// 则包括Init属性，否则排除Init属性</param>
    /// <returns></returns>
    public static PropertyInfo[] GetPropertyInfoAlmighty(this Type type, bool includeInit = false)
        => type.GetPropertyInfoRecursion(BindingFlags.Public | BindingFlags.Instance).
        Where(x => x.CanRead && x.CanWrite && (includeInit || !x.IsInitOnly()) && !x.IsIndexing()).ToArray();
    #endregion
    #endregion
    #region 关于成员
    #region 返回是否为编译器生成
    /// <summary>
    /// 返回该成员是否由编译器自动生成
    /// </summary>
    /// <param name="member">待检查的成员</param>
    /// <returns></returns>
    public static bool IsCompilerGenerated(this MemberInfo member)
        => member.IsDefined(typeof(CompilerGeneratedAttribute));
    #endregion
    #region 获取一个成员的真正类型
    /// <summary>
    /// 获取一个成员的真正类型
    /// </summary>
    /// <param name="member">要获取真正类型的成员</param>
    /// <returns></returns>
    public static Type GetTypeTrue(this MemberInfo member)
        => member.MemberType switch
        {
            MemberTypes.Constructor => typeof(ConstructorInfo),
            MemberTypes.Event => typeof(EventInfo),
            MemberTypes.Field => typeof(FieldInfo),
            MemberTypes.Method => typeof(MethodInfo),
            MemberTypes.Property => typeof(PropertyInfo),
            MemberTypes.TypeInfo => typeof(Type),
            _ => typeof(MemberTypes)     //此为占位，后续会加以修改
        };

    /*需要这个方法的原因在于：
      MemberInfo的GetType方法貌似返回一个动态类型，
      通过它无法方便的判断这个MemberInfo到底是什么类型*/
    #endregion
    #region 获取成员的访问修饰符
    /// <summary>
    /// 获取一个成员的访问修饰符
    /// </summary>
    /// <param name="member">要获取访问修饰符的成员，不支持属性</param>
    /// <returns></returns>
    public static AccessPermissions GetAccess(this MemberInfo member)
    {
        switch (member)
        {
            case Type t:
                return t.IsPublic ? AccessPermissions.Public : AccessPermissions.Internal;
            case EventInfo e:
                return e.AddMethod!.GetAccess();
            case PropertyInfo:
                throw new NotSupportedException("不支持直接获取属性的访问修饰符，因为属性的读写访问器修饰符可能不同");
            default:
                var dy = (dynamic)member;
                if (dy.IsPublic)
                    return AccessPermissions.Public;
                if (dy.IsPrivate)
                    return AccessPermissions.Private;
                if (dy.IsFamily)
                    return AccessPermissions.Protected;
                return dy.IsFamilyAndAssembly ?
                    AccessPermissions.InternalProtected : AccessPermissions.PrivateProtected;
        }
    }
    #endregion
    #region 返回成员是否具有某特性
    /// <summary>
    /// 返回成员是否具有某特性
    /// </summary>
    /// <typeparam name="Attribute">要检查的特性</typeparam>
    /// <param name="member">要检查的成员</param>
    /// <param name="inherit">如果要搜索此成员的继承链以查找属性，
    /// 则为<see langword="true"/>，否则为<see langword="false"/>，
    /// 会忽略属性和事件的此参数</param>
    /// <returns></returns>
    public static bool IsDefined<Attribute>(this MemberInfo member, bool inherit = true)
        where Attribute : System.Attribute
        => member.IsDefined(typeof(Attribute), inherit);
    #endregion
    #region 递归获取类型成员
    /// <summary>
    /// 递归获取类型的成员，
    /// 如果该类型是一个接口，
    /// 它可以保证接口能够正常获取基接口的所有成员
    /// </summary>
    /// <typeparam name="Member">类型成员的类型</typeparam>
    /// <param name="type">要获取成员的类型</param>
    /// <param name="getMember">用来获取成员的委托</param>
    /// <param name="bindingFlags">用来搜索成员的条件</param>
    /// <returns></returns>
    private static Member[] GetMemberInfoRecursion<Member>(this Type type, Func<Type, BindingFlags, Member[]> getMember, BindingFlags bindingFlags)
        where Member : MemberInfo
    {
        var members = getMember(type, bindingFlags);
        if (!type.IsInterface)
            return members;
        foreach (var @base in type.GetInterfaces())
        {
            members = [.. members, .. @base.GetMemberInfoRecursion(getMember, bindingFlags)];
        }
        return members;
    }
    #endregion
    #endregion
}
