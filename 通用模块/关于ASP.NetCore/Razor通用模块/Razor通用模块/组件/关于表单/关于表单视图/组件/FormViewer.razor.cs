using System.ComponentModel.DataAnnotations;
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
    #region 有关渲染
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
    public RenderFragment<IEnumerable<RenderFormViewerMainInfo<Model>>> RenderMain { get; set; }
    #endregion
    #region 用来渲染提交部分的委托
    /// <summary>
    /// 用来渲染提交部分的委托，
    /// 提交部分指的是包含提交，删除，重置等按钮的区域
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderSubmitInfo> RenderSubmit { get; set; }
    #endregion
    #region 是否仅显示
    /// <summary>
    /// 这个委托的第一个参数是当前属性，
    /// 第二个参数是当前模型，
    /// 返回值是该属性是否处于只读状态，不提供数据编辑功能
    /// </summary>
    [Parameter]
    public Func<PropertyInfo, Model, bool> IsReadOnly { get; set; } = (_, _) => false;
    #endregion
    #region 是否显示提交部分
    /// <summary>
    /// 这个委托的参数是当前模型，
    /// 返回值是是否显示提交部分
    /// </summary>
    [Parameter]
    public Func<Model, bool> ShowSubmit { get; set; } = _ => true;
    #endregion
    #endregion
    #region 有关模型
    #region 初始化模型
    /// <summary>
    /// 获取用来初始化模型的委托，
    /// 组件实际上会获取它的返回值的浅拷贝，
    /// 建议每次执行此委托，都返回一个新的模型
    /// </summary>
    [Parameter]
    public Func<Task<Model>> InitializationModel { get; set; }
        = () => Task.FromResult<Model>(new());
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
    /// 它的参数是要筛选的属性,
    /// 如果不指定，则使用一个默认方法
    /// </summary>
    [Parameter]
    public Func<PropertyInfo, bool> FilterProperties { get; set; } = FilterPropertiesDefault;
    #endregion
    #region 默认方法
    /// <summary>
    /// 本方法是<see cref="FilterProperties"/>的默认方法，
    /// 它选中具有<see cref="DisplayAttribute"/>特性的属性，并将其作为渲染的成员，
    /// 如果<paramref name="isOnlyDisplay"/>为<see langword="false"/>，
    /// 还要求这个属性是全能属性
    /// </summary>
    /// <param name="property">要筛选的属性</param>
    /// <returns></returns>
    public static bool FilterPropertiesDefault(PropertyInfo property)
        => property.HasAttributes<DisplayAttribute>();
    #endregion
    #endregion
    #region 用来将属性转换为渲染参数的委托
    #region 正式属性
    /// <summary>
    /// 这个委托的第一个参数是要渲染的属性，
    /// 第二个参数是这个组件本身，
    /// 返回值是将其转换后的渲染参数，
    /// 它的顺序非常重要，组件会依次渲染它们
    /// </summary>
    [Parameter]
    public Func<IEnumerable<PropertyInfo>, FormViewer<Model>, IEnumerable<RenderFormViewerPropertyInfo<Model>>> ToPropertyInfo { get; set; } = ToPropertyInfoDefault;
    #endregion
    #region 默认方法
    /// <summary>
    /// 本方法是<see cref="ToPropertyInfo"/>的默认方法，
    /// 它通过<see cref="DisplayAttribute"/>属性确定如何进行转换
    /// </summary>
    /// <param name="property">要筛选的属性</param>
    /// <param name="formViewer">当前表单视图组件</param>
    /// <returns></returns>
    public static IEnumerable<RenderFormViewerPropertyInfo<Model>> ToPropertyInfoDefault(IEnumerable<PropertyInfo> property, FormViewer<Model> formViewer)
    {
        var propertys = property.Select(x =>
        {
            var display = x.GetCustomAttribute<DisplayAttribute>();
            return new
            {
                Item = x,
                Name = display?.Name ?? x.Name,
                Order = display?.Order ?? 0,
            };
        }).ToArray().OrderBy(x => x.Order).ToArrayIfDeBug();
        var model = formViewer.FormModel;
        return propertys.Select(x =>
        {
            var property = x.Item;
            return new RenderFormViewerPropertyInfo<Model>()
            {
                FormModel = model,
                Property = property,
                PropertyName = x.Name,
                IsReadOnly = !formViewer.ShowSubmit(model) || !property.IsAlmighty() || formViewer.IsReadOnly(property, model),
                OnPropertyChangeed = formViewer.OnPropertyChangeed
            };
        }).ToArray();
    }
    #endregion
    #endregion
    #region 用来验证数据的委托
    #region 正式属性
    /// <summary>
    /// 获取用来验证数据的委托，
    /// 如果为不指定，则默认不进行验证，
    /// 将验证的责任转交给服务端
    /// </summary>
    [Parameter]
    public DataVerify Verify { get; set; } = VerifyDefault;
    #endregion
    #region 默认方法
    #region 创建数据验证默认委托
    /// <summary>
    /// 用于验证数据的默认方法，
    /// 它实际不进行验证，
    /// 将验证的责任交给服务端
    /// </summary>
    /// <inheritdoc cref="DataVerify"/>
    public static VerificationResults VerifyDefault(object obj)
        => new()
        {
            Data = obj,
            FailureReason = Array.Empty<(PropertyInfo, string)>()
        };
    #endregion
    #endregion
    #endregion
    #region 数据属性改变时的委托
    /// <summary>
    /// 当数据属性改变时，执行这个委托，
    /// 它的参数就是当前属性渲染参数
    /// </summary>
    [Parameter]
    public Func<RenderFormViewerPropertyInfo<Model>, Task> OnPropertyChangeed { get; set; } = _ => Task.CompletedTask;
    #endregion
    #endregion
    #endregion
    #region 公开成员
    #region 表单模型
    /// <summary>
    /// 获取要渲染的表单模型
    /// </summary>
    public Model FormModel { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 获取渲染参数
    /// <summary>
    /// 获取本组件的渲染参数
    /// </summary>
    /// <returns></returns>
    private RenderFormViewerInfo<Model> GetRenderInfo()
    {
        var properties = typeof(Model).GetProperties().Where(x => !x.IsStatic());
        var renderProperties = properties.Where(FilterProperties).ToArrayIfDeBug();
        var renderFormViewerPropertyInfo = ToPropertyInfo(renderProperties, this).ToArray();
        var renderPropertys = renderFormViewerPropertyInfo.Select(x => (x, RenderProperty(x))).ToArray();
        var renderMain = renderPropertys.Select(x => new RenderFormViewerMainInfo<Model>()
        {
            Property = x.x.Property,
            PropertyName = x.x.PropertyName,
            Render = x.Item2,
            FormModel = FormModel
        }).ToArray();
        var renderSubmit = ShowSubmit(FormModel) ? new RenderSubmitInfo()
        {
            Resetting = async () =>
            {
                var model = await InitializationModel();
                FormModel = model.MemberwiseClone();
            },
            ModelAndVerify = () => Verify(FormModel),
            ExistingForms = ExistingForms(FormModel),
        } : null;
        return new()
        {
            RenderMain = RenderMain(renderMain),
            RenderSubmit = renderSubmit is { } ?
            RenderSubmit(renderSubmit) :
            _ => { },
            FormModel = FormModel
        };
    }
    #endregion
    #region 重写OnInitializedAsync
    protected override async Task OnInitializedAsync()
    {
        var model = await InitializationModel();
        FormModel = model.MemberwiseClone();
    }
    #endregion
    #endregion
}
