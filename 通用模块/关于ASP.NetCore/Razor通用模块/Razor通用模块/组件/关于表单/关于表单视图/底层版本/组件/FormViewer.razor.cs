using System.DataFrancis;
using System.Reflection;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件是一个表单视图，
/// 它能够正确地渲染表单
/// </summary>
/// <typeparam name="Model">表单模型的类型</typeparam>
public sealed partial class FormViewer<Model> : ComponentBase
    where Model : class
{
    #region 组件参数
    #region 有关渲染
    #region 用来渲染每个属性的委托
    /// <summary>
    /// 获取用来渲染每个属性的委托
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderFormViewerPropertyInfoBase<Model>> RenderProperty { get; set; }
    #endregion
    #region 用来渲染整个组件的委托
    /// <summary>
    /// 用来渲染整个组件的委托
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderFormViewerInfo<Model>> RenderComponent { get; set; }
    #endregion
    #region 用来渲染主体部分的委托
    /// <summary>
    /// 获取用来渲染主体部分的委托，
    /// 主体部分指的是表单的所有属性部分，不包括提交重置按钮等，
    /// 它的参数是一个集合，枚举了渲染所有属性的委托
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderFormViewerMainInfo<Model>> RenderMain { get; set; }
    #endregion
    #region 用来渲染提交部分的委托
    /// <summary>
    /// 用来渲染提交部分的委托，
    /// 提交部分指的是包含提交，删除，重置等按钮的区域
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderSubmitInfo<Model>> RenderSubmit { get; set; }
    #endregion
    #region 是否可编辑
    /// <summary>
    /// 获取这个表单是否可编辑，
    /// 它比<see cref="IsReadOnlyProperty"/>优先级更高，
    /// 只有在它为<see langword="true"/>的时候，才会去检查后者
    /// </summary>
    [Parameter]
    public bool CanEdit { get; set; }
    #endregion
    #region 是否仅显示
    #region 正式方法
    /// <summary>
    /// 这个委托的第一个参数是当前属性，
    /// 第二个参数是当前模型，
    /// 返回值是该属性是否处于只读状态，不提供数据编辑功能
    /// </summary>
    [Parameter]
    public Func<PropertyInfo, Model, bool> IsReadOnlyProperty { get; set; } = PropertyStateJudge;
    #endregion
    #region 根据特性进行判断
    /// <summary>
    /// 获取一个委托，
    /// 它根据属性身上的<see cref="RenderDataBaseAttribute.IsReadOnly"/>特性判断该属性是否为只读属性
    /// </summary>
    public static Func<PropertyInfo, Model, bool> PropertyStateJudge { get; }
        = static (property, model)
        => property.GetCustomAttribute<RenderDataBaseAttribute>()?.IsReadOnly ?? true;
    #endregion
    #region 返回属性是否仅显示的高阶函数
    /// <summary>
    /// 这个高阶函数返回一个函数，
    /// 它将所有的属性设置为只读或可写
    /// </summary>
    /// <param name="isReadOnly">如果这个值为<see langword="true"/>，
    /// 则将所有属性设置为只读，否则将所有属性设置为可写</param>
    /// <returns></returns>
    public static Func<PropertyInfo, Model, bool> PropertyStateUnified(bool isReadOnly)
        => isReadOnly ?
        static (_, _) => true :
        static (_, _) => false;
    #endregion
    #region 返回过滤指定属性的高阶函数
    /// <summary>
    /// 返回一个高阶函数，
    /// 它将指定名称的属性设置为只读或可写
    /// </summary>
    /// <param name="isReadOnly">如果这个值为<see langword="true"/>，
    /// 表示将名称存在于<paramref name="propertyName"/>的属性设置为只读，否则设置为可写</param>
    /// <param name="propertyName">枚举要筛选的属性的名称</param>
    /// <returns></returns>
    public static Func<PropertyInfo, Model, bool> PropertyStateFilter(bool isReadOnly, params string[] propertyName)
    {
        var filterPropertyName = propertyName.ToHashSet();
        return (property, model) =>
        {
            var exist = filterPropertyName.Contains(property.Name);
            return isReadOnly ? exist : !exist;
        };
    }
    #endregion
    #endregion
    #endregion
    #region 有关模型
    #region 初始化模型
    /// <summary>
    /// 获取用来初始化模型的委托
    /// </summary>
    [Parameter]
    [EditorRequired]
    public Func<Task<Model>> InitializationModel { get; set; }
    #endregion
    #region 模型是否按值引用
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示模型按值引用，否则按引用引用
    /// </summary>
    [Parameter]
    public bool IsValueReference { get; set; } = true;
    #endregion
    #region 用来判断是否为现有表单的委托
    #region 正式属性
    /// <summary>
    /// 这个委托传入模型，
    /// 返回当前表单是否为现有表单，
    /// 现有表单支持修改和删除，不支持新增
    /// </summary>
    [Parameter]
    public Func<Model, bool> ExistingForms { get; set; } = ExistingFormsDefault;
    #endregion
    #region 默认方法
    /// <summary>
    /// 这个方法是<see cref="ExistingForms"/>的默认方法，
    /// 它返回当前表单是否为现有表单
    /// </summary>
    /// <param name="model">待判断的模型</param>
    /// <returns></returns>
    public static bool ExistingFormsDefault(Model model)
        => model is IWithID { ID: { } id } && id != default;
    #endregion
    #endregion
    #region 用来获取属性渲染参数的委托
    #region 正式属性
    /// <summary>
    /// 这个委托的第一个参数是模型的类型，
    /// 第二个参数是这个组件本身，
    /// 返回值是所有属性渲染参数，
    /// 它的顺序非常重要，组件会依次渲染它们
    /// </summary>
    [Parameter]
    public Func<Type, FormViewer<Model>, IEnumerable<RenderFormViewerPropertyInfoBase<Model>>> GetRenderPropertyInfo { get; set; } = GetRenderPropertyInfoDefault;
    #endregion
    #region 默认方法
    /// <summary>
    /// 本方法是<see cref="GetRenderPropertyInfo"/>的默认方法，
    /// 它通过<see cref="RenderDataBaseAttribute"/>属性确定如何进行转换
    /// </summary>
    /// <param name="modelType">模型的类型</param>
    /// <param name="formViewer">当前表单视图组件</param>
    /// <returns></returns>
    public static IEnumerable<RenderFormViewerPropertyInfoBase<Model>> GetRenderPropertyInfoDefault(Type modelType, FormViewer<Model> formViewer)
    {
        var property = modelType.GetProperties().Where(x => !x.IsStatic()).ToArray();
        var propertys = property.Select(x =>
        {
            var renderData = x.GetCustomAttribute<RenderDataBaseAttribute>();
            return new
            {
                Item = x,
                Attribute = renderData!,
            };
        }).
        Where(x => x.Attribute is { }).ToArray().
        OrderBy(x => x.Attribute.Order).ToArray();
        var model = formViewer.FormModel;
        return propertys.Select(x =>
        {
            var property = x.Item;
            var attribute = x.Attribute;
            var groupName = attribute.GroupName;
            var isReadOnlyProperty = !formViewer.CanEdit || !property.IsAlmighty() || formViewer.IsReadOnlyProperty(property, model);
            return attribute switch
            {
                RenderDataAttribute renderDataAttribute => (RenderFormViewerPropertyInfoBase<Model>)new RenderFormViewerPropertyInfo<Model>()
                {
                    FormModel = model,
                    GroupName = groupName,
                    IsReadOnly = isReadOnlyProperty,
                    Property = property,
                    Name = renderDataAttribute.Name,
                    RenderPreference = new()
                    {
                        Format = renderDataAttribute.Format,
                        RenderLongTextRows = renderDataAttribute.RenderLongTextRows
                    }
                },
                RenderDataCustomAttribute => new RenderFormViewerPropertyInfoCustom<Model>()
                {
                    FormModel = model,
                    GroupName = groupName,
                    IsReadOnly = isReadOnlyProperty,
                    Property = property,
                },
                _ => throw new NotSupportedException("无法识别这个渲染数据特性")
            };
        }).ToArray();
    }
    #endregion
    #region 外部方法
    /// <summary>
    /// 当您需要在外部获取渲染属性的条件，
    /// 而不是把它们放到组件内渲染时，
    /// 可以调用本方法
    /// </summary>
    /// <param name="model">要渲染的模型</param>
    public static IEnumerable<RenderFormViewerPropertyInfoBase<Model>> GetRenderPropertyInfoExternal(Model model)
    {
        var formViewer = new FormViewer<Model>()
        {
            FormModel = model,
            IsReadOnlyProperty = static (_, _) => true
        };
        return GetRenderPropertyInfoDefault(typeof(Model), formViewer);
    }
    #endregion
    #endregion
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
        var renderFormViewerPropertyInfo = GetRenderPropertyInfo(typeof(Model), this).ToArray();
        var renderPropertyGroup = renderFormViewerPropertyInfo.GroupBy(x => x.GroupName).ToArray();
        #region 返回渲染组的本地函数
        RenderFormViewerRegionInfo<Model> RenderGroup(IGrouping<string?, RenderFormViewerPropertyInfoBase<Model>> renderProperty)
        {
            var renderRegion = renderProperty.Select(x => (x, RenderProperty(x))).ToArray();
            return new RenderFormViewerRegionInfo<Model>()
            {
                GroupName = renderProperty.Key,
                RenderRegion = renderRegion
            };
        }
        #endregion
        var renderMain = new RenderFormViewerMainInfo<Model>()
        {
            RenderGroup = renderPropertyGroup.Select(RenderGroup).ToArray(),
            FormModel = FormModel
        };
        var renderSubmit = new RenderSubmitInfo<Model>()
        {
            Resetting = async () =>
            {
                await SetModel();
                this.StateHasChanged();
            },
            ExistingForms = ExistingForms(FormModel),
            FormModel = FormModel,
            CanEdit = renderFormViewerPropertyInfo.Any(x => !x.IsReadOnly)
        };
        return new()
        {
            RenderMain = RenderMain(renderMain),
            RenderSubmit = RenderSubmit(renderSubmit),
            FormModel = FormModel
        };
    }
    #endregion
    #region 重写OnInitializedAsync
    protected override async Task OnInitializedAsync()
    {
        await SetModel();
    }
    #endregion
    #region 写入模型
    /// <summary>
    /// 初始化并写入<see cref="FormModel"/>属性
    /// </summary>
    /// <returns></returns>
    private async Task SetModel()
    {
        var model = await InitializationModel();
        FormModel = IsValueReference ? model.MemberwiseClone() : model;
    }
    #endregion
    #endregion
}
