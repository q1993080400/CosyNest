using System.ComponentModel.DataAnnotations;
using System.DataFrancis;
using System.DataFrancis.EntityDescribe;
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
    #region 是否仅显示
    /// <summary>
    /// 这个委托的第一个参数是当前属性，
    /// 第二个参数是当前模型，
    /// 返回值是该属性是否处于只读状态，不提供数据编辑功能，
    /// 与<see cref="ShowSubmit"/>不同的是，
    /// 本委托是属性级的，而它是模型级的
    /// </summary>
    [Parameter]
    public Func<PropertyInfo, Model, bool> IsReadOnlyProperty { get; set; } = (_, _) => false;
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
    /// <summary>
    /// 这个委托传入模型，
    /// 返回当前表单是否为现有表单，
    /// 现有表单支持修改和删除，不支持新增
    /// </summary>
    [Parameter]
    public Func<Model, bool> ExistingForms { get; set; } = _ => false;
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
    /// 它通过<see cref="DisplayAttribute"/>属性确定如何进行转换
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
        var showSubmit = formViewer.ShowSubmit(model);
        return propertys.Select(x =>
        {
            var property = x.Item;
            var attribute = x.Attribute;
            var groupName = attribute.GroupName;
            var isReadOnlyProperty = !showSubmit || !property.IsAlmighty() || formViewer.IsReadOnlyProperty(property, model);
            var onPropertyChangeed = formViewer.OnPropertyChangeed;
            return attribute switch
            {
                RenderDataAttribute renderDataAttribute => (RenderFormViewerPropertyInfoBase<Model>)new RenderFormViewerPropertyInfo<Model>()
                {
                    FormModel = model,
                    GroupName = groupName,
                    IsReadOnlyProperty = isReadOnlyProperty,
                    OnPropertyChangeed = onPropertyChangeed,
                    Property = property,
                    PropertyName = renderDataAttribute.Name,
                },
                RenderDataCustomAttribute => new RenderFormViewerPropertyInfoCustom<Model>()
                {
                    FormModel = model,
                    GroupName = groupName,
                    IsReadOnlyProperty = isReadOnlyProperty,
                    OnPropertyChangeed = onPropertyChangeed,
                    Property = property,
                },
                _ => throw new NotSupportedException("无法识别这个渲染数据特性")
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
            FailureReason = []
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
    public Func<RenderFormViewerPropertyInfoBase<Model>, Task> OnPropertyChangeed { get; set; } = _ => Task.CompletedTask;
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
        var renderSubmit = ShowSubmit(FormModel) ? new RenderSubmitInfo<Model>()
        {
            Resetting = async () =>
            {
                var model = await InitializationModel();
                FormModel = IsValueReference ? model.MemberwiseClone() : model;
            },
            ModelAndVerify = () => Verify(FormModel),
            ExistingForms = ExistingForms(FormModel),
            FormModel = FormModel
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
        FormModel = IsValueReference ? model.MemberwiseClone() : model;
    }
    #endregion
    #endregion
}
