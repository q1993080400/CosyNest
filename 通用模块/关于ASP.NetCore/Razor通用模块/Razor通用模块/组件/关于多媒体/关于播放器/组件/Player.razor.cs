namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件是一个无外观的播放器，
/// 它可以用来播放视频或音频
/// </summary>
public sealed partial class Player : ComponentBase, IContentComponent<RenderFragment<RenderPlayerInfo>>
{
    #region 说明文档
    /*说明文档
      这个组件的设计目标如下：

      1.完全无外观，每一个细节都可以自行定制
    
      2.尽量避免显式使用JS进行交互，
      绝大多数情况下使用C#就足以控制它
    
      3.同时兼容音频和视频*/
    #endregion
    #region 组件参数
    #region 组件的初始状态
    /// <summary>
    /// 获取播放器的初始状态，
    /// 它仅用于第一次渲染
    /// </summary>
    [Parameter]
    [EditorRequired]
    public Func<Task<RenderPlayerStateOperational>> InitialState { get; set; }
    #endregion
    #region 子内容
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderPlayerInfo> ChildContent { get; set; }
    #endregion
    #region 仅显式改变状态
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则只有组件内部触发的事件可以改变状态，
    /// 传入组件参数不会改变状态，
    /// 它可以在特殊情况下保持组件的状态不变
    /// </summary>
    [Parameter]
    public bool OnlyExplicitlyChangingState { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 渲染参数
    /// <summary>
    /// 获取播放器的渲染参数
    /// </summary>
    private RenderPlayerInfo RenderPlayerInfo { get; set; }
    #endregion
    #region 播放器ID
    /// <summary>
    /// 获取播放器的ID，
    /// 它必须被赋值给指定的video或audio标签
    /// </summary>
    private string PlayerID { get; } = CreateASP.JSObjectName();
    #endregion
    #region 错误文本
    /// <summary>
    /// 返回一个错误文本，
    /// 它提示由于没有将<see cref="PlayerID"/>赋值给标签所导致的异常
    /// </summary>
    private string Message
        => $"没有找到ID为{PlayerID}的video或audio元素，你是不是忘记了把{nameof(PlayerID)}赋值给标签？";
    #endregion
    #region 状态已改变
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 指示状态已经改变，需要将状态通过JS写回Web
    /// </summary>
    private bool StatusChanged { get; set; }
    #endregion
    #region 重写SetParametersAsync方法
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var extension = parameters.ToDictionary().ToDictionary();
        var initialState = parameters.GetValueOrDefault<Func<Task<RenderPlayerStateOperational>>>(nameof(InitialState)) ??
            throw new NullReferenceException($"{nameof(InitialState)}参数必须显式指定，且不能为null");
        var onlyExplicitlyChangingState = parameters.GetValueOrDefault<bool>(nameof(OnlyExplicitlyChangingState));
        #region 获取播放器状态的本地函数
        async Task<RenderPlayerState> GetState()
           => await JSWindow.InvokeAsync<RenderPlayerState>("GetPlayerState", PlayerID) ??
               throw new NullReferenceException(Message);
        #endregion
        var statusChanged = RenderPlayerInfo is null || !onlyExplicitlyChangingState;
        RenderPlayerInfo = statusChanged ? new()
        {
            RenderPlayerState = new()
            {
                RenderPlayerStateOperational = await initialState(),
                PlayerID = PlayerID
            },
            OnSwitchPlayerStatus = async () =>
            {
                var state = await JSWindow.InvokeAsync<RenderPlayerState>("SwitchPlayerStatus", PlayerID);
                RenderPlayerInfo = RenderPlayerInfo! with
                {
                    RenderPlayerState = state
                };
            },
            OnRefresh = async () =>
            {
                var state = await GetState();
                RenderPlayerInfo = RenderPlayerInfo! with
                {
                    RenderPlayerState = state
                };
            },
            OnStateChange = async fun =>
            {
                var state = await GetState();
                var changeStateOperational = fun(state);
                RenderPlayerInfo = RenderPlayerInfo! with
                {
                    RenderPlayerState = state with
                    {
                        RenderPlayerStateOperational = changeStateOperational
                    }
                };
                StatusChanged = true;
            },
            PlayerComponent = this
        } : RenderPlayerInfo!;
        StatusChanged = statusChanged;
        await base.SetParametersAsync(ParameterView.FromDictionary(extension));
    }
    #endregion
    #region 重写OnAfterRenderAsync
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!StatusChanged)
            return;
        var state = RenderPlayerInfo.RenderPlayerState.RenderPlayerStateOperational;
        var success = await JSWindow.InvokeAsync<bool>("SetPlayerState", PlayerID, state);
        if (!success)
            throw new NullReferenceException(Message);
        StatusChanged = false;
    }
    #endregion
    #endregion
}
