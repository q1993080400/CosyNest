namespace System.DataFrancis.Verify;

/// <summary>
/// 这个特性是所有验证特性的基类
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public abstract class VerifyAttribute : Attribute
{
    #region 执行验证
    /// <summary>
    /// 执行验证，并返回验证结果
    /// </summary>
    /// <param name="obj">要验证的对象</param>
    /// <param name="describe">对验证对象的描述，
    /// 如果为<see langword="null"/>，表示没有描述</param>
    /// <returns>一个元组，它的项分别是验证是否成功，
    /// 以及如果验证不成功，指示验证失败的原因</returns>
    public abstract (bool IsSuccess, string Message) Verify(object? obj, string? describe = null);
    #endregion
}
