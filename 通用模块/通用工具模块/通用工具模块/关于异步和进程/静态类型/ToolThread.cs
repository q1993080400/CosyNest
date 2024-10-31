using System.Diagnostics;

namespace System.Threading;

/// <summary>
/// 这个静态类是有关线程和进程的工具类
/// </summary>
public static class ToolThread
{
    #region 打开进程
    /// <summary>
    /// 打开一个进程，<see cref="Process.Start"/>方法不同，
    /// 它可以打开除了exe以外的文件，只要该文件在操作系统中关联了打开方式
    /// </summary>
    /// <param name="path">要打开的文件或目录的路径</param>
    /// <returns></returns>
    public static Process? Open(string path)
        => Process.Start(new ProcessStartInfo()
        {
            FileName = path,
            UseShellExecute = true
        });
    #endregion
}
