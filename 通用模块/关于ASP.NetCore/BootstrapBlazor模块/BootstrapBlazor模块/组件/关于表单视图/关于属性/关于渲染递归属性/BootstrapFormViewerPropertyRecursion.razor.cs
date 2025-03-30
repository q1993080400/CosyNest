using System.Reflection.Metadata;

using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 这个组件可以用来递归渲染<see cref="BootstrapFormViewer{Model}"/>中的属性
/// </summary>
/// <typeparam name="PropertyType">要进行递归渲染的属性的类型，
/// 注意：它指的不是模型的类型</typeparam>
public sealed partial class BootstrapFormViewerPropertyRecursion<PropertyType> : ComponentBase
    where PropertyType : class
{
    #region 组件参数
    #region 渲染参数
    /// <summary>
    /// 获取渲染这个属性的参数
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFormViewerPropertyInfoRecursion RenderPropertyInfo { get; set; }
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
    #region 添加时触发的事件
    #region 事件本体
    /// <summary>
    /// 添加元素时触发的事件的本体
    /// </summary>
    private void OnAddEvent()
    {
        var method = this.GetType().GetMethod(nameof(OnAdd), BindingFlags.Static | BindingFlags.NonPublic)!;
        var elementType = typeof(PropertyType).GetCollectionElementType()!;
        method.MakeGenericMethod(elementType).Invoke(null, [RenderPropertyInfo.Value]);
    }
    #endregion
    #region 事件实现
    /// <summary>
    /// 添加元素时触发的事件
    /// </summary>
    /// <typeparam name="ElementType">元素的类型</typeparam>
    /// <param name="elementTypes">元素的集合</param>
    private static void OnAdd<ElementType>(ICollection<ElementType> elementTypes)
        where ElementType : class, ICreate<ElementType>
    {
        elementTypes.Add(ElementType.Create());
    }
    #endregion 
    #endregion
    #endregion
}

/// <summary>
/// 这个静态辅助类可以用来动态渲染递归属性
/// </summary>
public static class BootstrapFormViewerPropertyRecursion
{
    #region 获取渲染递归属性的委托
    /// <summary>
    /// 这个委托可以用来动态渲染递归属性
    /// </summary>
    /// <param name="recursionInfo">用来渲染递归属性的参数</param>
    /// <returns></returns>
    public static RenderFragment DynamicRender(RenderFormViewerPropertyInfoRecursion recursionInfo)
        => builder =>
        {
            var propertyType = recursionInfo.PropertyType;
            var componentType = typeof(BootstrapFormViewerPropertyRecursion<>).MakeGenericType(propertyType);
            builder.OpenComponent(0, componentType);
            builder.AddComponentParameter(1, nameof(BootstrapFormViewerPropertyRecursion<>.RenderPropertyInfo), recursionInfo);
            builder.CloseComponent();
        };
    #endregion
}
