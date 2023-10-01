using System.MathFrancis;
using System.MathFrancis.Plane;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录是触控手势的类别
/// </summary>
public sealed record Gesture
{
    #region 静态成员
    #region 根据触控点获取手势
    /// <summary>
    /// 根据触控点列表，获取手势
    /// </summary>
    /// <param name="points">触控点列表</param>
    /// <param name="threshold">触发手势的阈值，单位是视口的百分比，
    /// 如果滑动幅度小于阈值，视为误触</param>
    /// <param name="viewport">浏览器视口大小，它用来计算误触，
    /// 如果为<see langword="null"/>，则不会计算误触</param>
    /// <returns></returns>
    public static Gesture GetGesture(IList<IPoint> points, double threshold = 0.15, ISizePixel? viewport = null)
    {
        var (h, v) = viewport ?? CreateMath.SizePixel(0, 0);
        var (firstR, firstT) = points[0];
        var (lastR, lastT) = points[^1];
        var isHorizontal = ToolArithmetic.Abs(lastR - firstR) > ToolArithmetic.Abs(lastT - firstT);
        var distance = isHorizontal ? lastR - firstR : firstT - lastT;
        var distanceAbs = ToolArithmetic.Abs(distance);
        if (isHorizontal ? distanceAbs <= h * threshold : distanceAbs <= v * threshold)     //检测误触
            return TouchByMistake;
        var isRightOrDown = distance > 0;
        return (isHorizontal, isRightOrDown) switch
        {
            (true, true) => SwipeRight,
            (true, false) => SwipeLeft,
            (false, true) => SwipeDown,
            (false, false) => SwipeUp
        };
    }
    #endregion
    #region 预设手势
    #region 左滑
    /// <summary>
    /// 获取表示左滑的手势
    /// </summary>
    public static Gesture SwipeLeft { get; } = new()
    {
        Description = "左滑"
    };
    #endregion
    #region 右滑
    /// <summary>
    /// 获取表示右滑的手势
    /// </summary>
    public static Gesture SwipeRight { get; } = new()
    {
        Description = "右滑"
    };
    #endregion
    #region 上滑
    /// <summary>
    /// 获取表示上滑的手势
    /// </summary>
    public static Gesture SwipeUp { get; } = new()
    {
        Description = "上滑"
    };
    #endregion
    #region 下滑
    /// <summary>
    /// 获取表示下滑的手势
    /// </summary>
    public static Gesture SwipeDown { get; } = new()
    {
        Description = "下滑"
    };
    #endregion
    #region 误触
    /// <summary>
    /// 获取表示误触的手势，
    /// 它一般由于滑动幅度太小而引起
    /// </summary>
    public static Gesture TouchByMistake { get; } = new()
    {
        Description = "误触"
    };
    #endregion
    #endregion
    #endregion
    #region 手势说明
    /// <summary>
    /// 对触控手势的描述
    /// </summary>
    public string Description { get; init; }
    #endregion
    #region 重写ToString
    public override string ToString()
        => Description;
    #endregion
}
