namespace System.Underlying;

/// <summary>
/// 这个记录是所有硬件信息的基类
/// </summary>
public abstract record DeviceInfo
{
    #region 设备型号
    /// <summary>
    /// 返回设备型号
    /// </summary>
    public required string Model { get; init; }
    #endregion
    #region 设备编号
    /// <summary>
    /// 返回设备编号
    /// </summary>
    public required string Number { get; init; }
    #endregion
}
