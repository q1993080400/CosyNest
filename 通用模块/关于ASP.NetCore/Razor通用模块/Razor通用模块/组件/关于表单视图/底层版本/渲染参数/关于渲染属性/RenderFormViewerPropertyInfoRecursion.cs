namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是渲染<see cref="FormViewer{Model}"/>中的递归属性的参数
/// </summary>
public sealed record RenderFormViewerPropertyInfoRecursion
{
    #region 属性的值
    /// <summary>
    /// 获取属性的值
    /// </summary>
    public required object? Value { get; init; }
    #endregion
    #region 获取属性的值
    /// <summary>
    /// 将属性转换为指定的值，
    /// 然后返回
    /// </summary>
    /// <typeparam name="Obj">属性的值的类型</typeparam>
    /// <returns></returns>
    public Obj? GetValue<Obj>()
        => Value.To<Obj>();
    #endregion
    #region 获取属性的类型
    /// <summary>
    /// 获取属性的类型
    /// </summary>
    public required Type PropertyType { get; init; }
    #endregion
    #region 是否只读
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示仅提供数据显示功能，不提供数据编辑功能
    /// </summary>
    public required bool IsReadOnly { get; init; }
    #endregion
    #region 是否为集合类型
    /// <summary>
    /// 获取这个属性类型是否为集合类型
    /// </summary>
    public bool IsEnumerableType
        => PropertyType.GetCollectionElementType() is { };
    #endregion
    #region 是否为可修改集合
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示<see cref="PropertyType"/>是一个可修改的集合，
    /// 它同时实现了<see cref="ICollection{T}"/>和<see cref="ICreate{Obj}"/>
    /// </summary>
    public bool CanUpdateCollection
    {
        get
        {
            var (isRealize, _, genericParameters) = PropertyType.IsRealizeGeneric(typeof(ICollection<>));
            if (!isRealize)
                return false;
            var genericParameter = genericParameters[0];
            return genericParameter.IsGenericRealize(typeof(ICreate<>));
        }
    }
    #endregion
}
