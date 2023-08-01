using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace System.DataFrancis.EntityDescribe;

/// <summary>
/// 这个特性使用正则表达式对实体进行验证
/// </summary>
public sealed class VerifyRegexAttribute : VerifyAttribute
{
    #region 公开成员
    #region 执行验证
    public override string? Verify(object? obj, string describe)
    {
        if (obj is not string t)
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
    private IRegex Regex { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的正则表达式初始化对象
    /// </summary>
    /// <param name="regex">用来匹配实体属性的正则表达式</param>
    public VerifyRegexAttribute([StringSyntax(StringSyntaxAttribute.Regex)] string regex)
    {
        Regex = regex.Op().Regex();
    }
    #endregion
}
