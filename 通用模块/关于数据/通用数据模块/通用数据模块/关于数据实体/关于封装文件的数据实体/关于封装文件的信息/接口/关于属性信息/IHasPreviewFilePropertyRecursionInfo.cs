namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个针对属性的，
/// 封装可预览文件的信息，
/// 它指示该属性间接封装了可预览文件
/// </summary>
public interface IHasPreviewFilePropertyRecursionInfo : IHasPreviewFilePropertyInfo
{
    #region 对属性类型的描述
    /// <summary>
    /// 获取对属性类型的描述，
    /// 它可以进一步说明哪个属性封装了可预览文件
    /// </summary>
    IHasPreviewFileTypeInfo PropertyTypeInfo { get; }
    #endregion
    #region 是否直接所有
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示属性类型的子属性直接拥有可预览文件，
    /// 否则表示属性的类型的子属性本身不拥有，
    /// 但是它的已知派生类型的子属性拥有可预览文件
    /// </summary>
    bool IsDirectOwne { get; }
    #endregion
}
