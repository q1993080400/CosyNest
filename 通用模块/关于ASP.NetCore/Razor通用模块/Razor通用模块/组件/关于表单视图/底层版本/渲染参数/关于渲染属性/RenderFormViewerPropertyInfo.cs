namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是用来渲染<see cref="FormViewer{Model}"/>的中每个属性的参数
/// </summary>
/// <inheritdoc cref="RenderFormViewerPropertyInfoBase{Model}"/>
public sealed record RenderFormViewerPropertyInfo<Model> : RenderFormViewerPropertyInfoBase<Model>, ITitleData
    where Model : class
{
    #region 属性名称
    public required string Name { get; init; }
    #endregion
    #region 带点号的属性名称
    /// <summary>
    /// 获取带点号的属性名称，
    /// 在某些情况下，它是建议的属性名称
    /// </summary>
    public string NameWithPoint
        => (Name.IsVoid() || Name.EndsWith(':')) ? Name : Name + ":";
    #endregion
    #region 是否显示名称
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示应该显示名称
    /// </summary>
    public bool ShowName
        => !Name.IsVoid();
    #endregion
    #region 获取属性的值的类型
    public Type ValueType
         => Property.PropertyType;
    #endregion
    #region 是否递归渲染
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则指示这个属性的类型是一个复杂的对象，
    /// 应该进行递归渲染，它把这个属性视为一个新的表单
    /// </summary>
    public required bool IsRecursion { get; init; }
    #endregion
    #region 说明
    /// <summary>
    /// 获取对这个字段的说明，
    /// 它被放在这个字段的下方
    /// </summary>
    public required string? Describe { get; init; }
    #endregion
    #region 转换为渲染递归属性的参数
    /// <summary>
    /// 将本记录转换为渲染递归属性的参数，
    /// 如果不是递归渲染，则为<see langword="null"/>
    /// </summary>
    /// <returns></returns>
    public RenderFormViewerPropertyInfoRecursion? ToRecursion()
        => IsRecursion ? new()
        {
            IsReadOnly = IsReadOnly,
            Value = Value,
            PropertyType = Property.PropertyType
        } : null;
    #endregion
    #region 值为null的时候显示的文本
    /// <summary>
    /// 获取值为<see langword="null"/>的时候显示的文本，
    /// 只在只读状态下生效
    /// </summary>
    public required string? ValueIfNullText { get; init; }
    #endregion
    #region 将值格式化为文本
    /// <summary>
    /// 将属性的值格式化为文本，
    /// 它会被作为最终显示的文本
    /// </summary>
    /// <returns></returns>
    public string? FormatValue()
        => (Value, RenderPreference) switch
        {
            (null, _) => ValueIfNullText,
            (var value, { } renderPreference) => renderPreference.RenderToText(value),
            (true, _) => "是",
            (false, _) => "否",
            (Enum @enum, _) => @enum.GetDescription(),
            (var value, _) => value.ToString()
        };
    #endregion
}
