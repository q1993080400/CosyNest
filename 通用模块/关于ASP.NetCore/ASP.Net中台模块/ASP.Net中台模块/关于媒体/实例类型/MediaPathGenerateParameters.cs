namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是媒体路径生成的参数
/// </summary>
public sealed record MediaPathGenerateParameters
{
    #region 基路径
    /// <summary>
    /// 获取媒体文件的基路径
    /// </summary>
    public required string Path { get; init; }
    #endregion
    #region 扩展名
    /// <summary>
    /// 获取媒体文件的扩展名，不带点号
    /// </summary>
    public required string Extension { get; init; }
    #endregion
    #region 排序
    /// <summary>
    /// 获取媒体文件的排序，顺序为升序
    /// </summary>
    public required int Sort { get; init; }
    #endregion
}
