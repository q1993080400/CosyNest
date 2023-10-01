using System.Localization;

using Microsoft.AspNetCore.Components;

namespace ViewDependencies;

/// <summary>
/// 这个组件可以用来渲染所有模块
/// </summary>
public sealed partial class RenderAllModules : ComponentBase
{
    #region 内部成员
    #region 所有模块
    /// <summary>
    /// 获取所有模块
    /// </summary>
    private static IEnumerable<Module> Modules { get; }
        = FindModule.GetModules(@"C:\CosyNest");
    #endregion
    #region 要显示的模块
    private IEnumerable<Module>? DisplayModuleField;

    /// <summary>
    /// 获取要显示的模块
    /// </summary>
    private IEnumerable<Module> DisplayModule
    {
        get => DisplayModuleField ?? Modules;
        set => DisplayModuleField = value;
    }
    #endregion
    #region 模块结构
    /// <summary>
    /// 获取模块结构，
    /// 它按照引用深度升序排列模块
    /// </summary>
    /// <returns></returns>
    private IEnumerable<IEnumerable<Module>> ModuleStructure()
    {
        var modules = DisplayModule.GroupBy(x => x.ReferenceDepth).OrderBy(x => x.Key).ToArray();
        foreach (var item in modules)
        {
            yield return item.OrderBy(x => x.Name, CreateLocalization.ComparableStringChinese);
        }
    }
    #endregion
    #endregion
}
