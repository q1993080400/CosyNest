namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 当播放器状态改变时，触发这个委托
/// </summary>
/// <param name="pipe">这个委托的参数是播放器的旧状态，返回值是播放器的可操作新状态</param>
/// <returns></returns>
public delegate Task OnPlayerStateChange(Func<RenderPlayerState, RenderPlayerStateOperational> pipe);