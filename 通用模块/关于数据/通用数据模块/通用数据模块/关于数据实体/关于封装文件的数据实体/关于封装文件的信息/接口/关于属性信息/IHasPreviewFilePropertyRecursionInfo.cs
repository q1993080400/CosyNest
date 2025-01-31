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
}
