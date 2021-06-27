using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    /// <summary>
    /// 所有关于文本的扩展方法全部放在这个类中，通常无需专门调用
    /// </summary>
    public static class ExtenText
    {
        #region 关于字符串的判断
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
            => character.Any(x => text.Contains(x));
        #endregion
        #endregion
        #region 关于字符串的转化
        #region 转化为正则表达式
        /// <summary>
        /// 将一个字符串转化为正则表达式
        /// </summary>
        /// <param name="text">指定的字符串，
        /// 它将作为<see cref="Regex"/>的正则表达式（但不是被匹配的文本）</param>
        /// <param name="options">正则表达式的选项</param>
        /// <returns></returns>
        public static IRegex ToRegex(this string text, RegexOptions options = RegexOptions.None)
            => new RegexRealize(text, options);
        #endregion
        #region 转换为字节数组
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
                $"({remove.Join("|")})".ToRegex().Remove(text);

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
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                if (trimStart ? text.StartsWith(current) : text.EndsWith(current))
                {
                    var len = current.Length;
                    text = text[trimStart ? len.. : ..^len];
                    enumerator.Reset();
                }
            }
            return text;
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
        /// <param name="join">用来作为连接的字符，默认为<see langword="null"/></param>
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
}
