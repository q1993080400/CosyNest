using System.Linq.Expressions;
using System.Performance;
using System.Reflection;

namespace System;

public static partial class ExtenReflection
{
    //这个部分类专门声明有关方法，委托，构造函数和事件的扩展方法

    #region 关于委托
    #region 将一个方法对象转换为委托
    /// <summary>
    /// 将一个<see cref="MethodInfo"/>转换为委托
    /// </summary>
    /// <typeparam name="Del">转换的目标类型</typeparam>
    /// <param name="method">要创建委托的方法</param>
    /// <param name="target">用来执行方法的类型实例</param>
    /// <returns></returns>
    public static Del CreateDelegate<Del>(this MethodInfo method, object? target = null)
        where Del : Delegate
        => (Del)Delegate.CreateDelegate(typeof(Del), target, method);

    /*说明文档：
       即便met是实例方法，target参数仍然可以传入null，
       但是在这种情况下，目标委托类型的第一个参数必须为方法实例类型，举例说明：

       假设类型Class有一个实例方法叫Fun，它有一个int参数，无返回值，
       那么假如target为null，目标委托类型应为Action<Class,int>，
       或其他与之签名兼容的委托类型

       经测试，这个方法支持委托的协变与逆变，
       但是按照C#的规范，值类型不在支持范围之内，
       测试的时候请多加注意*/
    #endregion
    #region 返回一个类型是否为委托
    /// <summary>
    /// 判断一个类型是否为委托
    /// </summary>
    /// <param name="type">待判断的类型</param>
    /// <returns></returns>
    public static bool IsDelegate(this Type type)
        => typeof(Delegate).IsAssignableFrom(type);
    #endregion
    #endregion
    #region 关于方法
    #region 关于签名
    #region 返回参数的类型
    /// <summary>
    /// 返回参数列表中参数的类型
    /// </summary>
    /// <param name="parameter">参数列表</param>
    /// <returns></returns>
    public static Type[] GetParType(this IEnumerable<ParameterInfo> parameter)
        => parameter.Select(x => x.ParameterType).ToArray();
    #endregion
    #region 返回签名
    #region 返回一个方法或构造函数的签名
    #region 缓存字典
    /// <summary>
    /// 这个字典缓存方法的签名，
    /// 以确保相同的方法签名只会初始化一次
    /// </summary>
    private static ICache<MethodBase, ISignature> CacheSignature { get; }
    = CreatePerformance.CacheThreshold(
        x =>
        {
            var par = x.GetParameters().GetParType();
            return x switch
            {
                MethodInfo a => CreateReflection.MethodSignature(a.ReturnType, par),
                ConstructorInfo a => CreateReflection.ConstructSignature(par),
                _ => throw new Exception($"{x}不是方法也不是构造函数"),
            };
        },
        500, CacheSignature);
    #endregion
    #region 返回MethodBase的签名
    /// <summary>
    /// 获取一个方法或构造函数的签名
    /// </summary>
    /// <param name="methodBase">要获取签名的方法或构造函数</param>
    /// <returns></returns>
    public static ISignature GetSignature(this MethodBase methodBase)
        => CacheSignature[methodBase];
    #endregion
    #region 返回方法签名
    /// <summary>
    /// 获取一个方法的签名
    /// </summary>
    /// <param name="method">要获取签名的方法</param>
    /// <returns></returns>
    public static IMethodSignature GetSignature(this MethodInfo method)
        => (IMethodSignature)CacheSignature[method];
    #endregion
    #region 返回构造函数签名
    /// <summary>
    /// 获取一个构造函数的签名
    /// </summary>
    /// <param name="construct">要获取签名的构造函数</param>
    /// <returns></returns>
    public static IConstructSignature GetSignature(this ConstructorInfo construct)
        => (IConstructSignature)CacheSignature[construct];
    #endregion
    #endregion
    #region 返回委托的签名
    #region 传入委托的类型
    /// <param name="delegateType">委托的类型，
    /// 如果它不是委托，会引发一个异常</param>
    /// <inheritdoc cref="GetSignature(Delegate)"/>
    public static IMethodSignature GetSignature(this Type delegateType)
        => delegateType.IsDelegate() ?
        delegateType.GetMethod(nameof(Action.Invoke))!.GetSignature() :
        throw new ArgumentException($"类型{delegateType}不是委托");
    #endregion
    #region 传入委托的实例
    /// <summary>
    /// 返回一个委托的签名
    /// </summary>
    /// <param name="delegate">待返回签名的委托</param>
    /// <returns></returns>
    public static IMethodSignature GetSignature(this Delegate @delegate)
        => @delegate.Method.GetSignature();
    #endregion
    #endregion
    #region 返回表达式树的签名
    /// <summary>
    /// 返回<see cref="LambdaExpression"/>的签名
    /// </summary>
    /// <param name="lambda">要返回签名的表达式</param>
    /// <returns></returns>
    public static IMethodSignature GetSignature(this LambdaExpression lambda)
        => CreateReflection.MethodSignature(lambda.ReturnType, lambda.Parameters.Select(x => x.Type).ToArray());
    #endregion
    #endregion 
    #region 判断方法签名是否兼容
    #region 传入方法签名
    /// <summary>
    /// 判断一个方法或构造函数是否与一个签名兼容，
    /// 该判断参数支持协变，返回值支持逆变
    /// </summary>
    /// <param name="methodBase">要判断的方法</param>
    /// <param name="signature">要检查兼容的签名</param>
    /// <returns></returns>
    public static bool IsSame(this MethodBase methodBase, ISignature signature)
        => methodBase.GetSignature().IsSame(signature);
    #endregion
    #region 传入另一个方法
    /// <summary>
    /// 判断两个方法或构造函数的签名是否兼容，
    /// 该判断参数支持协变，返回值支持逆变
    /// </summary>
    /// <param name="methodA">要判断的第一个方法</param>
    /// <param name="methodB">要判断的第二个方法</param>
    /// <returns></returns>
    public static bool IsSame(this MethodBase methodA, MethodBase methodB)
        => methodA.IsSame(methodB.GetSignature());
    #endregion
    #endregion
    #endregion
    #region 调用方法
    /// <summary>
    /// 调用一个<see cref="MethodInfo"/>
    /// </summary>
    /// <typeparam name="Ret">返回值类型</typeparam>
    /// <param name="method">要调用的方法</param>
    /// <param name="target">调用方法的目标，如果是静态方法，则为<see langword="null"/></param>
    /// <param name="parameters">方法的参数列表</param>
    /// <returns>方法的返回值</returns>
    public static Ret? Invoke<Ret>(this MethodInfo method, object? target, params object?[] parameters)
        => (Ret?)method.Invoke(target, parameters);
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
        => constructor.IsStatic ? throw new ArgumentException("无法通过静态构造函数构造对象") : (Ret)constructor.Invoke(parameters);
    #endregion
    #endregion
    #region 关于事件
    #region 根据布尔值，注册或注销事件
    /// <summary>
    /// 根据一个布尔值，从事件中注册或注销委托
    /// </summary>
    /// <param name="event">待注册或注销委托的事件</param>
    /// <param name="target">事件所依附的对象实例，如果为静态事件，则为<see langword="null"/></param>
    /// <param name="isAdd">如果这个值为<see langword="true"/>，则注册事件，否则注销事件</param>
    /// <param name="delegate">待注册或注销的委托</param>
    public static void AddOrRemove(this EventInfo @event, object? target, bool isAdd, Delegate @delegate)
    {
        if (isAdd)
            @event.AddEventHandler(target, @delegate);
        else @event.RemoveEventHandler(target, @delegate);
    }
    #endregion
    #endregion
}
