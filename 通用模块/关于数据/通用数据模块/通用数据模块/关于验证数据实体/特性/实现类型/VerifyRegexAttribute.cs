using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace System.DataFrancis;

/// <summary>
/// 这个特性使用正则表达式对实体进行验证
/// </summary>
/// <param name="regex">用来匹配实体属性的正则表达式</param>
public sealed class VerifyRegexAttribute([StringSyntax(StringSyntaxAttribute.Regex)] string regex) : VerifyAttribute
{
    #region 公开成员
    #region 执行验证
    public override string? Verify(object? value, string describe, DataVerify recursion)
    {
        if (value is not string t)
            return $"{describe}不是不为null的string类型，无法匹配";
        return Regex.IsMatch(t) ?
            null :
            $"{describe}不符合正则表达式的匹配模式";
    }
    #endregion 
    #endregion
    #region 内部成员
    #region 正则表达式
    /// <summary>
    /// 获取用来匹配实体属性的正则表达式
    /// </summary>
    private IRegex Regex { get; } = regex.Op().Regex();
    #endregion
    #endregion
}
