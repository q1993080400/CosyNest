namespace System.DataFrancis;

/// <summary>
/// 这个特性是所有验证特性的基类
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public abstract class VerifyAttribute : Attribute
{
    #region 说明文档
    /*说明文档
      非空验证没有专门的特性负责，
      因为BCL自带的可为空批注已经能够说明这个属性是否可为空*/
    #endregion
    #region 错误信息
    /// <summary>
    /// 获取错误信息，如果它不为<see langword="null"/>，
    /// 则在验证不通过时，返回这个错误信息，且具有最高优先级
    /// </summary>
    public string? Message { get; init; }

    /*注意：
      除非必要，否则不建议使用本属性，
      因为本属性不能根据不同的情况生成不同的错误文本*/
    #endregion
    #region 执行验证
    /// <summary>
    /// 执行验证，并返回验证结果
    /// </summary>
    /// <param name="value">要验证的属性的值</param>
    /// <param name="describe">对验证对象的描述，它影响在验证不通过时，生成的错误文本</param>
    /// <param name="recursion">这个验证委托可以支持递归验证</param>
    /// <returns>验证不通过的原因，如果为<see langword="null"/>，表示验证通过</returns>
    public abstract string? Verify(object? value, string describe, DataVerify recursion);
    #endregion
}
