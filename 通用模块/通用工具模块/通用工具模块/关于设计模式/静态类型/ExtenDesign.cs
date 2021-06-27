using System.Collections.Generic;
using System.ComponentModel;
using System.Design;
using System.Design.Direct;
using System.Linq;
using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// 有关设计模式的扩展方法全部放在这里
    /// </summary>
    public static class ExtenDesign
    {
        #region 关于INotifyPropertyChanged
        #region 发出属性已修改的通知
        #region 弱事件版本
        /// <summary>
        /// 调用<see cref="INotifyPropertyChanged.PropertyChanged"/>事件，
        /// 自动填入调用属性的名称
        /// </summary>
        /// <param name="obj">要引发事件的<see cref="INotifyPropertyChanged"/>对象</param>
        /// <param name="delegate"><see cref="INotifyPropertyChanged.PropertyChanged"/>事件所在的弱引用封装</param>
        /// <param name="propertyName">调用属性的名称，可自动获取，如果是<see cref="string.Empty"/>，代表所有属性都已更改</param>
        public static void Changed(this INotifyPropertyChanged obj, WeakDelegate<PropertyChangedEventHandler>? @delegate, [CallerMemberName] string propertyName = "")
            => @delegate?.DynamicInvoke(obj, new PropertyChangedEventArgs(propertyName));

        /*注释：如果将更改属性名设为String.Empty，
           可以通知索引器已经更改，
           但如果填入索引器的默认名称Item，则不会发出通知，
           原因可能是索引器能够重载*/
        #endregion
        #region 传统事件版本
        /// <summary>
        /// 调用一个<see cref="INotifyPropertyChanged.PropertyChanged"/>事件，
        /// 自动填入调用属性的名称
        /// </summary>
        /// <param name="obj">要引发事件的<see cref="INotifyPropertyChanged"/>对象</param>
        /// <param name="delegate"><see cref="INotifyPropertyChanged.PropertyChanged"/>事件的传统委托封装</param>
        /// <param name="propertyName">调用属性的名称，可自动获取，如果是<see cref="string.Empty"/>，代表所有属性都已更改</param>
        public static void Changed(this INotifyPropertyChanged obj, PropertyChangedEventHandler? @delegate, [CallerMemberName] string propertyName = "")
            => @delegate?.Invoke(obj, new PropertyChangedEventArgs(propertyName));
        #endregion
        #endregion
        #endregion
        #region 检查IDirect的架构
        #region 同步迭代器版本
        /// <summary>
        /// 检查一个<see cref="IDirect"/>集合的架构，
        /// 如果出现任何架构不一致的元素，则抛出异常
        /// </summary>
        /// <param name="directs">待检查架构的集合</param>
        /// <returns></returns>
        public static IEnumerable<IDirect> CheckSchema(this IEnumerable<IDirect> directs)
        {
            var (first, other, hasElements) = directs.First(true);
            if (!hasElements)
                yield break;
            var schema = first.Schema ?? CreateDesign.Schema(first);
            yield return first;
            foreach (var item in other)
            {
                schema.SchemaCompatible(item, true);
                yield return item;
            }
        }
        #endregion
        #region 异步迭代器版本

        #endregion
        #endregion
    }
}
