using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.ML
{
    /// <summary>
    /// 关于机器学习的工具类
    /// </summary>
    public static class ToolML
    {
        #region 返回一个公用的MLContext
        /// <summary>
        /// 返回一个公用的<see cref="MLContext"/>对象
        /// </summary>
        public static MLContext Context { get; }
        = new MLContext();
        #endregion
    }
}
