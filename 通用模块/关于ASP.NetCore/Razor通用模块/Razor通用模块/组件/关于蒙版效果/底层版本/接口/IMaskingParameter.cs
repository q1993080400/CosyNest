namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个接口可以为蒙版效果提供参数
/// </summary>
public interface IMaskingParameter
{
    #region 占满全屏
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示蒙版效果应该占满全屏
    /// </summary>
    bool IsFullScreen { get; }
    #endregion
}
