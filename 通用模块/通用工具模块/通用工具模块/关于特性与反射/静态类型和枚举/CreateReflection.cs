namespace System.Reflection;

/// <summary>
/// 这个静态类可以用来帮助创建和反射有关的对象
/// </summary>
public static class CreateReflection
{
    #region 创建方法签名
    #region 直接创建
    /// <summary>
    /// 通过参数和返回值类型创建一个方法签名
    /// </summary>
    /// <param name="return">返回值类型，如果为<see langword="null"/>，则为<see cref="void"/></param>
    /// <param name="parameters">方法的参数列表，如果它的元素不是<see cref="Type"/>，
    /// 则调用<see cref="object.GetType"/>将其转换为<see cref="Type"/></param>
    public static IMethodSignature MethodSignature(Type? @return, params object[] parameters)
        => new MethodSignature(@return, parameters);
    #endregion
    #region 无参数无返回值的签名
    /// <summary>
    /// 返回无参数无返回值的方法签名
    /// </summary>
    public static IMethodSignature MethodNoParameters { get; } = new MethodSignature(null, []);
    #endregion
    #region 创建委托的签名
    /// <summary>
    /// 创建一个方法签名，该签名的参数和返回值与一个委托签名相同
    /// </summary>
    /// <typeparam name="DelegatesType">委托类型</typeparam>
    /// <returns></returns>
    public static IMethodSignature MethodSignature<DelegatesType>()
        where DelegatesType : Delegate
        => typeof(DelegatesType).GetMethod("Invoke")!.GetSignature();
    #endregion
    #endregion
    #region 创建构造函数签名
    #region 直接创建
    /// <summary>
    /// 通过参数创建一个构造函数签名
    /// </summary>
    /// <param name="parameters">构造函数的参数列表，如果它的元素不是<see cref="Type"/>，
    /// 则调用<see cref="object.GetType"/>将其转换为<see cref="Type"/></param>
    /// <returns></returns>
    public static IConstructSignature ConstructSignature(params object[] parameters)
        => new ConstructSignature(parameters);
    #endregion
    #region 返回无参数构造函数签名
    /// <summary>
    /// 返回无参数构造函数的签名
    /// </summary>
    public static IConstructSignature ConstructNoParameters { get; } = new ConstructSignature([]);
    #endregion
    #endregion
}
