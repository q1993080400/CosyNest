namespace System.DataFrancis;

/// <summary>
/// 这个特性指示了对布尔值数据类型的渲染偏好
/// </summary>
public sealed class RenderPreferenceBoolAttribute : RenderPreferenceAttribute
{
    #region 渲染布尔值时的偏好
    /// <summary>
    /// 在渲染布尔值时的偏好
    /// </summary>
    public FormBoolRender RenderBool { get; init; }
    #endregion
    #region 为True时的说明
    /// <summary>
    /// 获取为<see langword="true"/>时的说明
    /// </summary>
    public string DescribeTrue { get; init; } = "打开";
    #endregion
    #region 为Flase时的说明
    /// <summary>
    /// 获取为<see langword="false"/>时的说明
    /// </summary>
    public string DescribeFlase { get; init; } = "关闭";
    #endregion
    #region 抽象成员实现：返回渲染偏好
    public override RenderPreferenceBool GetRenderPreference()
        => new()
        {
            RenderBool = RenderBool,
            DescribeTrue = DescribeTrue,
            DescribeFlase = DescribeFlase
        };
    #endregion
}
