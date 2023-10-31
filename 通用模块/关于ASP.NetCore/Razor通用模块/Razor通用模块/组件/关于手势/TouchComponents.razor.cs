using Microsoft.AspNetCore.Components.Web;

using System.MathFrancis;
using System.MathFrancis.Plane;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 本组件能够以更友好的方式捕捉并响应浏览器手势
/// </summary>
public sealed partial class TouchComponents : ComponentBase
{
    #region 组件参数
    #region 子内容
    /// <summary>
    /// 组件的子内容
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment ChildContent { get; set; }
    #endregion
    #region 当触控手势完成后，执行这个委托
    /// <summary>
    /// 当一个触控手势完成后，执行这个异步委托，
    /// 委托的第一个参数是相对于浏览器视口的触控点列表，
    /// 第二个参数是完成的手势
    /// </summary>
    [EditorRequired]
    [Parameter]
    public Func<IReadOnlyList<IPoint>, Gesture, Task> GestureEnd { get; set; }
    #endregion
    #region 触发手势的阈值
    /// <summary>
    /// 指定触发手势的阈值，单位是视口的百分比，
    /// 只有当滑动幅度大于这个阈值时，才会触发手势事件，
    /// 它可以用来防止误触
    /// </summary>
    [Parameter]
    public double Threshold { get; set; } = 0.15;
    #endregion
    #region 参数展开
    /// <summary>
    /// 该参数展开控制父div容器的样式
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? ContainerAttributes { get; set; }
    #endregion
    #endregion
    #region 公开成员
    #region OnTouchMove事件触发时执行的方法
    /// <summary>
    /// 当触发OnTouchMove事件时，执行这个方法
    /// </summary>
    /// <param name="args">事件参数</param>
    public void OnTouchMove(TouchEventArgs args)
    {
        var point = args.ChangedTouches[0];
        Points.Add(CreateMath.Point(point.ClientX, -point.ClientY));
    }
    #endregion
    #region OnTouchEnd事件触发时执行的方法
    /// <summary>
    /// 当触发OnTouchEnd事件时，执行这个方法
    /// </summary>
    /// <param name="args">事件参数</param>
    public async Task OnTouchEnd(TouchEventArgs args)
    {
        if (Points.Count == 0)    //这是为了与Click事件区分开
            return;
        var screen = await JSWindow.Screen;
        var gesture = Gesture.GetGesture(Points, Threshold, screen.Resolution);
        if (gesture != Gesture.TouchByMistake)
            await GestureEnd(Points, gesture);
        Points.Clear();
    }
    #endregion
    #endregion
    #region 内部成员
    #region 手势触点列表
    /// <summary>
    /// 获取手势触点列表，
    /// 触点坐标相对于浏览器视口
    /// </summary>
    private List<IPoint> Points { get; } = [];
    #endregion
    #endregion
}
