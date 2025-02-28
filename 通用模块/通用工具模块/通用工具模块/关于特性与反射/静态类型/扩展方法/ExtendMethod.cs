using System.Reflection;

namespace System;

public static partial class ExtendReflection
{
    //这个部分类专门声明有关方法，委托，构造函数和事件的扩展方法

    #region 关于委托
    #region 返回一个类型是否为委托
    /// <summary>
    /// 判断一个类型是否为委托
    /// </summary>
    /// <param name="type">待判断的类型</param>
    /// <returns></returns>
    public static bool IsDelegate(this Type type)
        => typeof(Delegate).IsAssignableFrom(type);
    #endregion
    #region 返回一个委托的方法
    /// <summary>
    /// 返回一个委托Invoke方法的<see cref="MethodInfo"/>，
    /// 它可以用来检查委托的参数和返回值等情况
    /// </summary>
    /// <param name="type">要检查的委托</param>
    /// <returns></returns>
    public static MethodInfo GetDelegateMethod(this Type type)
        => type.IsDelegate() ?
        type.GetMethod(nameof(Action.Invoke))! :
        throw new NotSupportedException($"类型{type}不是委托");
    #endregion
    #endregion
    #region 方法和构造函数共用
    #region 获取所有参数类型
    /// <summary>
    /// 获取一个方法或构造函数的所有参数类型
    /// </summary>
    /// <param name="method">要获取参数类型的方法或构造函数</param>
    /// <returns></returns>
    public static Type[] GetParameterTypes(this MethodBase method)
        => [.. method.GetParameters().Select(static p => p.ParameterType)];
    #endregion
    #region 判断方法是否与指定的签名兼容
    #region 仅指定方法参数
    /// <summary>
    /// 判断方法是否与指定的签名兼容
    /// </summary>
    /// <param name="method">要判断的方法</param>
    /// <param name="parameterType">方法参数的类型</param>
    /// <returns></returns>
    public static bool IsSame(this MethodBase method, Type[] parameterType)
    {
        var methodParameter = method.GetParameterTypes();
        return methodParameter.Length == parameterType.Length &&
            methodParameter.Zip(parameterType).All(static x =>
            {
                var (fromMethod, fromExternal) = x;
                return fromMethod.IsAssignableFrom(fromExternal);
            });
    }
    #endregion
    #region 指定方法参数和返回值
    /// <summary>
    /// 判断方法是否与指定的签名和返回值兼容
    /// </summary>
    /// <param name="returnType">方法的返回值</param>
    /// <returns></returns>
    /// <inheritdoc cref="IsSame(MethodBase, Type[])"/>
    public static bool IsSame(this MethodInfo method, Type returnType, Type[] parameterType)
        => returnType.IsAssignableFrom(method.ReturnType) && method.IsSame(parameterType);
    #endregion
    #endregion
    #endregion
    #region 关于方法
    #region 调用方法
    /// <summary>
    /// 调用一个<see cref="MethodInfo"/>
    /// </summary>
    /// <typeparam name="Ret">返回值类型</typeparam>
    /// <param name="method">要调用的方法</param>
    /// <param name="target">调用方法的目标，如果是静态方法，则为<see langword="null"/></param>
    /// <param name="parameters">方法的参数列表</param>
    /// <returns>方法的返回值</returns>
    public static Ret Invoke<Ret>(this MethodInfo method, object? target, params object?[] parameters)
        => (Ret)method.Invoke(target, parameters)!;
    #endregion
    #region 递归获取方法
    /// <summary>
    /// 递归获取类型的方法，
    /// 如果该类型是一个接口，
    /// 它可以保证接口能够正常获取基接口的所有方法
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="GetMemberInfoRecursion{Member}(Type, Func{Type, BindingFlags, Member[]}, BindingFlags)"/>
    public static MethodInfo[] GetMethodInfoRecursion(this Type type, BindingFlags bindingFlags = CreateReflection.BindingFlagsAll)
        => type.GetMemberInfoRecursion(static (type, bindingFlags) => type.GetMethods(bindingFlags), bindingFlags);
    #endregion
    #region 判断方法是否具有返回值
    /// <summary>
    /// 如果这个方法没有返回值，是void方法，
    /// 则返回<see langword="true"/>，
    /// 否则返回<see langword="false"/>
    /// </summary>
    /// <param name="method">要判断的方法</param>
    /// <returns></returns>
    public static bool IsVoid(this MethodInfo method)
        => method.ReturnType == typeof(void);
    #endregion
    #endregion
    #region 关于构造函数
    #region 调用构造函数
    /// <summary>
    /// 调用一个构造函数
    /// </summary>
    /// <typeparam name="Ret">返回值类型</typeparam>
    /// <param name="constructor">要调用的构造函数，如果它是静态构造函数，则引发异常</param>
    /// <param name="parameters">构造函数的参数列表</param>
    /// <returns>调用构造函数所构造出的对象</returns>
    public static Ret Invoke<Ret>(this ConstructorInfo constructor, params object?[] parameters)
        => constructor.IsStatic ?
        throw new ArgumentException("无法通过静态构造函数构造对象") :
        (Ret)constructor.Invoke(parameters);
    #endregion
    #endregion
    #region 关于事件
    #region 递归获取事件
    /// <summary>
    /// 递归获取类型的事件，
    /// 如果该类型是一个接口，
    /// 它可以保证接口能够正常获取基接口的所有事件
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="GetMemberInfoRecursion{Member}(Type, Func{Type, BindingFlags, Member[]}, BindingFlags)"/>
    public static EventInfo[] GetEventInfoRecursion(this Type type, BindingFlags bindingFlags = CreateReflection.BindingFlagsAll)
        => type.GetMemberInfoRecursion(static (type, bindingFlags) => type.GetEvents(bindingFlags), bindingFlags);
    #endregion
    #endregion
}
