using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace System;

public static partial class ExtendText
{
    //这个静态类专门声明和转化字符串有关的API

    #region 关于字符串的转化
    #region 返回字符串操作对象
    /// <summary>
    /// 将字符串封装为一个<see cref="StringOperate"/>，
    /// 通过它可以访问到一些和字符串有关的，高度特化的扩展方法
    /// </summary>
    /// <param name="text">被封装的字符串</param>
    /// <returns></returns>
    public static StringOperate Op(this string text)
        => new()
        {
            Text = text
        };
    #endregion
    #region 转化为正则表达式
    /// <summary>
    /// 将一个字符串转化为正则表达式
    /// </summary>
    /// <param name="op">封装指定的字符串的对象，
    /// 它将作为<see cref="Text.RegularExpressions.Regex"/>的正则表达式（但不是被匹配的文本）</param>
    /// <param name="options">正则表达式的选项</param>
    /// <returns></returns>
    public static IRegex Regex(this StringOperate op, RegexOptions options = RegexOptions.None)
        => new RegexRealize(op.Text, options);
    #endregion
    #region 从字符串转换为字节数组
    /// <summary>
    /// 将一个字符串转换为它的二进制形式，并返回
    /// </summary>
    /// <param name="text">待转换的字符串</param>
    /// <param name="encoding">字符串使用的编码，
    /// 如果为<see langword="null"/>，默认为UTF-8 </param>
    /// <returns></returns>
    public static byte[] ToBytes(this string text, Encoding? encoding = null)
        => (encoding ?? Encoding.UTF8).GetBytes(text);
    #endregion
    #region 从字节数组转换为字符串
    /// <summary>
    /// 将一个字节数组转换为字符串
    /// </summary>
    /// <param name="bytes">待转换的字符串</param>
    /// <param name="encoding">字符串使用的编码，
    /// 如果为<see langword="null"/>，默认为UTF-8</param>
    /// <returns></returns>
    public static string ToText(this byte[] bytes, Encoding? encoding = null)
        => (encoding ?? Encoding.UTF8).GetString(bytes);
    #endregion
    #region 删除指定字符
    #region 直接删除
    /// <summary>
    /// 删除字符串中出现的指定字符
    /// </summary>
    /// <param name="text">待删除字符的字符串</param>
    /// <param name="remove">需要删除的字符</param>
    /// <returns></returns>
    public static string Remove(this string text, params string[] remove)
        => remove.Length <= 4 ?
            remove.Aggregate(text, (seed, text) => seed.Replace(text, "")) :
            $"({remove.Join("|")})".Op().Regex().Remove(text);

    //当要替换的字符比较多时，使用正则表达式，提高性能
    #endregion
    #region Trim字符串
    /// <summary>
    /// 从字符串的开头或末尾删除指定的字符
    /// </summary>
    /// <param name="text">待执行删除操作的字符串</param>
    /// <param name="trimStart">如果这个值为<see langword="true"/>，
    /// 则从开头删除，否则从末尾删除</param>
    /// <param name="trims">需要删除的字符串，函数会递归进行删除，
    /// 直到<paramref name="text"/>的开头或末尾不存在<paramref name="trims"/>的任何一个元素为止</param>
    /// <returns></returns>
    public static string Trim(this string text, bool trimStart, params string[] trims)
    {
        using var enumerator = trims.To<IEnumerable<string>>().GetEnumerator();
        var newText = text;
        while (enumerator.MoveNext())
        {
            var current = enumerator.Current;
            if (current.IsVoid())
                continue;
            if (trimStart ? newText.StartsWith(current) : newText.EndsWith(current))
            {
                var len = current.Length;
                newText = newText[trimStart ? len.. : ..^len];
                enumerator.Reset();
            }
        }
        return newText;
    }
    #endregion
    #endregion
    #endregion
    #region 关于连接
    #region 连接Char集合
    /// <summary>
    /// 将一个嵌套的<see cref="char"/>集合连接为字符串数组
    /// </summary>
    /// <param name="chars">待连接的集合</param>
    /// <returns></returns>
    public static string[] JoinChar(this IEnumerable<IEnumerable<char>> chars)
        => chars.Select(x => x.Join()).ToArray();
    #endregion
    #region 用指定的字符连接一个集合
    /// <summary>
    /// 调用<see cref="object.ToString"/>方法，将指定集合的所有元素转换为文本，
    /// 再用指定的字符将这些文本连接起来，并返回
    /// </summary>
    /// <typeparam name="Obj">集合的元素类型</typeparam>
    /// <param name="collections">要转换的集合</param>
    /// <param name="join">用来作为连接的字符</param>
    /// <returns></returns>
    public static string Join<Obj>(this IEnumerable<Obj> collections, string? join = null)
        => string.Join(join, collections);
    #endregion
    #region 用委托转换成字符，然后连接
    /// <summary>
    /// 用一个委托将集合中的元素转换为字符串，
    /// 然后再用指定的字符将其连接起来
    /// </summary>
    /// <typeparam name="Obj">集合元素的类型</typeparam>
    /// <param name="collections">要连接的集合</param>
    /// <param name="delegate">将集合元素转换为<see cref="string"/>的委托</param>
    /// <param name="join">用来连接元素的字符</param>
    /// <returns></returns>
    public static string Join<Obj>(this IEnumerable<Obj> collections, Func<Obj, string> @delegate, string? join = null)
        => string.Join(join, collections.Select(@delegate));
    #endregion
    #region 用换行符将文本连接起来
    /// <summary>
    /// 在一个文本后面追加若干文本，
    /// 它们之间全部用换行符分隔
    /// </summary>
    /// <param name="text">原始文本</param>
    /// <param name="newText">新追加的文本</param>
    /// <returns></returns>
    public static string AddLine(this string text, params string[] newText)
    {
        var text2 = new StringBuilder(text);
        foreach (var t in newText)
        {
            text2.Append(Environment.NewLine);
            text2.Append(t);
        }
        return text2.ToString();
    }
    #endregion
    #endregion
    #region 按照文本数枚举字符串
    /// <summary>
    /// 按照文本数量，而不是字符数量枚举字符串，
    /// 需要本方法是因为，有些文本可能占用两个字符
    /// </summary>
    /// <param name="text">待枚举的字符串</param>
    /// <returns></returns>
    public static IEnumerable<string> EnumeratorText(this string text)
    {
        var enumerator = StringInfo.GetTextElementEnumerator(text);
        while (enumerator.MoveNext())
        {
            yield return (string)enumerator.Current;
        }
    }

    /*吐槽：BCL对这个问题的设计非常不合理，
      它应该是一个string的实例方法*/
    #endregion
}
