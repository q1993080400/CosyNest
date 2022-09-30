namespace System.Underlying;

/// <summary>
/// 该静态类枚举了操作系统的名称
/// </summary>
public static class OSName
{
    #region WindowsNT
    /// <summary>
    /// WindowsNT操作系统的名称
    /// </summary>
    public const string Windows = "WindowsNT";
    #endregion
    #region Mac
    /// <summary>
    /// Mac操作系统的名称
    /// </summary>
    public const string Mac = "Mac";
    #endregion
    #region IOS
    /// <summary>
    /// IOS操作系统的名称
    /// </summary>
    public const string IOS = "IOS";
    #endregion
    #region 安卓
    /// <summary>
    /// 安卓操作系统的名称
    /// </summary>
    public const string Android = "Android";
    #endregion
    #region 不明操作系统
    /// <summary>
    /// 获取表示不明操作系统的名称
    /// </summary>
    public const string Unknown = "UnknownOperatingSystem";
    #endregion
}
