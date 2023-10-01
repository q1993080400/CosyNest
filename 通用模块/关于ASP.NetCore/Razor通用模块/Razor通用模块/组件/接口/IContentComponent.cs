namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个拥有子内容的组件
/// </summary>
/// <typeparam name="Content">组件子内容的类型</typeparam>
public interface IContentComponent<Content> : IComponent
{
    #region 子内容
    /// <summary>
    /// 获取或设置组件的子内容
    /// </summary>
    Content ChildContent { get; set; }
    #endregion
}
