namespace System.Underlying;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个剪切板
/// </summary>
public interface IClipboard
{
    #region 写入剪切板文本
    /// <summary>
    /// 写入剪切板文本，并返回是否写入成功
    /// </summary>
    /// <param name="text">要写入的文本</param>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    Task<bool> WriteText(string? text, CancellationToken cancellationToken = default);
    #endregion
    #region 读取剪切板文本
    /// <summary>
    /// 读取剪切板文本，并返回一个元组，
    /// 它的项分别是读取是否成功，以及读取到的文本
    /// </summary>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    Task<(bool IsSuccess, string? Text)> ReadText(CancellationToken cancellationToken = default);
    #endregion
}
