using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.DataFrancis;
using System.DataFrancis.EntityDescribe;
using System.Reflection;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件是一个表单视图，
/// 它能够正确地渲染表单
/// </summary>
/// <typeparam name="Model">表单模型的类型</typeparam>
public sealed partial class FormViewer<Model> : ComponentBase, IContentComponent<RenderFragment<RenderFormViewerInfo<Model>>>
    where Model : class, new()
{
    #region 组件参数
    #region 用来渲染每个属性的委托
    /// <summary>
    /// 获取用来渲染每个属性的委托
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderFormViewerPropertyInfo<Model>> RenderProperty { get; set; }
    #endregion
    #region 用来渲染组件的委托
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderFormViewerInfo<Model>>? ChildContent { get; set; }
    #endregion
    #region 用来渲染主体部分的委托
    /// <summary>
    /// 获取用来渲染主体部分的委托，
    /// 主体部分指的是表单的所有属性部分，不包括提交重置按钮等，
    /// 它的参数是一个集合，枚举了渲染所有属性的委托
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<IEnumerable<RenderFormViewerMainInfo>> RenderMain { get; set; }
    #endregion
    #region 用来初始化模型的委托
    /// <summary>
    /// 获取用来初始化模型的委托，
    /// 如果它返回<see langword="null"/>，
    /// 表示不需要初始化模型
    /// </summary>
    [Parameter]
    public Func<Task<Model?>> InitializationModel { get; set; }
        = () => Task.FromResult<Model?>(null);
    #endregion
    #region 用来重置模型的委托
    /// <summary>
    /// 获取用来重置模型的委托，
    /// 它的参数是旧模型，返回值是新模型
    /// </summary>
    [Parameter]
    public Func<Model, Model> ResetModel { get; set; } = _ => new();
    #endregion
    #region 用来判断是否为现有表单的委托
    /// <summary>
    /// 这个委托传入模型，
    /// 返回当前表单是否为现有表单，
    /// 现有表单支持修改和删除，不支持新增
    /// </summary>
    [Parameter]
    public Func<Model, bool> ExistingForms { get; set; } = _ => false;
    #endregion
    #region 用来筛选属性的委托
    #region 正式属性
    /// <summary>
    /// 这个委托可以用来筛选表单模型的哪些属性需要渲染，
    /// 如果不指定，则使用一个默认方法，
    /// 注意：如果属性不是全能属性，则直接排除，不会参与筛选
    /// </summary>
    [Parameter]
    public Func<PropertyInfo, bool> FilterProperties { get; set; } = FilterPropertiesDefault;
    #endregion
    #region 默认方法
    /// <summary>
    /// 本方法是<see cref="FilterProperties"/>的默认方法，
    /// 它选中没有<see cref="NotMappedAttribute"/>特性的全能属性，并将其作为渲染的成员
    /// </summary>
    /// <param name="property">要筛选的属性</param>
    /// <returns></returns>
    public static bool FilterPropertiesDefault(PropertyInfo property)
        => !property.HasAttributes<NotMappedAttribute>();
    #endregion
    #endregion
    #region 用来将属性转换为渲染参数的委托
    #region 正式属性
    /// <summary>
    /// 这个委托的第一个参数是要渲染的属性，
    /// 第二个参数是当前表单模型，
    /// 返回值是将其转换后的渲染参数，
    /// 它的顺序非常重要，组件会依次渲染它们
    /// </summary>
    [Parameter]
    public Func<IEnumerable<PropertyInfo>, Model, IEnumerable<RenderFormViewerPropertyInfo<Model>>> ToPropertyInfo { get; set; } = ToPropertyInfoDefault;
    #endregion
    #region 默认方法
    /// <summary>
    /// 本方法是<see cref="ToPropertyInfo"/>的默认方法，
    /// 它通过<see cref="DisplayAttribute"/>属性确定如何进行转换
    /// </summary>
    /// <param name="property">要筛选的属性</param>
    /// <param name="model">当前表单模型</param>
    /// <returns></returns>
    public static IEnumerable<RenderFormViewerPropertyInfo<Model>> ToPropertyInfoDefault(IEnumerable<PropertyInfo> property, Model model)
    {
        var propertys = property.Select(x =>
        {
            var display = x.GetCustomAttribute<DisplayAttribute>();
            return (x, display?.Name ?? x.Name, display?.Order ?? 0);
        }).ToArray().OrderBy(x => x.Item3).ToArrayIfDeBug();
        return propertys.Select(x => new RenderFormViewerPropertyInfo<Model>()
        {
            FormModel = model,
            Property = x.x,
            PropertyName = x.Item2
        }).ToArray();
    }
    #endregion
    #endregion
    #region 用来验证数据的委托
    /// <summary>
    /// 获取用来验证数据的委托，
    /// 如果为不指定，则使用一个默认方法
    /// </summary>
    [Parameter]
    public DataVerify Verify { get; set; } = CreateDataObj.DataVerifyDefault;
    #endregion
    #endregion
    #region 内部成员
    #region 表单模型
    /// <summary>
    /// 获取要渲染的表单模型
    /// </summary>
    private Model FormModel { get; set; }
    #endregion
    #region 获取渲染参数
    /// <summary>
    /// 获取本组件的渲染参数
    /// </summary>
    /// <returns></returns>
    private RenderFormViewerInfo<Model> GetRenderInfo()
    {
        var properties = typeof(Model).GetTypeData().AlmightyPropertys;
        var renderProperties = properties.Where(FilterProperties).ToArrayIfDeBug();
        var renderFormViewerPropertyInfo = ToPropertyInfo(renderProperties, FormModel).ToArray();
        var renderPropertys = renderFormViewerPropertyInfo.Select(x => (x, RenderProperty(x))).ToArray();
        var renderMain = renderPropertys.Select(x => new RenderFormViewerMainInfo()
        {
            Property = x.x.Property,
            PropertyName = x.x.PropertyName,
            Render = x.Item2
        }).ToArray();
        return new()
        {
            RenderMain = RenderMain(renderMain),
            Resetting = () => FormModel = ResetModel(FormModel),
            ModelAndVerify = () => Verify(FormModel),
            ExistingForms = ExistingForms(FormModel)
        };
    }
    #endregion
    #region 重写OnInitializedAsync
    protected override async Task OnInitializedAsync()
    {
        var model = await InitializationModel();
        FormModel = model ?? new();
    }
    #endregion
    #endregion
}
