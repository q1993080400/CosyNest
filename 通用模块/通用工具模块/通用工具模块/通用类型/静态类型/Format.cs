namespace System;

/// <summary>
/// 这个静态类为格式化提供帮助
/// </summary>
public static class Format
{
    #region 日期格式化字符串
    #region 带时间
    /// <summary>
    /// 获取日期格式化字符串，
    /// 它包含时间部分
    /// </summary>
    public const string FormatChineseWithTimeText = "yyyy年M月d日H:mm";
    #endregion
    #region 不带时间
    /// <summary>
    /// 获取日期格式化字符串，
    /// 它不包含时间部分
    /// </summary>
    public const string FormatChineseText = "yyyy年M月d日";
    #endregion
    #endregion 
    #region 常见的格式化数字字符串
    /// <summary>
    /// 获取一个常见的格式化数字字符串，
    /// 它将数字格式化为普通人习惯的格式
    /// </summary>
    public const string FormattedNumCommon = "###############0.#########";
    #endregion
}
