namespace System.Reflection;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个构造函数的签名
/// </summary>
public interface IConstructSignature : ISignature
{
    #region 判断方法签名是否兼容
    /// <inheritdoc cref="ISignature.IsSame(ISignature)"/>
    /// <inheritdoc cref="CreateReflection.ConstructSignature(object[])"/>
    bool IsSame(params object[] parameters)
        => IsSame(CreateReflection.ConstructSignature(parameters));
    #endregion
}
