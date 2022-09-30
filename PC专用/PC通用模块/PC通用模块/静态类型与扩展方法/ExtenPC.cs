using System.Diagnostics;
using System.IOFrancis.FileSystem;
using System.Runtime.InteropServices;

using IWshRuntimeLibrary;

using Microsoft.VisualBasic.FileIO;

using IFile = System.IOFrancis.FileSystem.IFile;

namespace System;

/// <summary>
/// 所有有关PC的扩展方法全部放在这里
/// </summary>
public static class ExtenPC
{
    #region 关于IO
    #region 把文件或目录移动到回收站
    /// <summary>
    /// 将文件或目录移动到回收站
    /// </summary>
    /// <param name="io">待删除的文件或目录</param>
    public static void DeleteToRecycling(this IIO io)
    {
        const RecycleOption deleteMod = RecycleOption.SendToRecycleBin;
        const UIOption dialogMod = UIOption.OnlyErrorDialogs;
        switch (io)
        {
            case IFile f:
                FileSystem.DeleteFile(f.Path, dialogMod, deleteMod);
                break;
            case IDirectory d:
                FileSystem.DeleteDirectory(d.Path, dialogMod, deleteMod);
                break;
            default:
                throw new TypeUnlawfulException(io, typeof(IFile), typeof(IDirectory));
        }
    }
    #endregion
    #region 创建快捷方式
    /// <summary>
    /// 创建一个文件或目录的快捷方式
    /// </summary>
    /// <param name="io">要创建快捷方式的文件或目录</param>
    /// <param name="pos">快捷方式所在的位置，它指的不是快捷方式的目标，不要加上扩展名lnk</param>
    public static void Shortcut(this IIO io, PathText pos)
    {
        var shell = new WshShell();
        var shortcut = (IWshShortcut)shell.CreateShortcut(pos.Path + ".lnk");
        shortcut.TargetPath = io.Path;
        shortcut.Save();
    }
    #endregion
    #endregion
    #region 最小化或最大化指定进程
    /// <summary>
    /// 将指定的进程最小化或最大化
    /// </summary>
    /// <param name="process">待调整的进程</param>
    /// <param name="isMinimize">如果这个值为<see langword="true"/>，
    /// 则为最小化，否则为最大化</param>
    public static void Adjustment(this Process process, bool isMinimize)
    {
#pragma warning disable IDE1006,CA1806
        #region 基础支持
        [DllImport("user32.dll", EntryPoint = "PostMessage")]
        static extern int PostMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        const int WM_SYSCOMMAND = 0x112;
        const int SC_MINIMIZE = 0xF020;
        const int SC_MAXIMIZE = 0xF030;
        #endregion
        process.Refresh();
        try
        {
            var handle = process.MainWindowHandle;
            if (handle.ToInt64() is not 0)
                PostMessage(handle, WM_SYSCOMMAND,
                    isMinimize ? SC_MINIMIZE : SC_MAXIMIZE, 0);
        }
        catch (InvalidOperationException)
        {

        }
#pragma warning restore
    }
    #endregion
}
