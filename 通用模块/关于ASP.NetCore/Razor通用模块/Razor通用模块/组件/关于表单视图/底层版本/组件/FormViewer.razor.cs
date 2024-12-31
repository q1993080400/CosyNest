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
    #region 用来渲染区域的委托
    /// <summary>
    /// 用来渲染区域的委托，
    /// 区域指的是处于同一个组的属性
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderFormViewerRegionInfo<Model>> RenderRegion { get; set; }
    #endregion
    #region 是否只读
    #region 正式属性
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
    /// 它根据属性是否可写，
    /// 以及属性身上的<see cref="RenderDataBaseAttribute.CanEditMod"/>特性，
    /// 判断该属性是否为只读属性
    /// </summary>
    public static Func<PropertyInfo, Model, bool> PropertyStateJudge { get; }
        = static (property, model) =>
        {
            var notAlmighty = !property.IsAlmighty();
            if (notAlmighty)
                return true;
            return property.GetCustomAttribute<RenderDataBaseAttribute>()?.CanEditMod switch
            {
                null or CanEditMod.ReadOnly => true,
                CanEditMod.Auto => notAlmighty,
                { } canEditMod => throw canEditMod.Unrecognized()
            };
        };
    #endregion
    #region 直接将所有属性设置为只读
    /// <summary>
    /// 这个高阶函数返回一个函数，
    /// 它直接将所有属性设置为只读
    /// </summary>
    /// <returns></returns>
    public static Func<PropertyInfo, Model, bool> PropertyAllReadOnly()
        => static (_, _) => true;
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
    /// 注意：<see cref="FormModel"/>只是它的副本，不是同一个引用，
    /// 并且它只会在调用<see cref="OnInitialized"/>时初始化一次，重复传入无效
    /// </summary>
    [Parameter]
    [EditorRequired]
    public Model InitializationModel { get; set; }
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
    #region 值转换函数
    /// <summary>
    /// 这个函数的第一个参数是属性，
    /// 第二个参数是属性的值，
    /// 返回值是渲染的时候应该渲染的值，
    /// 通过它可以实现某些特殊操作，
    /// 例如把空字符串渲染成不明
    /// </summary>
    [Parameter]
    public Func<PropertyInfo, object?, object?>? PropertyValueConvert { get; set; }
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
        var propertys = typeof(Model).GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(x =>
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
            var isReadOnly = formViewer.IsReadOnlyProperty(property, model);
            var readOnlyConvert = isReadOnly ? formViewer.PropertyValueConvert : null;
            var previewFilePropertyDescribe = HasPreviewFilePropertyNatureState.Get(typeof(Model)).
            PreviewFilePropertyDescribe.GetValueOrDefault(property.Name);
            var inUpload = formViewer.InUpload && (previewFilePropertyDescribe?.IsStrict ?? false);
            var renderInfo = attribute switch
            {
                RenderDataAttribute renderDataAttribute => (RenderFormViewerPropertyInfoBase<Model>)new RenderFormViewerPropertyInfo<Model>()
                {
                    FormModel = model,
                    GroupName = groupName,
                    IsReadOnly = isReadOnly,
                    Property = property,
                    Name = renderDataAttribute.Name,
                    PropertyValueConvert = readOnlyConvert,
                    InUpload = inUpload,
                    OnPropertyChange = null,
                    PreviewFilePropertyDescribe = previewFilePropertyDescribe,
                    RenderPreference = new()
                    {
                        Format = renderDataAttribute.Format,
                        RenderLongTextRows = renderDataAttribute.RenderLongTextRows,
                        RenderEnum = renderDataAttribute.RenderEnum,
                    }
                },
                RenderDataCustomAttribute => new RenderFormViewerPropertyInfoCustom<Model>()
                {
                    FormModel = model,
                    GroupName = groupName,
                    IsReadOnly = isReadOnly,
                    Property = property,
                    PropertyValueConvert = readOnlyConvert,
                    OnPropertyChange = null,
                    PreviewFilePropertyDescribe = previewFilePropertyDescribe,
                    InUpload = inUpload
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
    #region 初始化表单模型
    /// <summary>
    /// 初始化并写入<see cref="FormModel"/>属性
    /// </summary>
    /// <returns></returns>
    public void InitializationFormModel()
    {
        FormModelField = CopyModel.MemberwiseClone();
        var hasPreviewFilePropertyNatureState = HasPreviewFilePropertyNatureState.Get(typeof(Model));
        if (!hasPreviewFilePropertyNatureState.HasPreviewFile)
            return;
        var propertyDescribes = hasPreviewFilePropertyNatureState.PreviewFilePropertyDescribe.Values.
            Where(x => x.IsStrict).ToArray();
        foreach (var propertyDescribe in propertyDescribes)
        {
            var value = propertyDescribe.GetFiles(FormModelField).
                Select(x => x.MemberwiseClone()).Cast<IHasPreviewFile>().ToArray();
            if (value.Length is 0)
                continue;
            object setValue = propertyDescribe.Multiple ?
                value : value.Single();
            propertyDescribe.Property.SetValue(FormModelField, setValue);
        }
    }
    #endregion
    #endregion
    #region 内部成员
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
        var existingForms = ExistingForms(formModel);
        #region 用来刷新的本地函数
        async Task Resetting()
        {
            InitializationFormModel();
            await this.Resetting();
        }
        #endregion
        #region 用来提交的本地函数
        async Task OnSubmit()
        {
            #region 提交逻辑主体
            async Task OnSubmitMain()
            {
                var hasPreviewFilePropertyNatureState = HasPreviewFilePropertyNatureState.Get(typeof(Model));
                if (!hasPreviewFilePropertyNatureState.HasPreviewFile)
                {
                    await NotUploadFile();
                    return;
                }
                var uploadFiles = hasPreviewFilePropertyNatureState.
                    PreviewFilePropertyDescribe.Values.Where(x => x.IsStrict).
                    Select(x => x.GetFiles(formModel)).
                    SelectMany(x => x).
                    OfType<IHasUploadFileClient>().
                    WhereEnableAndNotUpload().ToArray();
                if (uploadFiles.Length is 0)
                    await NotUploadFile();
                else
                    await HasUploadFile();
                #region 包含上传逻辑
                async Task HasUploadFile()
                {
                    #region 获取上传锁的本地函数
                    IDisposable UploadLock()
                    => HasPreviewFilePropertyNatureState.Get(typeof(Model)).HasPreviewFile ?
                    FileUploadNavigationContext.UploadLockSecure() :
                    FastRealize.DisposableEmpty();
                    #endregion
                    try
                    {
                        using var uploadLock = UploadLock();
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
            this.StateHasChanged();
        }
        #endregion
        var renderSubmit = new RenderSubmitInfo<Model>()
        {
            Resetting = Resetting,
            ExistingForms = existingForms,
            FormModel = formModel,
            InUpload = InUpload,
            CanEdit = renderFormViewerPropertyInfo.Any(x => !x.IsReadOnly),
            Delete = (existingForms, Delete) is ({ }, { }) ?
                 async () =>
                 {
                     var isSuccess = await Delete(formModel);
                     if ((isSuccess, Cancellation) is (true, { } cancellation))
                         await cancellation();
                 }
            : null,
            Submit = OnSubmit,
            Cancellation = Cancellation is null ?
                null :
                Cancellation
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
        CopyModel = InitializationModel;
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
