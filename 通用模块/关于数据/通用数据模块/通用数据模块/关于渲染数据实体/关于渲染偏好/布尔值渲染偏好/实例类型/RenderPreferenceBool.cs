namespace System.DataFrancis;

/// <summary>
/// 这个记录指示对布尔值数据类型的渲染偏好
/// </summary>
public sealed record RenderPreferenceBool : RenderPreference, ICreate<RenderPreferenceBool>
{
    #region 静态抽象成员实现：创建对象
    public static RenderPreferenceBool Create()
        => new()
        {
            RenderBool = default,
            DescribeTrue = "打开",
            DescribeFlase = "关闭"
        };
    #endregion
    #region 渲染布尔值时的偏好
    /// <summary>
    /// 在渲染布尔值时的偏好
    /// </summary>
    public required FormBoolRender RenderBool { get; init; }
    #endregion
    #region 为True时的说明
    /// <summary>
    /// 获取为<see langword="true"/>时的说明
    /// </summary>
    public required string DescribeTrue { get; init; } = "打开";
    #endregion
    #region 为Flase时的说明
    /// <summary>
    /// 获取为<see langword="false"/>时的说明
    /// </summary>
    public required string DescribeFlase { get; init; } = "关闭";
    #endregion
    #region 抽象实现：返回值的文本
    public override string? RenderToText(object value)
        => value switch
        {
            true => DescribeTrue,
            false => DescribeFlase,
            var v => throw new NotSupportedException($"无法按照渲染偏好返回{v}的文本，它只支持布尔值")
        };
    #endregion
}
