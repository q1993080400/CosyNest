using System.MathFrancis;

namespace System.Media.Play;

/// <summary>
/// 这个静态类枚举媒体的清晰度
/// </summary>
public static class Definition
{
    #region 720P
    /// <summary>
    /// 获取720P的清晰度，
    /// 也就是1280*720
    /// </summary>
    public static ISize<int> P720 { get; } = CreateMath.Size(1280, 720);
    #endregion
    #region 480P
    /// <summary>
    /// 获取480P的清晰度，
    /// 也就是854*480
    /// </summary>
    public static ISize<int> P480 { get; } = CreateMath.Size(854, 480);
    #endregion
}
