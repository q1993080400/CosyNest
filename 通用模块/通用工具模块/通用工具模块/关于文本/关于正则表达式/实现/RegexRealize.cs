using System.Linq;

namespace System.Text.RegularExpressions
{
    /// <summary>
    /// 这个类型是<see cref="IRegex"/>的实现，
    /// 可以视为一个正则表达式
    /// </summary>
    class RegexRealize : IRegex
    {
        #region 匹配所需的信息
        #region 用于匹配的正则表达式
        public string RegexText { get; }
        #endregion
        #region 匹配选项
        /// <summary>
        /// 获取用于匹配的选项
        /// </summary>
        private RegexOptions Options { get; }
        #endregion
        #endregion
        #region 关于匹配
        #region 返回是否匹配到了结果
        public bool IsMatch(string text)
             => Regex.IsMatch(text, RegexText, Options);
        #endregion
        #region 返回匹配结果
        public (bool IsMatch, IMatch[] Matches) Matches(string text)
        {
            var matcher = Regex.Matches(text, RegexText, Options).Select(x => (IMatch)new RegexMatch(x, RegexText)).ToArray();
            return (matcher.Any(), matcher);
        }
        #endregion
        #region 返回第一个匹配到的结果
        public IMatch? MatcheFirst(string text)
        {
            var match = Regex.Match(text, RegexText, Options);
            return match.Success ? new RegexMatch(match, RegexText) : null;
        }
        #endregion
        #region 替换匹配到的字符
        public string Replace(string text, string replace)
            => Regex.Replace(text, RegexText, replace, Options);
        #endregion
        #endregion
        #region 构造方法
        /// <summary>
        /// 使用指定的正则表达式初始化对象
        /// </summary>
        /// <param name="regexText">指定的正则表达式</param>
        /// <param name="options">正则表达式的选项</param>
        public RegexRealize(string regexText, RegexOptions options)
        {
            this.RegexText = regexText;
            this.Options = options;
        }
        #endregion
        #region 静态构造函数
        static RegexRealize()
        {
            Regex.CacheSize = 30;
        }
        #endregion
    }
}
