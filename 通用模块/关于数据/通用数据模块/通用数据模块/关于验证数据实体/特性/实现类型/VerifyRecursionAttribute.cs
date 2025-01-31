namespace System.DataFrancis;

/// <summary>
/// 这个特性可以用来进行递归验证
/// </summary>
public sealed class VerifyRecursionAttribute : VerifyAttribute
{
    #region 执行验证
    public override string? Verify(object? value, string describe, DataVerify recursion)
        => value is null ?
        throw new NullReferenceException("不能递归验证为null的实体") :
        recursion(value).FailureReasonMessage();
    #endregion 
}
