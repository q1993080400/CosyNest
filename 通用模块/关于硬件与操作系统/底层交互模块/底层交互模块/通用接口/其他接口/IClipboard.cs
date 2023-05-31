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
    Task<bool> SetText(string? text, CancellationToken cancellationToken = default);
    #endregion
}
