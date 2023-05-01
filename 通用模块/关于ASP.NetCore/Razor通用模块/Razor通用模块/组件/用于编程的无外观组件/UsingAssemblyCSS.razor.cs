namespace Microsoft.AspNetCore.Components.Web;

/// <summary>
/// 该组件可以在Head标签中自动引用本程序集中嵌入的CSS
/// </summary>
public sealed partial class UsingAssemblyCSS : ComponentBase
{
    #region 注意事项
    /*#本组件的性能不好，可能因为加载速度而出现残影现象，
      如果介意这个问题，可以考虑将css引用硬编码到HTML页面中*/
    #endregion
}
