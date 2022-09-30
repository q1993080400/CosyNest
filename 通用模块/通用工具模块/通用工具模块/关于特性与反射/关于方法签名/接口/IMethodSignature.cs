namespace System.Reflection;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个方法的签名
/// </summary>
public interface IMethodSignature : ISignature
{
    #region 返回值类型
    /// <summary>
    /// 获取方法的返回值类型
    /// </summary>
    Type Return { get; }
    #endregion
    #region 判断方法签名是否兼容
    /// <inheritdoc cref="ISignature.IsSame(ISignature)"/>
    /// <inheritdoc cref="CreateReflection.MethodSignature(Type?, object[])"/>
    bool IsSame(Type? @return, params object[] parameters)
        => IsSame(CreateReflection.MethodSignature(@return, parameters));
    #endregion
}
