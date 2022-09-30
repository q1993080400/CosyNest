namespace System.Underlying;

/// <summary>
/// 该静态类枚举了硬件类型的名称
/// </summary>
public static class HardwareTypeName
{
    #region PC
    /// <summary>
    /// 获取硬件类型PC的名称
    /// </summary>
    public const string PC = "PC";
    #endregion
    #region 手机
    /// <summary>
    /// 获取硬件类型手机的名称
    /// </summary>
    public const string Phone = "Phone";
    #endregion
    #region 不明硬件类型
    /// <summary>
    /// 获取表示不明硬件类型的名称
    /// </summary>
    public const string Unknown = "UnknownHardwareType";
    #endregion
}
