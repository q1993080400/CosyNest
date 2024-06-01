namespace System.Underlying;

/// <summary>
/// 这个记录表示命令行的输出
/// </summary>
public sealed record CommandLineOutput
{
    #region 输出文本
    /// <summary>
    /// 返回命令行输出的文本
    /// </summary>
    public required string Text { get; init; }
    #endregion
    #region 输出类型
    /// <summary>
    /// 获取命令行输出类型
    /// </summary>
    public required CommandLineOutputType Type { get; init; }
    #endregion
}
