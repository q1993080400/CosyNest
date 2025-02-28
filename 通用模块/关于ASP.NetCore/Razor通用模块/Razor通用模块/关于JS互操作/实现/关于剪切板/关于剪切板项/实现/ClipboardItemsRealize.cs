namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是<see cref="IClipboardItems"/>的实现，
/// 可以用来封装剪切板数据
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="clipboardItems">所有剪切板数据</param>
sealed class ClipboardItemsRealize(IEnumerable<ClipboardItem> clipboardItems) : IClipboardItems
{
    #region 所有剪切板数据
    /// <summary>
    /// 获取所有剪切板数据
    /// </summary>
    public IReadOnlyCollection<IClipboardItem> ClipboardItems { get; }
        = [.. clipboardItems.Select(x => x.Text is { } text ?
        (IClipboardItem)new ClipboardItemText(x.Size, x.Type, x.Text) :
        new ClipboardItemBinary(x.Size, x.Type, x.Data!,x.ObjectURL))];
    #endregion
}
