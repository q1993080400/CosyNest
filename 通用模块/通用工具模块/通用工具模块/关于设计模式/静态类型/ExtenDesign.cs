using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System;

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
        => @delegate?.Invoke(obj, new PropertyChangedEventArgs(propertyName));

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
}
