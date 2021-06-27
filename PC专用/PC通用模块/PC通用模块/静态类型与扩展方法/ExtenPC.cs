using System.IOFrancis.FileSystem;

using Microsoft.VisualBasic.FileIO;

namespace System
{
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
                    throw new ExceptionTypeUnlawful(io, typeof(IFile), typeof(IDirectory));
            }
        }
        #endregion
        #endregion
    }
}
