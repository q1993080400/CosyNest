namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是<see cref="IClipboardItemText"/>的实现，
/// 可以视为一个封装文本的剪切板项
/// </summary>
/// <param name="Size">数据的大小</param>
/// <param name="Type">数据的MIME类型</param>
/// <param name="Text">封装的文本</param>
sealed record ClipboardItemText(long Size, string Type, string Text) : IClipboardItemText
{

}
