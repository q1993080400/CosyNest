using System.Reflection;

namespace System.DataFrancis;

/// <summary>
/// 这个记录是对于某个封装可预览文件属性的描述
/// </summary>
public sealed record PreviewFilePropertyDescribe
{
    #region 静态成员
    #region 获取属性描述
    /// <summary>
    /// 获取一个属性有关封装可预览文件信息的描述，
    /// 如果它没有封装可预览文件信息，
    /// 则返回<see langword="null"/>
    /// </summary>
    /// <param name="propertyInfo">要检查的属性</param>
    /// <returns></returns>
    public static PreviewFilePropertyDescribe? Get(PropertyInfo propertyInfo)
    {
        var propertyType = propertyInfo.PropertyType;
        var isInitOnly = propertyInfo.IsInitOnly();
        return propertyType switch
        {
            var type when typeof(IEnumerable<IHasReadOnlyPreviewFile>).IsAssignableFrom(type)
            => new()
            {
                IsInitOnly = isInitOnly,
                Multiple = true,
                Property = propertyInfo,
                IsStrict = typeof(IEnumerable<IHasPreviewFile>).IsAssignableFrom(type)
            },
            var type when typeof(IHasReadOnlyPreviewFile).IsAssignableFrom(type)
            => new()
            {
                IsInitOnly = isInitOnly,
                Multiple = false,
                Property = propertyInfo,
                IsStrict = typeof(IHasPreviewFile).IsAssignableFrom(type)
            },
            _ => null
        };
    }
    #endregion
    #endregion
    #region 公开成员
    #region 封装可预览文件的属性
    /// <summary>
    /// 封装了可预览文件的属性
    /// </summary>
    public required PropertyInfo Property { get; init; }
    #endregion
    #region 是否为可预览文件的集合
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示该属性为可预览文件的集合，
    /// 否则表示只能封装单个可预览文件
    /// </summary>
    public required bool Multiple { get; init; }
    #endregion
    #region 是否可写入
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示这个属性是Init属性，
    /// 只能在构造时写入
    /// </summary>
    public required bool IsInitOnly { get; init; }
    #endregion
    #region 是否为严格模式
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则表示该属性为严格模式，它表示属性必然是<see cref="IHasPreviewFile"/>或它的集合，
    /// 否则只表示属性是<see cref="IHasReadOnlyPreviewFile"/>或它的集合
    /// </summary>
    public required bool IsStrict { get; init; }
    #endregion
    #region 获取可预览文件属性的值
    /// <summary>
    /// 获取可预览文件属性的值，
    /// 注意，无论<see cref="Multiple"/>的值是什么，它都返回一个集合，
    /// 如果<see cref="Multiple"/>为<see langword="false"/>，
    /// 则返回只有一个文件的集合，
    /// 为<see langword="null"/>则返回空集合
    /// </summary>
    /// <param name="target">属性依附的对象实例</param>
    /// <returns></returns>
    public IEnumerable<IHasReadOnlyPreviewFile> GetFiles(object target)
    {
        var value = Property.GetValue(target);
        if (Multiple)
            return value.To<IEnumerable<IHasReadOnlyPreviewFile>>() ?? [];
        var file = value.To<IHasReadOnlyPreviewFile>();
        return file is null ? [] : [file];
    }
    #endregion
    #endregion
}
