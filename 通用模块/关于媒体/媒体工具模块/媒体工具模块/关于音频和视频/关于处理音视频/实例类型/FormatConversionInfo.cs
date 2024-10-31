using System.MathFrancis;

namespace System.Media.Play;

/// <summary>
/// 这个记录是<see cref="IVideoProcessing.FormatConversion(FormatConversionInfo)"/>方法的参数
/// </summary>
public sealed record class FormatConversionInfo
{
    #region 要转换的音视频路径
    /// <summary>
    /// 获取要转换的音视频的路径
    /// </summary>
    public required string MediaPath { get; init; }
    #endregion
    #region 转换的目标路径
    /// <summary>
    /// 获取要转换的目标路径
    /// </summary>
    public required string TargetPath { get; init; }
    #endregion
    #region 选择编码时是否重视兼容
    /// <summary>
    /// 指定转换过程中选择编码的策略，
    /// 如果为<see langword="true"/>，则注重兼容，
    /// 如果为<see langword="false"/>，则倾向于利用更先进，但是受支持更少的格式
    /// </summary>
    public bool EmphasizeCompatibility { get; init; } = true;
    #endregion
    #region 用来报告进度的委托
    /// <summary>
    /// 一个用于报告进度的委托，它的参数就是当前进度
    /// </summary>
    public Func<decimal, Task>? ReportProgress { get; init; }
    #endregion
    #region 用于取消异步操作的令牌
    /// <summary>
    /// 一个用于取消异步操作的令牌
    /// </summary>
    public CancellationToken CancellationToken { get; init; }
    #endregion
    #region 转换后的最高清晰度
    /// <summary>
    /// 获取转换后的最高清晰度，
    /// 如果为<see langword="null"/>，
    /// 则不加以限制，它的横纵比并不重要，
    /// 最后计算大小的时候，以像素总数为准
    /// </summary>
    public ISize<int>? MaxDefinition { get; init; }
    #endregion
}
