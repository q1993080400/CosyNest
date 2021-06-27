using System.Collections.Generic;
using System.DrawingFrancis.Text;
using System.Reflection;
using System.Linq;

namespace System.Office
{
    /// <summary>
    /// 这个静态类储存了一些常用的Office文本格式
    /// </summary>
    public static class OfficeTextStyleCom
    {
        #region 缓存ITextStyleVar的属性
        /// <summary>
        /// 这个属性缓存<see cref="ITextStyleVar"/>中所有可读，可写，且公开的属性，
        /// 它可以为实现某些API提供便利
        /// </summary>
        public static IEnumerable<PropertyInfo> CacheStylePro { get; }
        = typeof(ITextStyleVar).GetProperties().Where
                    (par => par.GetPermissions() == null
                    && !par.IsStatic()).ToArray();
        #endregion
        #region 当文本具有多种样式时的样式
        /// <summary>
        /// 这个对象表示文本的各个部分具有不同的格式
        /// </summary>
        public static ITextStyleVar Multiple { get; }
        = new TextStyleMultiple();
        #endregion
    }
}
