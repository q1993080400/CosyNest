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
    #region 是否为集合
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示该类型是一个集合类型
    /// </summary>
    bool Multiple
        => Type.IsCollection();
    #endregion
    #region 索引对封装可预览文件的属性的描述
    /// <summary>
    /// 按照属性的名称，
    /// 索引对封装可预览文件的属性的描述
    /// </summary>
    IReadOnlyDictionary<string, IHasPreviewFilePropertyInfo> HasPreviewFilePropertyInfo { get; }
    #endregion
    #region 是否直接映射
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示它直接映射到<see cref="IHasReadOnlyPreviewFile"/>或它的集合，
    /// 否则表示它是通过<see cref="MapToPreviewFileAttribute"/>特性间接映射的
    /// </summary>
    bool IsDirect { get; }
    #endregion
    #region 已知派生类型
    /// <summary>
    /// 按照派生类型索引这个类型的已知派生类型的可预览文件信息，
    /// 有可能这个类型本身没有封装可预览文件，
    /// 但是它的派生类型封装了
    /// </summary>
    IReadOnlyDictionary<Type, IHasPreviewFileTypeInfo> KnownDerivedTypes { get; }
    #endregion
    #region 集合元素的类型信息
    /// <summary>
    /// 如果这个类型是一个集合，
    /// 则返回集合元素的类型可预览文件信息，
    /// 否则为<see langword="null"/>
    /// </summary>
    IHasPreviewFileTypeInfo? ElementPreviewFileTypeInfo { get; }
    #endregion
}
