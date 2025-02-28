namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是<see cref="IElementNumber"/>的实现，
/// 可以为元素编号
/// </summary>
/// <param name="js">JS运行时对象</param>
/// <param name="prefix">编号前缀/></param>
/// <param name="scrollingContextCSS">滚动上下文的CSS样式，
/// 它适用于在粘性滚动的情况下，将元素滚动到正确的位置</param>
sealed class ElementNumber(IJSRuntime js, string prefix, string? scrollingContextCSS) : IElementNumber
{
    #region 获取元素编号
    public string GetElementID(int index)
        => prefix + index;
    #endregion
    #region 跳转到指定元素
    public async Task JumpToElement(int index, bool smooth = true, bool jumpToEnd = true)
    {
        var id = GetElementID(index);
        await js.InvokeVoidAsync("JumpTo", id, smooth, jumpToEnd, scrollingContextCSS);
    }
    #endregion 
}
