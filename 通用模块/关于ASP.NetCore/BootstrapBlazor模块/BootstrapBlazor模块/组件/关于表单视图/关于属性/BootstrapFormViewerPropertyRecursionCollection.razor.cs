using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 这个组件用来在递归渲染中渲染集合
/// </summary>
/// <typeparam name="ElementType">元素的类型</typeparam>
public sealed partial class BootstrapFormViewerPropertyRecursionCollection<ElementType> : ComponentBase
    where ElementType : class
{
    #region 组件参数
    #region 是否只读
    /// <summary>
    /// 指示是否可以修改集合和集合中的元素
    /// </summary>
    [Parameter]
    [EditorRequired]
    public bool IsReadOnly { get; set; }
    #endregion
    #region 要渲染的集合
    /// <summary>
    /// 要渲染的集合
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IEnumerable<ElementType> RenderCollection { get; set; }
    #endregion
    #region 级联参数：用来进行递归渲染的委托
    /// <summary>
    /// 获取用来递归渲染属性的委托
    /// </summary>
    [CascadingParameter]
    private RenderFragment<RenderFormViewerPropertyInfoRecursion>? RenderRecursion { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 获取该集合是否可修改
    /// <summary>
    /// 获取这个集合是否可以修改
    /// </summary>
    /// <returns></returns>
    private bool CanUpdateCollection()
        => RenderCollection is ICollection<ElementType> &&
        typeof(ICreate<ElementType>).IsAssignableFrom(typeof(ElementType));
    #endregion
    #region 添加时触发的事件
    /// <summary>
    /// 添加元素时触发的事件
    /// </summary>
    private void OnAdd()
    {
        var value = typeof(ElementType).GetMethod(nameof(ICreate<>.Create),
            BindingFlags.Static | BindingFlags.Public, [])!.Invoke<ElementType>(null)!;
        RenderCollection.To<ICollection<ElementType>>()!.Add(value);
    }
    #endregion
    #region 删除时触发的事件
    /// <summary>
    /// 删除元素时触发这个事件
    /// </summary>
    /// <param name="element">要删除的元素</param>
    private void OnDelete(ElementType element)
    {
        RenderCollection.To<ICollection<ElementType>>()!.Remove(element);
    }
    #endregion
    #endregion
}
