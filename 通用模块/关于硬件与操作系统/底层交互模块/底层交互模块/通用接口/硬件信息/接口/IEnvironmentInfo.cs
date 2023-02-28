namespace System.Underlying;

/// <summary>
/// 该接口封装了主机的基本环境信息
/// </summary>
public interface IEnvironmentInfo
{
    #region 操作系统
    /// <summary>
    /// 获取操作系统
    /// </summary>
    OS OS { get; }
    #endregion
    #region 硬件类型
    /// <summary>
    /// 获取硬件类型，如PC，手机等
    /// </summary>
    HardwareType HardwareType { get; }
    #endregion
}
