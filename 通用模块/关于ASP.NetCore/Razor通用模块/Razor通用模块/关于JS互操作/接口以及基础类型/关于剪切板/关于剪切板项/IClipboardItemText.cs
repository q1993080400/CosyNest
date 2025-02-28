namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个剪切板中的文本数据项
/// </summary>
public interface IClipboardItemText : IClipboardItem
{
    #region 数据的文本
    /// <summary>
    /// 获取数据的文本
    /// </summary>
    string Text { get; }
    #endregion
}
