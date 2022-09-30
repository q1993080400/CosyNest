namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 该组件是对<see cref="ComponentBase"/>的扩展，
/// 拥有更强大的功能，包括呈现追踪等
/// </summary>
public abstract class Component : ComponentBase
{
    #region 说明文档
    /*问：为什么需要本类型？
      答：这是因为Blazor组件的渲染过程非常复杂，
      尤其是顺序难以控制，在OnAfterRender方法中，
      经常不知道OnInitializedAsync和OnParametersSetAsync是否已经调用，
      以及它们返回的Task是否已经等待完毕，这导致初始化很容易出现问题，
      因此作者声明了本类型，当本组件被渲染时，它知道自己在渲染过程中处于哪个阶段

      但是，使用本类需注意以下细节：
      #渲染阶段和渲染次数是两个无关的概念，
      这是因为在调用OnInitializedAsync和OnParametersSetAsync方法时，
      如果它们返回了直接完成的Task，则对应的渲染次数会被跳过

      #如果在OnAfterRenderRealize中调用StateHasChanged方法，
      则此时的state参数不会改变，这是因为在这个递归调用中，
      堆栈下层的OnAfterRenderAsync方法还没有执行完，
      但是renderCount参数会发生变化，这是因为对该变量的自增发生在OnAfterRenderAsync方法的开头*/
    #endregion
    #region OnAfterRenderAsync事件（组件参数）
    /// <summary>
    /// 当<see cref="OnAfterRenderAsyncRealize(int)"/>执行完毕后，
    /// 执行这个事件，事件的第一个参数是当前渲染状态，
    /// 第二个参数是完全渲染后，执行渲染的次数，从1开始，
    /// 为0表示尚未完全渲染
    /// </summary>
    [Parameter]
    public Func<RenderState, int, Task>? OnAfterRenderAsyncEvent { get; set; }

    /*问：为什么明明有OnAfterRenderAsync方法，还需要这个事件？
      答：因为它可以从外部注入代码，避免声明一个新的类型重写OnAfterRenderAsync
     
      问：那么既然如此，为什么OnParametersSetAsync和OnInitializedAsync没有对应事件？
      答：因为OnAfterRenderAsync*/
    #endregion
    #region 渲染阶段
    /// <summary>
    /// 返回本组件的渲染阶段
    /// </summary>
    protected RenderState State { get; private set; } = RenderState.CreateRender3;

    /*问：为什么渲染阶段的初始值是CreateRender3？
      答：这是因为在未重写任何方法的情况下，第一次调用OnAfterRenderAsync，
      本来就是处于CreateRender3状态*/
    #endregion
    #region 重写渲染阶段方法
    #region OnInitializedAsync阶段
    #region 重写方法
    protected sealed override Task OnInitializedAsync()
    {
        var task = OnInitializedAsyncRealize();
        State = task.IsCompleted ? State : RenderState.CreateRender1;
        return task;
    }
    #endregion
    #region 模板方法
    /// <summary>
    /// 本方法是<see cref="OnInitializedAsync"/>的模板方法，
    /// 如果希望重写该方法的逻辑，可以重写本方法
    /// </summary>
    /// <returns></returns>
    protected virtual Task OnInitializedAsyncRealize()
        => Task.CompletedTask;
    #endregion
    #endregion
    #region OnParametersSetAsync阶段
    #region 正式方法
    protected sealed override Task OnParametersSetAsync()
    {
        var task = OnParametersSetAsyncRealize();
        State = task.IsCompleted ? RenderState.CreateRender3 : RenderState.CreateRender2;
        return task;
    }
    #endregion
    #region 模板方法
    /// <summary>
    /// 本方法是<see cref="OnParametersSetAsync"/>的模板方法，
    /// 如果希望重写该方法的逻辑，可以重写本方法
    /// </summary>
    /// <returns></returns>
    protected virtual Task OnParametersSetAsyncRealize()
        => Task.CompletedTask;
    #endregion
    #endregion
    #region OnAfterRenderAsync阶段
    #region 渲染次数
    /// <summary>
    /// 指示目前的渲染是完全渲染后的第几次渲染，从1开始，
    /// 如果尚未完全渲染，则为0
    /// </summary>
    protected int RenderCount { get; private set; }
    #endregion
    #region 重写方法
    protected sealed override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (State >= RenderState.CreateRender3)
            RenderCount++;
        await OnAfterRenderAsyncRealize();
        if (OnAfterRenderAsyncEvent is { } e)
            await e(State, RenderCount);
        State = State == RenderState.CreateRender2 ? RenderState.CreateRender3 : State;
    }
    #endregion
    #region 模板方法
    /// <summary>
    /// 本类型是<see cref="OnAfterRenderAsync(bool)"/>的模板方法，
    /// 如果希望重写该方法的逻辑，可以重写本方法，
    /// 本方法可以通过<see cref="State"/>和<see cref="RenderCount"/>来获取关于呈现阶段的信息
    /// </summary>
    /// <returns></returns>
    protected virtual Task OnAfterRenderAsyncRealize()
        => Task.CompletedTask;
    #endregion
    #endregion
    #endregion
}
