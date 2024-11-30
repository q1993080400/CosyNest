using System.Collections.Immutable;
using System.Performance;

namespace System.DataFrancis;

/// <summary>
/// 这个记录指示某个类型是否含有，以及含有哪些封装可预览文件的属性，
/// 它可以缓存下来，避免大量的反射
/// </summary>
public sealed record HasPreviewFilePropertyNatureState
{
    #region 静态成员
    #region 缓存
    /// <summary>
    /// 获取按类型索引本对象的缓存
    /// </summary>
    private static ICache<Type, HasPreviewFilePropertyNatureState> Cache { get; set; }
        = CreatePerformance.MemoryCache<Type, HasPreviewFilePropertyNatureState>(static type =>
        {
            var previewFilePropertyDescribe = type.GetPropertyInfoAlmighty(true).
            Select(DataFrancis.PreviewFilePropertyDescribe.Get).WhereNotNull().
            ToImmutableDictionary(x => x.Property.Name, x => x);
            return new()
            {
                HasReadOnlyPreviewFile = previewFilePropertyDescribe.Count > 0,
                HasPreviewFile = previewFilePropertyDescribe.Values.Any(x => x.IsStrict),
                PreviewFilePropertyDescribe = previewFilePropertyDescribe,
                Type = type
            };
        });
    #endregion
    #region 根据类型获取本对象
    /// <summary>
    /// 返回一个对象，
    /// 它指示某类型是否含有，以及含有哪些封装上传文件的属性
    /// </summary>
    /// <param name="type">要判断的类型</param>
    /// <returns></returns>
    public static HasPreviewFilePropertyNatureState Get(Type type)
        => Cache[type];
    #endregion
    #endregion
    #region 公开成员
    #region 指定的类型
    /// <summary>
    /// 获取被描述的类型
    /// </summary>
    public required Type Type { get; init; }
    #endregion
    #region 是否包含只读可预览文件
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示这个对象封装了<see cref="IHasReadOnlyPreviewFile"/>，或者它的集合
    /// </summary>
    public required bool HasReadOnlyPreviewFile { get; init; }
    #endregion
    #region 是否包含可预览文件
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示这个对象封装了<see cref="IHasPreviewFile"/>，或者它的集合
    /// </summary>
    public required bool HasPreviewFile { get; init; }
    #endregion
    #region 索引对封装可预览文件的属性的描述
    /// <summary>
    /// 按照属性的名称，
    /// 索引对封装可预览文件的属性的描述
    /// </summary>
    public required IReadOnlyDictionary<string, PreviewFilePropertyDescribe> PreviewFilePropertyDescribe { get; init; }
    #endregion
    #endregion 
}