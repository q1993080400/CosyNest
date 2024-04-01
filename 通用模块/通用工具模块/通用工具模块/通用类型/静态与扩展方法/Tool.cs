namespace System;

/// <summary>
/// 通用工具类
/// </summary>
public static class Tool
{
    #region 如果为null，则写入，否则抛出异常
    /// <summary>
    /// 如果一个对象为<see langword="null"/>，
    /// 则写入并返回写入后的对象，否则引发一个异常
    /// </summary>
    /// <typeparam name="Obj">对象的类型</typeparam>
    /// <param name="obj">要写入的对象</param>
    /// <param name="set">要写入的新值</param>
    /// <returns></returns>
    public static Obj? IfNotNullSet<Obj>(ref Obj? obj, Obj? set)
        => obj is null ?
        obj = set :
        throw new NotSupportedException("要写入的对象不为null，不允许再次写入");
    #endregion
    #region 常见的格式化数字字符串
    /// <summary>
    /// 获取一个常见的格式化数字字符串，
    /// 它将数字格式化为普通人习惯的格式
    /// </summary>
    public const string FormattedNumCommon = "###############0.#########";
    #endregion
}
