using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.ML
{
    /// <summary>
    /// 这个类型存储了一些机器学习的常用名词，
    /// 在输入它们的时候可以使用智能提示，避免拼写错误
    /// </summary>
    public static class MLVocabulary
    {
        #region 词汇：特性
        /// <summary>
        /// 获取表示特性的词汇
        /// </summary>
        public static string VocFeatures { get; } = "Features";
        #endregion
        #region 词汇：分数
        /// <summary>
        /// 获取表示分数的词汇，
        /// 大多数机器学习任务的都包含有这个输出列
        /// </summary>
        public static string VocScore { get; } = "Score";
        #endregion
    }
}
