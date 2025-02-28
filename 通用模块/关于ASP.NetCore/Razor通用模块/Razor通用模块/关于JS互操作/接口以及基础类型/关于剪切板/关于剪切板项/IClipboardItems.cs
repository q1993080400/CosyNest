namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个剪切板的数据项的集合，
/// 它封装了剪切板中的所有数据
/// </summary>
public interface IClipboardItems
{
    #region 所有剪切板数据
    /// <summary>
    /// 获取所有剪切板数据
    /// </summary>
    IReadOnlyCollection<IClipboardItem> ClipboardItems { get; }
    #endregion
}
