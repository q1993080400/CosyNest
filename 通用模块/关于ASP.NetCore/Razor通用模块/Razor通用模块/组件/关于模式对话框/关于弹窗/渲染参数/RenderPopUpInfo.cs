namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是渲染<see cref="PopUp"/>的参数
/// </summary>
public sealed record RenderPopUpInfo
{
    #region 标题
    /// <summary>
    /// 获取标题
    /// </summary>
    public required string Title { get; init; }
    #endregion
    #region 用来取消弹窗的委托
    /// <summary>
    /// 获取用来取消弹窗的委托
    /// </summary>
    public required EventCallback Cancellation { get; init; }
    #endregion
    #region 包装其他逻辑与取消委托
    /// <summary>
    /// 这个高阶函数返回一个函数，
    /// 它执行其他方法，然后取消这个弹窗
    /// </summary>
    /// <param name="func">取消弹窗之前执行的委托</param>
    /// <returns></returns>
    public Func<Task> ThenCancellation(Func<Task> func)
        => async () =>
        {
            await func();
            await Cancellation.InvokeAsync();
        };
    #endregion
}
