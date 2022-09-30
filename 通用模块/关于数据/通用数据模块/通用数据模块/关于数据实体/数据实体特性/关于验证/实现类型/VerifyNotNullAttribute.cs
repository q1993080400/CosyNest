namespace System.DataFrancis.Verify;

/// <summary>
/// 这个特性表示非空验证，
/// 如果属性值为<see langword="null"/>或空字符串，则无法通过验证
/// </summary>
public sealed class VerifyNotNullAttribute : VerifyAttribute
{
    #region 执行验证
    public override (bool IsSuccess, string Message) Verify(object? obj, string? describe = null)
    {
        describe ??= "数据";
        return obj switch
        {
            null => (false, $"{(describe is "null" ? "数据" : describe)}不能为null"),
            string s when s.IsVoid() => (false, $"{describe}不能为空字符串"),
            _ => (true, "")
        };
    }
    #endregion 
}
