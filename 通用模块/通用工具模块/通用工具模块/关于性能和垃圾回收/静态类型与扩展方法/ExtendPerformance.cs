namespace System;

/// <summary>
/// 关于性能的扩展方法全部放在这个类中，通常无需专门调用
/// </summary>
public static class ExtendPerformance
{
    #region 关于IDisposable
    #region 批量释放IDisposable
    /// <summary>
    /// 批量释放<see cref="IDisposable"/>
    /// </summary>
    /// <param name="collections">待释放的<see cref="IDisposable"/></param>
    public static void DisposableAll(this IEnumerable<IDisposable> collections)
        => collections.ForEach(x => x.Dispose());
    #endregion
    #endregion
}
