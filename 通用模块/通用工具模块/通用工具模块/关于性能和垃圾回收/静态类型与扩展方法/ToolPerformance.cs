using System.Diagnostics;

namespace System.Performance;

/// <summary>
/// 关于性能的工具类
/// </summary>
public static class ToolPerformance
{
    #region 关于垃圾回收
    #region 强制进行垃圾回收
    /// <summary>
    /// 强制挂起当前线程，并且进行一次垃圾回收，警告：
    /// 本方法对性能有严重影响，仅用于测试
    /// </summary>
    public static void StartGC()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
    #endregion
    #endregion
    #region 关于弱事件
    #region 向弱事件中注册委托
    /// <summary>
    /// 向一个弱事件中注册委托，
    /// 如果这个对象为<see langword="null"/>，
    /// 则将会其实例化一个新<see cref="WeakDelegate{Del}"/>
    /// </summary>
    /// <typeparam name="Del">弱事件封装的委托类型</typeparam>
    /// <param name="weak">要添加委托的弱事件</param>
    /// <param name="delegate">要添加的委托</param>
    public static void AddWeakDel<Del>(ref WeakDelegate<Del>? weak, Del @delegate)
        where Del : Delegate
        => (weak ??= new WeakDelegate<Del>()).Add(@delegate);
    #endregion
    #endregion
    #region 计算程序运行所需时间
    /// <summary>
    /// 测量程序运行所花费的时间
    /// </summary>
    /// <param name="code">函数会执行这个委托，然后计算所花费的时间</param>
    /// <returns>执行<paramref name="code"/>所花费的时间</returns>
    public static TimeSpan Timing(Action code)
    {
        var clock = new Stopwatch();
        clock.Start();
        code();
        clock.Stop();
        return clock.Elapsed;
    }
    #endregion
}
