using System.Reflection;

namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个针对属性的，
/// 封装可预览文件的信息
/// </summary>
public interface IHasPreviewFilePropertyInfo : IHasPreviewFileInfo
{
    #region 封装可预览文件的属性
    /// <summary>
    /// 封装了可预览文件的属性
    /// </summary>
    PropertyInfo Property { get; }
    #endregion
    #region 是否可写入
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示这个属性是Init属性，
    /// 只能在构造时写入
    /// </summary>
    bool IsInitOnly { get; }
    #endregion
}
