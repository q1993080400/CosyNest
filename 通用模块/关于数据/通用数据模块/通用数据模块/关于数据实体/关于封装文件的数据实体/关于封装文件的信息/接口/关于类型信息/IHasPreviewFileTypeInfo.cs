namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个针对类型的，
/// 封装可预览文件的信息
/// </summary>
public interface IHasPreviewFileTypeInfo : IHasPreviewFileInfo
{
    #region 指定的类型
    /// <summary>
    /// 获取被描述的类型
    /// </summary>
    Type Type { get; }
    #endregion
    #region 索引对封装可预览文件的属性的描述
    /// <summary>
    /// 按照属性的名称，
    /// 索引对封装可预览文件的属性的描述
    /// </summary>
    IReadOnlyDictionary<string, IHasPreviewFilePropertyInfo> HasPreviewFilePropertyInfo { get; }
    #endregion
}
