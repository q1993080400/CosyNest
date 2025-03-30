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
    #region 用来渲染整个组件的委托
    /// <summary>
    /// 用来渲染整个组件的委托
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderFormViewerInfo<Model>> RenderComponent { get; set; }
    #endregion
    #region 用来渲染每个属性的委托
    /// <summary>
    /// 获取用来渲染每个属性的委托
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderFormViewerPropertyInfoBase<Model>> RenderProperty { get; set; }
    #endregion
    #region 用来进行递归渲染的委托
    /// <summary>
    /// 获取用来递归渲染属性的委托，
    /// 如果为<see langword="null"/>，
    /// 则使用一个默认方法，
    /// 它同时会作为一个级联参数传递给子组件
    /// </summary>
    [Parameter]
    public RenderFragment<RenderFormViewerPropertyInfoRecursion>? RenderRecursion { get; set; }
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
    #region 用来渲染区域的委托
    /// <summary>
    /// 用来渲染区域的委托，
    /// 区域指的是处于同一个组的属性
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderFormViewerRegionInfo<Model>> RenderRegion { get; set; }
    #endregion
    #region 用来渲染上传遮罩的委托
    /// <summary>
    /// 当本组件正在执行提交逻辑时，
    /// 如果这个逻辑包含一个正在进行的上传操作，
    /// 则渲染本委托，遮罩屏幕提醒用户正在上传，
    /// 如果为<see langword="null"/>，则不进行渲染
    /// </summary>
    [Parameter]
    public RenderFragment? RenderUploadMask { get; set; }
    #endregion
    #region 是否强制所有属性只读
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则强制所有属性只读，不会考虑<see cref="IsReadOnlyProperty"/>，
    /// 注意：如果<see cref="ExistingForm"/>返回<see langword="false"/>，它不会生效
    /// </summary>
    [Parameter]
    public bool ForceReadOnly { get; set; }
    #endregion
    #region 是否只读
    #region 正式属性
    /// <summary>
    /// 这个委托的第一个参数是当前属性，
    /// 第二个参数是当前模型，
    /// 返回值是该属性是否处于只读状态，不提供数据编辑功能，
    /// 注意：没有Set访问器的属性一律视为只读
    /// </summary>
    [Parameter]
    public Func<PropertyInfo, Model, bool> IsReadOnlyProperty { get; set; } = IsReadOnlyPropertyDefault;
    #endregion
    #region 根据特性进行判断
    /// <summary>
    /// 根据属性是否存在Set访问器，
    /// 以及属性身上的<see cref="RenderDataBaseAttribute.CanEditMod"/>特性，
    /// 判断该属性是否为只读属性
    /// </summary>
    /// <param name="property">要判断的属性</param>
    /// <param name="model">当前模型</param>
    /// <returns></returns>
    public static bool IsReadOnlyPropertyDefault(PropertyInfo property, Model model)
        => property.GetCustomAttribute<RenderDataBaseAttribute>()?.CanEditMod switch
        {
            null or CanEditMod.ReadOnly => true,
            CanEditMod.Auto => !property.CanWrite,
            { } canEditMod => throw canEditMod.Unrecognized()
        };
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
    public static Func<PropertyInfo, Model, bool> IsReadOnlyFilter(bool isReadOnly, params string[] propertyName)
    {
        var filterPropertyName = propertyName.ToHashSet();
        return (property, model) =>
        {
            var notAlmighty = !property.IsAlmighty();
            if (notAlmighty)
                return true;
            var exist = filterPropertyName.Contains(property.Name);
            return isReadOnly ? exist : !exist;
        };
    }
    #endregion
    #endregion
    #endregion
    #region 有关模型
    #region 初始模型
    /// <summary>
    /// 获取初始的模型，
    /// 注意：除非指定引用传递，否则<see cref="FormModel"/>只是它的副本，不是同一个引用，
    /// 并且它只会在调用<see cref="OnInitialized"/>时初始化一次，重复传入无效
    /// </summary>
    [Parameter]
    [EditorRequired]
    public Model InitializationModel { get; set; }
    #endregion
    #region 是否引用传递
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则表单模型使用引用传递，而不是创建它的副本
    /// </summary>
    [Parameter]
    public bool IsReference { get; set; }
    #endregion
    #region 用来判断是否为现有表单的委托
    #region 正式属性
    /// <summary>
    /// 这个委托传入模型，
    /// 返回当前表单是否为现有表单，
    /// 现有表单指的是已经保存到数据库中的表单，
    /// 它在某些业务逻辑上和没有保存，只是草稿的表单有区别
    /// </summary>
    [Parameter]
    public Func<Model, bool> ExistingForm { get; set; } = ExistingFormDefault;
    #endregion
    #region 默认方法
    /// <summary>
    /// 这个方法是<see cref="ExistingForm"/>的默认方法，
    /// 它返回当前表单是否为现有表单
    /// </summary>
    /// <param name="model">待判断的模型</param>
    /// <returns></returns>
    public static bool ExistingFormDefault(Model model)
        => model is IWithID { } entity && !entity.IsNew();
    #endregion
    #endregion
    #region 创建值改变时的函数的高阶函数
    /// <summary>
    /// 这个高阶函数传入属性渲染对象，
    /// 然后返回一个在属性的值被修改时触发的函数，
    /// 如果返回<see langword="null"/>，表示不执行
    /// </summary>
    [Parameter]
    public Func<RenderFormViewerPropertyInfoBase<Model>, Func<object?, Task>?>? CreatePropertyChangeEvent { get; set; }
    #endregion
    #region 用来筛选要渲染的属性的委托
    #region 正式方法
    /// <summary>
    /// 这个委托传入模型的类型，
    /// 返回它的哪些属性应该被渲染
    /// </summary>
    [Parameter]
    public Func<Type, IEnumerable<PropertyInfo>> FilterRenderPropertyInfo { get; set; } = FilterRenderPropertyInfoDefault;
    #endregion
    #region 默认方法
    /// <summary>
    /// 这个方法是<see cref="FilterRenderPropertyInfo"/>的默认方法，
    /// 它通过<see cref="RenderDataBaseAttribute"/>特性来确定是否渲染
    /// </summary>
    /// <param name="modelType">模型的类型</param>
    /// <returns></returns>
    public static IEnumerable<PropertyInfo> FilterRenderPropertyInfoDefault(Type modelType)
        => FilterRenderPropertyInfoBase(modelType).
        Where(x => !x.IsDefined<RenderDataConditionAttribute>());
    #endregion
    #region 仅渲染拥有某些条件的属性
    /// <summary>
    /// 返回一个高阶函数，
    /// 它仅渲染具有某些条件的属性
    /// </summary>
    /// <param name="conditions">拥有这些条件其中任意一项的属性会被认为是可渲染</param>
    /// <returns></returns>
    public static Func<Type, IEnumerable<PropertyInfo>> FilterRenderPropertyInfoOnly(params string[] conditions)
        => modelType =>
        FilterRenderPropertyInfoBase(modelType).
        Where(x => x.GetCustomAttributes<RenderDataConditionAttribute>().Any(x => conditions.Contains(x.RenderCondition)));
    #endregion
    #region 内部方法
    /// <summary>
    /// 这个方法是<see cref="FilterRenderPropertyInfo"/>的辅助方法，
    /// 它筛选所有公开，实例，不是索引器，且拥有<see cref="RenderDataBaseAttribute"/>的属性
    /// </summary>
    /// <param name="modelType">模型的类型</param>
    /// <returns></returns>
    private static IEnumerable<PropertyInfo> FilterRenderPropertyInfoBase(Type modelType)
        => modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance).
        Where(x => x.IsDefined<RenderDataBaseAttribute>() && !x.IsIndexing());
    #endregion
    #endregion
    #region 用来获取属性渲染参数的委托
    #region 正式属性
    /// <summary>
    /// 这个委托的参数是这个组件本身，
    /// 返回值是所有属性渲染参数，
    /// 它的顺序非常重要，组件会依次渲染它们
    /// </summary>
    [Parameter]
    public Func<FormViewer<Model>, IEnumerable<RenderFormViewerPropertyInfoBase<Model>>> GetRenderPropertyInfo { get; set; } = GetRenderPropertyInfoDefault;
    #endregion
    #region 默认方法
    /// <summary>
    /// 本方法是<see cref="GetRenderPropertyInfo"/>的默认方法，
    /// 它通过<see cref="RenderDataBaseAttribute"/>属性确定如何进行转换
    /// </summary>
    /// <param name="formViewer">当前表单视图组件</param>
    /// <returns></returns>
    public static IEnumerable<RenderFormViewerPropertyInfoBase<Model>> GetRenderPropertyInfoDefault(FormViewer<Model> formViewer)
    {
        var model = formViewer.FormModel;
        var modelType = model.GetType();
        var propertys = formViewer.FilterRenderPropertyInfo(modelType).
            Select(x => new
            {
                Item = x,
                Attribute = x.GetCustomAttribute<RenderDataBaseAttribute>()!,
            }).
        Where(x => x.Attribute is { }).ToArray().
        OrderBy(x => x.Attribute.Order).ToArray();
        var renderPropertyCount = propertys.Length;
        var forceReadOnly = formViewer.IsForceReadOnly(model);
        return [..propertys.Select((x, index) =>
        {
            var property = x.Item;
            var attribute = x.Attribute;
            var groupName = attribute.GroupName;
            var isReadOnly = forceReadOnly || !property.IsAlmighty() || formViewer.IsReadOnlyProperty(property, model);
            var previewFileTypeInfo = CreateDataObj.GetPreviewFileTypeInfo(modelType).HasPreviewFilePropertyInfo.GetValueOrDefault(property.Name);
            var inUpload = formViewer.InUpload && (previewFileTypeInfo?.IsStrict ?? false);
            var renderPreference = property.GetCustomAttribute<RenderPreferenceAttribute>()?.GetRenderPreference();
            RenderFormViewerPropertyInfoBase<Model> renderInfo = attribute switch
            {
                RenderDataAttribute renderDataAttribute => new RenderFormViewerPropertyInfo<Model>()
                {
                    FormModel = model,
                    GroupName = groupName,
                    IsReadOnly = isReadOnly,
                    Property = property,
                    Name = renderDataAttribute.Name,
                    ValueIfNullText = renderDataAttribute.ValueIfNullText,
                    InUpload = inUpload,
                    OnPropertyChange = null,
                    HasPreviewFilePropertyInfo = previewFileTypeInfo,
                    RenderPreference = renderPreference,
                    IsRecursion = renderDataAttribute.IsRecursion,
                    Order = index,
                    RenderPropertyCount = renderPropertyCount,
                    Describe = (renderDataAttribute.ShowDescribeInReadOnly || !isReadOnly) ? renderDataAttribute.Describe : null
                },
                RenderDataCustomAttribute => new RenderFormViewerPropertyInfoCustom<Model>()
                {
                    FormModel = model,
                    GroupName = groupName,
                    IsReadOnly = isReadOnly,
                    Property = property,
                    OnPropertyChange = null,
                    HasPreviewFilePropertyInfo = previewFileTypeInfo,
                    InUpload = inUpload,
                    Order = index,
                    RenderPropertyCount = renderPropertyCount,
                    RenderPreference = renderPreference
                },
                _ => throw new NotSupportedException("无法识别这个渲染数据特性")
            };
            var createPropertyChangeEvent = formViewer.CreatePropertyChangeEvent;
            return (isReadOnly, createPropertyChangeEvent) is (false, { }) ?
            renderInfo with
            {
                OnPropertyChange = createPropertyChangeEvent(renderInfo)
            } :
            renderInfo;
        })];
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
            InitializationModel = model,
            IsReadOnlyProperty = static (_, _) => true
        };
        formViewer.InitializationFormModel();
        return GetRenderPropertyInfoDefault(formViewer);
    }
    #endregion
    #endregion
    #endregion
    #region 关于业务逻辑
    #region 用来提交表单的业务逻辑
    /// <summary>
    /// 获取或设置用于提交表单的业务逻辑，
    /// 返回值是业务逻辑是否成功
    /// </summary>
    [Parameter]
    public Func<Model, Task<bool>> Submit { get; set; } = static _ => Task.FromResult(true);
    #endregion
    #region 是否提交后自动重置表单
    /// <summary>
    /// 获取提交后是否自动重置表单
    /// </summary>
    [Parameter]
    public bool ResetAfterSubmission { get; set; }
    #endregion
    #region 用来重置表单的业务逻辑
    /// <summary>
    /// 获取或设置用于重置表单的业务逻辑，
    /// 当执行这个委托的时候，模型已被恢复为初始状态
    /// </summary>
    [Parameter]
    public Func<Task> Resetting { get; set; }
    #endregion
    #region 用来删除表单的业务逻辑
    /// <summary>
    /// 获取或设置用于删除表单的业务逻辑，
    /// 它的参数就是当前表单的模型，
    /// 返回值是业务逻辑是否成功
    /// </summary>
    [Parameter]
    public Func<Model, Task<bool>>? Delete { get; set; }
    #endregion
    #region 用来取消表单的业务逻辑
    /// <summary>
    /// 用于取消表单，回到上级页面的业务逻辑，
    /// 如果不指定它，则不渲染取消按钮
    /// </summary>
    [Parameter]
    public Func<Task>? Cancellation { get; set; }
    #endregion
    #region 刷新目标
    /// <summary>
    /// 获取提交，删除成功或取消后应该刷新的组件，
    /// 它可以在子组件提交成功后刷新父组件，
    /// 如果为<see langword="null"/>，则刷新这个组件自己
    /// </summary>
    [Parameter]
    public ComponentBase? RefreshTarget { get; set; }
    #endregion
    #region 级联参数：上传上下文
    /// <summary>
    /// 获取级联的上传上下文，
    /// 当本组件存在正在上传的任务的时候，
    /// 会阻止用户离开页面，
    /// 它可以避免用户在尚未上传完毕时离开页面
    /// </summary>
    [CascadingParameter]
    private IFileUploadNavigationContext? FileUploadNavigationContext { get; set; }
    #endregion
    #endregion 
    #endregion
    #region 公开成员
    #region 表单模型
    private Model FormModelField;

    /// <summary>
    /// 获取实际要渲染的表单模型
    /// </summary>
    public Model FormModel => FormModelField;
    #endregion
    #region 是否正在上传
    private bool InUploadField;

    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示这个组件的模型含有需要上传的元素，
    /// 而且正在上传，你可以给予一些提示，
    /// 提醒用户上传完毕前不要离开
    /// </summary>
    public bool InUpload
        => InUploadField;
    #endregion
    #region 初始化表单模型（外部版本）
    /// <summary>
    /// 初始化并写入<see cref="FormModel"/>属性
    /// </summary>
    /// <returns></returns>
    public void InitializationFormModel()
    {
        CopyModel = InitializationModel;
        InitializationFormModelInternal();
    }
    #endregion
    #endregion
    #region 内部成员
    #region 是否强制所有属性只读
    /// <summary>
    /// 返回是否强制所有属性只读
    /// </summary>
    /// <param name="model">要判断的模型</param>
    /// <returns></returns>
    private bool IsForceReadOnly(Model model)
        => ForceReadOnly && ExistingForm(model);
    #endregion
    #region 初始化表单模型（内部版本）
    /// <summary>
    /// 初始化并写入<see cref="FormModel"/>属性，
    /// 它仅供内部调用
    /// </summary>
    /// <returns></returns>
    private void InitializationFormModelInternal()
    {
        FormModelField = IsReference ? CopyModel : CopyModel?.MemberwiseClone()!;
    }
    #endregion
    #region 待复制模型
    /// <summary>
    /// 这个模型专门用于复制
    /// </summary>
    private Model CopyModel { get; set; }
    #endregion
    #region 获取渲染参数
    /// <summary>
    /// 获取本组件的渲染参数
    /// </summary>
    /// <returns></returns>
    private RenderFormViewerInfo<Model> GetRenderInfo()
    {
        var renderFormViewerPropertyInfo = GetRenderPropertyInfo(this).ToArray();
        var renderPropertyGroup = renderFormViewerPropertyInfo.GroupBy(x => x.GroupName).ToArray();
        var formModel = FormModel;
        #region 返回渲染组的本地函数
        RenderFragment RenderGroup(IGrouping<string?, RenderFormViewerPropertyInfoBase<Model>> renderProperty)
        {
            var renderRegion = renderProperty.Select(x => (x, RenderProperty(x))).ToArray();
            var info = new RenderFormViewerRegionInfo<Model>()
            {
                GroupName = renderProperty.Key,
                RenderRegion = renderRegion,
                FormModel = formModel
            };
            return RenderRegion(info);
        }
        #endregion
        var notGroup = renderPropertyGroup.FirstOrDefault(x => x.Key is null);
        var renderMain = new RenderFormViewerMainInfo<Model>()
        {
            RenderGroup = renderPropertyGroup.Where(x => x.Key is { }).ToDictionary(x => x.Key!, RenderGroup),
            RenderNotGroup = notGroup is null ? _ => { } : RenderGroup(notGroup),
            FormModel = formModel
        };
        var isExistingForm = ExistingForm(formModel);
        #region 用来刷新组件的本地函数
        void RefreshComponent()
        {
            if (RefreshTarget is { })
                RefreshTarget.StateHasChanged();
            else
                this.StateHasChanged();
        }
        #endregion
        #region 用来重置的本地函数
        async Task Resetting()
        {
            InitializationFormModelInternal();
            await this.Resetting();
        }
        #endregion
        #region 如果刷新目标实现了IRefresh，则刷新它
        async Task Refresh()
        {
            if (RefreshTarget is IRefresh refresh)
                await refresh.Refresh();
        }
        #endregion
        #region 用来提交的本地函数
        async Task OnSubmit()
        {
            #region 提交逻辑主体
            async Task OnSubmitMain()
            {
                var previewFileTypeInfo = CreateDataObj.GetPreviewFileTypeInfo(formModel.GetType());
                if (!previewFileTypeInfo.HasPreviewFile)
                {
                    await NotUploadFile();
                    return;
                }
                var allPreviewFile = previewFileTypeInfo.AllPreviewFile(formModel, true).ToArray();
                var uploadFiles = allPreviewFile.SelectMany(x => x.PreviewFilePropertyInfo.GetPreviewFile(x.Target)).
                    OfType<IHasUploadFileClient>().WhereEnableAndNotUpload().ToArray();
                if (uploadFiles.Length > 0)
                    await HasUploadFile();
                else
                    await NotUploadFile();
                #region 包含上传逻辑
                async Task HasUploadFile()
                {
                    try
                    {
                        using var uploadLock = FileUploadNavigationContext.UploadLockSecure();
                        InUploadField = true;
                        this.StateHasChanged();
                        var isSuccess = await NotUploadFile();
                        if (isSuccess)
                        {
                            foreach (var uploadFile in uploadFiles)
                            {
                                uploadFile.UploadCompleted();
                            }
                        }
                    }
                    finally
                    {
                        InUploadField = false;
                    }
                }
                #endregion
                #region 不包含上传逻辑
                async Task<bool> NotUploadFile()
                {
                    var isSuccess = await Submit(formModel);
                    if (!isSuccess)
                        return false;
                    CopyModel = formModel.MemberwiseClone();
                    await Refresh();
                    if (Cancellation is { } cancellation)
                    {
                        await cancellation();
                        return true;
                    }
                    if (ResetAfterSubmission)
                    {
                        await Resetting();
                        return true;
                    }
                    return true;
                }
                #endregion
            }
            #endregion
            await OnSubmitMain();
            RefreshComponent();
        }
        #endregion
        #region 用来取消的本地函数
        async Task OnCancellation()
        {
            if (Cancellation is { })
                await Cancellation();
            if (RefreshTarget is { })
                RefreshTarget.StateHasChanged();
        }
        #endregion
        #region 用来删除的本地函数
        async Task OnDelete()
        {
            var isSuccess = await Delete(formModel);
            if (isSuccess)
            {
                await Refresh();
                await OnCancellation();
            }
        }
        #endregion
        var renderSubmit = new RenderSubmitInfo<Model>()
        {
            Resetting = Resetting,
            IsExistingForm = isExistingForm,
            FormModel = formModel,
            InUpload = InUpload,
            CanEdit = renderFormViewerPropertyInfo.Any(x => !x.IsReadOnly),
            Delete = (isExistingForm, Delete) is ({ }, { }) ? OnDelete : null,
            Submit = OnSubmit,
            RenderUploadMask = (InUpload, RenderUploadMask) is (true, { }) ?
            RenderUploadMask : static _ => { },
            Cancellation = Cancellation is null ? null : OnCancellation
        };
        return new()
        {
            RenderMain = RenderMain(renderMain),
            RenderSubmit = RenderSubmit(renderSubmit),
            FormModel = formModel
        };
    }
    #endregion
    #region 重写OnInitialized
    protected override void OnInitialized()
    {
        InitializationFormModel();
    }
    #endregion
    #endregion
    #region 构造函数
    public FormViewer()
    {
        Resetting = () =>
        {
            this.StateHasChanged();
            return Task.CompletedTask;
        };
    }
    #endregion
}
