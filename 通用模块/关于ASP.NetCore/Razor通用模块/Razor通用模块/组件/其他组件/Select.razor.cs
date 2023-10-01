namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件是一个选择器，
/// 它具有选择和未选择两种状态
/// </summary>
/// <typeparam name="Obj">要选择的值的类型</typeparam>
public sealed partial class Select<Obj> : ComponentBase, IContentComponent<RenderFragment<(bool, Obj)>>
{
    #region 说明文档
    /*注意：本组件不适用于以下情况，
      如果出现该情况，建议不要使用本组件，想其他的办法：
      
      #需要在多个选择器之间共享状态，
      本组件被设计为单独持有状态，无法满足这种情况*/
    #endregion
    #region 组件参数
    #region 子内容
    /// <summary>
    /// 获取组件的子内容，
    /// 它的参数分别是是否被选择，以及组件的值
    /// </summary>
    [EditorRequired]
    [Parameter]
    public RenderFragment<(bool, Obj)> ChildContent { get; set; }
    #endregion
    #region 待选择的值
    /// <summary>
    /// 获取或设置要选择的值
    /// </summary>
    [EditorRequired]
    [Parameter]
    public Obj Value { get; set; }
    #endregion
    #region 用来读写选择的委托
    /// <summary>
    /// 这个元组的项分别是获取和设置被选择状态的委托
    /// </summary>
    [Parameter]
    public (Func<Obj, Task<bool>> GetSelectState, Func<Obj, bool, Task> SetSelectState) SelectProperty { get; set; }
    #endregion
    #region 参数展开
    /// <summary>
    /// 这个字典用来接收参数展开
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? InputAttributes { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 缓存被选择状态
    /// <summary>
    /// 这个属性缓存被选择的状态
    /// </summary>
    private bool IsSelect { get; set; }
    #endregion
    #region 选择时触发的事件
    /// <summary>
    /// 当选择时，触发这个事件
    /// </summary>
    /// <returns></returns>
    private async Task OnSelect()
    {
        var (get, set) = SelectProperty;
        var isSelect = await get(Value);
        await set(Value, !isSelect);
        IsSelect = await get(Value);
    }
    #endregion
    #region 重写OnInitializedAsync
    protected override async Task OnInitializedAsync()
    {
        IsSelect = await SelectProperty.GetSelectState(Value);
    }
    #endregion
    #region 重写OnAfterRenderAsync
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        var isSelect = IsSelect;
        IsSelect = await SelectProperty.GetSelectState(Value);
        if (isSelect != IsSelect)
            StateHasChanged();
    }
    #endregion
    #endregion
    #region 构造函数
    public Select()
    {
        SelectProperty = (_ => Task.FromResult(IsSelect), (_, x) =>
        {
            IsSelect = x;
            return Task.CompletedTask;
        }
        );
    }
    #endregion
}

#region 静态辅助类
/// <summary>
/// <see cref="Select{Obj}"/>的静态辅助类，
/// 提供了一些使用这个类型的帮助API
/// </summary>
public static class Select
{
    #region 根据元素是否存在于集合中，确定是否选择，如何选择，选择是否有效
    /// <summary>
    /// 根据元素是否存在于集合中，确定是否选择，如何选择，选择是否有效，
    /// 它的返回值通常被赋值给<see cref="Select{Obj}.SelectProperty"/>属性
    /// </summary>
    /// <typeparam name="Obj">元素类型</typeparam>
    /// <param name="selectCollect">用来枚举被选择元素的集合，
    /// 当元素被选择时，它会被添加到这个集合中，
    /// 反之会被移除出这个集合，同时程序会根据元素是否存在于这个集合中，
    /// 来判断元素是否被选择</param>
    /// <param name="maxSelect">当元素被选择时，如果<paramref name="selectCollect"/>的元素数量超过了这个上限，
    /// 则禁止选择，如果为<see langword="null"/>，表示没有上限</param>
    /// <returns></returns>
    public static (Func<Obj, Task<bool>> GetSelectState, Func<Obj, bool, Task> SetSelectState) FromCollect<Obj>
        (ICollection<Obj> selectCollect, int? maxSelect = null)
        => (x => selectCollect.Contains(x).ToTask(),
        (obj, isSelect) =>
        {
            if ((maxSelect, isSelect) is ({ } max, true) && selectCollect.Count >= max)
                return Task.CompletedTask;
            if (isSelect)
                selectCollect.Add(obj);
            else selectCollect.Remove(obj);
            return Task.CompletedTask;
        }
    );
    #endregion
    #region 根据在本地存储中的键来确定是否选择
    /// <summary>
    /// 根据在本地存储中是否存在指定的键，
    /// 来确定一个选择器是否被选择，
    /// 它的返回值通常被赋值给<see cref="Select{Obj}.SelectProperty"/>属性
    /// </summary>
    /// <typeparam name="Obj">元素类型</typeparam>
    /// <param name="jsWindow">用来获取本地存储的JS运行时对象</param>
    /// <param name="key">用来读写本地存储的键</param>
    /// <returns></returns>
    public static (Func<Obj, Task<bool>> GetSelectState, Func<Obj, bool, Task> SetSelectState) FromLocalStorage<Obj>
        (IJSWindow jsWindow, string key)
        => (async _ => (await jsWindow.LocalStorage.TryGetValueAsync(key)) is (true, "True"),
        async (_, value) =>
        {
            await jsWindow.LocalStorage.IndexAsync.Set(key, value.ToString());
        }
    );
    #endregion
}
#endregion
