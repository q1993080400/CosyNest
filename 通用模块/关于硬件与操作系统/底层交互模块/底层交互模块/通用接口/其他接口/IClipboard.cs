namespace System.Underlying;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个剪切板
/// </summary>
public interface IClipboard
{
    #region 获取或设置剪切板文本
    /// <summary>
    /// 获取或设置剪切板文本
    /// </summary>
    string? Text { get; set; }
    #endregion
}
