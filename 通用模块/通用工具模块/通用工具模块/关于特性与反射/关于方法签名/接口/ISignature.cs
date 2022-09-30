namespace System.Reflection;

/// <summary>
/// 这个接口是方法和构造函数签名的基接口
/// </summary>
public interface ISignature
{
    #region 参数列表
    /// <summary>
    /// 获取方法的参数列表
    /// </summary>
    IReadOnlyList<Type> Parameters { get; }
    #endregion
    #region 判断方法签名是否兼容
    /// <summary>
    /// 判断这个签名和另一个方法签名是否兼容，
    /// 该比较方法参数支持逆变，返回值支持协变
    /// </summary>
    /// <param name="other">要比较的另一个签名</param>
    /// <returns></returns>
    bool IsSame(ISignature other);
    #endregion
}
