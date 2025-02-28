using System.Collections.Immutable;

namespace System.DataFrancis;

/// <summary>
/// 这个类型可以延迟获取<see cref="IHasPreviewFileTypeInfo"/>，
/// 它主要用来解决无限循环递归的问题
/// </summary>
sealed class HasPreviewFileTypeLazyInfo : IHasPreviewFileTypeInfo
{
    #region 公开成员
    #region 封装可预览文件的类型
    public required Type Type { get; init; }
    #endregion
    #region 集合元素的类型信息
    public IHasPreviewFileTypeInfo? ElementPreviewFileTypeInfo
        => TypeInfo?.ElementPreviewFileTypeInfo;
    #endregion
    #region 索引对可预览文件属性的描述
    public IReadOnlyDictionary<string, IHasPreviewFilePropertyInfo> HasPreviewFilePropertyInfo
        => TypeInfo?.HasPreviewFilePropertyInfo ?? ImmutableDictionary<string, IHasPreviewFilePropertyInfo>.Empty;
    #endregion
    #region 递归获取所有可预览文件
    public IEnumerable<PreviewFileInfo> AllPreviewFile(object? obj, bool isStrict)
        => TypeInfo?.AllPreviewFile(obj, isStrict) ?? [];
    #endregion
    #region 是否为严格模式
    public bool IsStrict
        => TypeInfo?.IsStrict ?? false;
    #endregion
    #region 是否直接映射
    public bool IsDirect
        => TypeInfo?.IsDirect ?? false;
    #endregion
    #region 封装可预览文件的状态
    public HasPreviewFileState HasPreviewFileState
        => TypeInfo?.HasPreviewFileState ?? HasPreviewFileState.None;
    #endregion
    #region 已知派生类型
    public IReadOnlyDictionary<Type, IHasPreviewFileTypeInfo> KnownDerivedTypes
        => TypeInfo?.KnownDerivedTypes ?? ImmutableDictionary<Type, IHasPreviewFileTypeInfo>.Empty;
    #endregion
    #endregion
    #region 内部成员
    #region 延迟获取可预览文件信息
    /// <summary>
    /// 延迟获取可预览文件信息，
    /// 如果尚未保存，则为<see langword="null"/>
    /// </summary>
    /// <returns></returns>
    private IHasPreviewFileTypeInfo? TypeInfo
    {
        get
        {
            if (field is { })
                return field;
            return CreateDataObj.Cache.TryGetValue(Type, out var info) ? field = info : null;
        }
    }
    #endregion
    #endregion
}
