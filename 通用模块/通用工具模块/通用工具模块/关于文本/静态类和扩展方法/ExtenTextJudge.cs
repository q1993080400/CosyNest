using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace System;

/// <summary>
/// 所有关于文本的扩展方法全部放在这个类中，通常无需专门调用
/// </summary>
public static partial class ExtenText
{
    //这个静态类专门声明和判断字符串有关的API

    #region 执行不区分大小写的比较
    /// <summary>
    /// 对两个字符串执行忽略大小写的比较，
    /// 并返回它们是否相等
    /// </summary>
    /// <param name="textA">要比较的第一个字符串</param>
    /// <param name="textB">要比较的第二个字符串</param>
    /// <returns></returns>
    public static bool EqualsIgnore(this string? textA, string? textB)
        => string.Equals(textA, textB, StringComparison.OrdinalIgnoreCase);
    #endregion
    #region 判断字符串是否无效
    /// <summary>
    /// 指示字符串是否无效，即为<see langword="null"/>，或为<see cref="string.Empty"/>，或完全由空白字符构成
    /// </summary>
    /// <param name="text">要检查的字符串</param>
    /// <returns></returns>
    public static bool IsVoid([NotNullWhen(false)] this string? text)
        => string.IsNullOrWhiteSpace(text);
    #endregion
    #region 判断是否包含某些字符
    /// <summary>
    /// 判断一个字符串中是否包含某些字符
    /// </summary>
    /// <param name="text">待判断的字符串</param>
    /// <param name="character">如果这些字符中的任意一个在字符串中出现过，则返回<see langword="true"/></param>
    /// <returns></returns>
    public static bool Contains(this string text, params string[] character)
        => character.Any(text.Contains);
    #endregion
    #region 判断是否与开头结尾匹配
    /// <summary>
    /// 检查字符串的开头和结尾是否与某一字符串匹配
    /// </summary>
    /// <param name="text">要检查的字符串</param>
    /// <param name="withStart">匹配开头的字符串</param>
    /// <param name="withEnd">匹配结尾的字符串，
    /// 如果为<see langword="null"/>，则与<paramref name="withStart"/>相同</param>
    /// <returns></returns>
    public static bool With(this string? text, string withStart, string? withEnd = null)
        => text is { } && text.StartsWith(withStart) && text.EndsWith(withEnd ?? withStart);
    #endregion
    #region 关于重复字符
    #region 判断是否为重复字符
    /// <summary>
    /// 如果字符A完全由字符B的重复组成，则返回<see langword="true"/>，否则返回<see langword="false"/>
    /// </summary>
    /// <param name="textA">字符A</param>
    /// <param name="textB">字符B</param>
    /// <returns></returns>
    public static bool IsRepeat(this string textA, string textB)
    {
        if (textA == textB)
            return true;
        var al = textA.Length;
        var bl = textB.Length;
        if (al < bl || al % bl != 0)
            return false;
        for (int beg = 0, end = bl; beg < al; beg += bl, end += bl)
        {
            if (textA[beg..end] != textB)
                return false;
        }
        return true;
    }
    #endregion
    #region 重复字符，并返回
    /// <summary>
    /// 将一个字符串重复若干次，并返回重复后的新字符串
    /// </summary>
    /// <param name="text">原始文本</param>
    /// <param name="number">重复的次数，
    /// 如果它小于等于0，返回<see cref="string.Empty"/>，为1，返回原始文本，
    /// 大于1，返回重复后的文本</param>
    /// <returns></returns>
    public static string Repeat(this string text, int number)
    {
        var text2 = new StringBuilder("");
        for (int i = 0; i < number; i++)
            text2.Append(text);
        return text2.ToString();
    }
    #endregion
    #endregion
}
