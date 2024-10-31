namespace System;

/// <summary>
/// 这个静态类可以用来创建常用异常
/// </summary>
public static class CreateException
{
    #region 返回NotSupportedException
    #region 由于不支持指定功能所引发
    /// <summary>
    /// 返回一个由于不支持指定的功能所引发的异常
    /// </summary>
    /// <param name="function">对不受支持的功能的说明</param>
    /// <returns></returns>
    public static NotSupportedException NotSupported(string? function = null)
        => new("不支持" + (function ?? "此功能"));
    #endregion
    #endregion
}
