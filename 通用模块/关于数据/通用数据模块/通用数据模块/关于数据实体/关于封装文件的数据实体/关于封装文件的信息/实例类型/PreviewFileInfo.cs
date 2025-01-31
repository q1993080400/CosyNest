using System.Reflection;

namespace System.DataFrancis;

/// <summary>
/// 这个记录描述了一条可预览文件依附于哪个对象，
/// 以及它的其他信息
/// </summary>
public sealed record PreviewFileInfo
{
    #region 可预览文件
    /// <summary>
    /// 获取封装的可预览文件
    /// </summary>
    public required IReadOnlyList<IHasReadOnlyPreviewFile> Files { get; init; }
    #endregion
    #region 可预览文件所依附的属性
    /// <summary>
    /// 获取这个可预览文件所依附的属性
    /// </summary>
    public required PropertyInfo Property { get; init; }
    #endregion
    #region 属性所依附的对象
    /// <summary>
    /// 获取这个属性所依附的对象
    /// </summary>
    public required object Target { get; init; }
    #endregion
    #region 是否为可预览文件的集合
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示该属性为可预览文件的集合，
    /// 否则表示只能封装单个可预览文件
    /// </summary>
    public required bool Multiple { get; init; }
    #endregion
    #region 是否为严格模式
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示该对象为严格模式，它表示必然直接或间接封装了<see cref="IHasPreviewFile"/>或它的集合，
    /// 否则只表示封装了<see cref="IHasReadOnlyPreviewFile"/>或它的集合，
    /// 或者根本没有封装可预览属性
    /// </summary>
    public required bool IsStrict { get; init; }
    #endregion
    #region 写入可预览文件
    /// <summary>
    /// 向属性中写入新的可预览文件
    /// </summary>
    /// <param name="files">要写入的新可预览文件</param>
    public void SetFile(IReadOnlyList<IHasReadOnlyPreviewFile> files)
    {
        if (Multiple)
        {
            var list = Property.PropertyType.CreateCollection(files);
            Property.SetValue(Target, list);
            return;
        }
        if (files.Count > 1)
            throw new NotSupportedException($"{nameof(Property.DeclaringType)}.{Property.Name}不是文件的集合，但是传入了多个文件");
        var file = files.SingleOrDefault();
        Property.SetValue(Target, file);
    }
    #endregion
}
