using System.Reflection;
using System.Runtime.CompilerServices;

namespace System;

/// <summary>
/// 关于反射和程序集的扩展方法，通常无需专门调用
/// </summary>
public static partial class ExtenReflection
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
        => property.GetIndexParameters().Any();
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
        !property.IsIndexing() &&
        property.BackingField() is not { IsInitOnly: true };
    #endregion
    #region 获取自动属性的背景字段
    /// <summary>
    /// 获取自动属性的背景字段，
    /// 如果该属性不是自动属性，则返回<see langword="null"/>
    /// </summary>
    /// <param name="property">要获取背景字段的自动属性</param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"><paramref name="property"/>是全局成员，它没有所属类型</exception>
    /// <exception cref="NotSupportedException">由于派生类隐藏基类的字段，找到了多个字段，无法判断谁是该属性的背景字段</exception>
    public static FieldInfo? BackingField(this PropertyInfo property)
    {
        var type = property.DeclaringType ?? throw new NullReferenceException($"{property}是全局成员，它没有所属类型");
        var fields = type.GetTypeData().FieldDictionary[$"<{property.Name}>k__BackingField"].ToArray();
        return fields.Length switch
        {
            1 => fields[0],
            0 => null,
            _ => throw new NotSupportedException($"由于派生类隐藏基类的字段，找到了多个字段，无法判断谁是该属性的背景字段")
        };
    }
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
    #region 关于属性的值
    #region 返回一个属性的值
    /// <summary>
    /// 通过Get访问器，返回一个属性的值，如果属性不可读，直接报错
    /// </summary>
    /// <typeparam name="Ret">返回值类型，会自动进行转换</typeparam>
    /// <param name="property">要返回值的属性</param>
    /// <param name="source">属性所依附的对象，如果是静态属性，直接忽略</param>
    /// <param name="parameters">属性的参数，如果这个属性不是索引器，则应忽略</param>
    /// <returns></returns>
    public static Ret? GetValue<Ret>(this PropertyInfo property, object? source = null, params object[] parameters)
        => (Ret?)property.GetValue(source, parameters);
    #endregion
    #region 打包属性的Get和Set访问器
    /// <summary>
    /// 打包一个属性的Get访问器和Set访问器，并作为委托返回，
    /// 如果属性不可读或不可写，则对应的访问器返回<see langword="null"/>
    /// </summary>
    /// <typeparam name="Obj">属性所依附的对象类型</typeparam>
    /// <typeparam name="Pro">属性本身的类型</typeparam>
    /// <param name="property">要打包的属性</param>
    /// <returns></returns>
    public static (Func<Obj, Pro>? Get, Action<Obj, Pro>? Set) GetGS<Obj, Pro>(this PropertyInfo property)
    {
        var get = property.GetMethod?.CreateDelegate<Func<Obj, Pro>>();
        var set = property.SetMethod?.CreateDelegate<Action<Obj, Pro>>();
        return (get, set);
    }
    #endregion
    #region 复制属性的值
    /// <summary>
    /// 将一个对象属性的值复制到另一个对象
    /// </summary>
    /// <param name="property">待复制的属性</param>
    /// <param name="source">复制的源对象</param>
    /// <param name="target">复制的目标</param>
    public static void Copy(this PropertyInfo property, object? source, object? target)
        => property.SetValue(target, property.GetValue(source));
    #endregion
    #endregion
    #region 获取虚属性的定义
    /// <summary>
    /// 如果一个属性是重写后的虚属性，
    /// 则返回它的定义，否则返回它自己
    /// </summary>
    /// <param name="property">待检查的属性</param>
    /// <returns></returns>
    public static PropertyInfo GetBaseDefinition(this PropertyInfo property)
    {
        var baseType = property.GetAccessors().First().GetBaseDefinition().DeclaringType!;
        return baseType.GetTypeData().PropertyDictionary[property.Name].Single();
    }
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
    #region 返回成员是否为隐藏
    /// <summary>
    /// 如果此方法返回<see langword="true"/>，
    /// 代表该成员是隐藏的，它由编译器生成，不可直接调用
    /// </summary>
    /// <param name="member">待检查的成员</param>
    /// <returns></returns>
    public static bool IsHide(this MemberInfo member)
        => member switch
        {
            var m when m.Name.StartsWith("<") => true,
            MethodInfo { IsSpecialName: true } => true,
            MethodInfo { Name: "Finalize" } => true,
            _ => false
        };
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
    /// <returns></returns>
    public static bool HasAttributes<Attribute>(this MemberInfo member)
        where Attribute : System.Attribute
        => member.GetCustomAttribute<Attribute>() is { };
    #endregion
    #endregion
}
