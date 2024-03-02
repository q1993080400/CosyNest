namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是用来渲染<see cref="Player"/>的参数
/// </summary>
public sealed record RenderPlayerInfo
{
    #region 组件的状态
    /// <summary>
    /// 获取组件的状态
    /// </summary>
    public required RenderPlayerState RenderPlayerState { get; init; }
    #endregion
    #region 组件自身
    /// <summary>
    /// 获取<see cref="Player"/>组件自身，
    /// 它可以作为刷新目标，警告：
    /// 在为<see cref="Player"/>的子内容绑定事件时，需要特别注意，
    /// 尽量刷新它的子内容，而不是刷新它的父组件，否则会导致Bug
    /// </summary>
    public required IHandleEvent PlayerComponent { get; init; }
    #endregion
    #region 当组件需要刷新时的委托
    /// <summary>
    /// 如果组件由于JS或浏览器的行为，需要刷新，
    /// 而且C#代码不知道这个过程，请调用本委托，手动刷新
    /// </summary>
    public required Func<Task> OnRefresh { get; init; }
    #endregion
    #region 切换播放器播放/暂停状态的委托
    /// <summary>
    /// 调用这个委托可以切换播放器的播放/暂停状态，
    /// 注意：切换到播放状态，不代表能够成功播放
    /// </summary>
    public required Func<Task> OnSwitchPlayerStatus { get; init; }
    #endregion
    #region 状态改变时触发的事件
    /// <summary>
    /// 当状态改变，例如调整音量，进度时，必须调用这个委托，
    /// 它传入一个委托，该委托传入修改前的状态，返回修改后的状态，
    /// 程序会根据修改后的状态重新呈现组件
    /// </summary>
    public required OnPlayerStateChange OnStateChange { get; init; }

    /*问：本类型通过纯函数来控制组件的状态，
      这在UI开发中似乎是很少见的，为什么要这么设计？
      答：因为这个组件比较特殊，它需要通过JS获取状态，
      用C#对状态进行处理，然后再通过JS把状态写回Web，
      这个过程是很复杂的，如果使用可变状态的话，会很难控制，
      这种行为是戴着镣铐跳舞，它很别扭，但是已经是最优设计*/
    #endregion
}
