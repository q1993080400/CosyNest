using System.IO;
using System.IOFrancis.FileSystem;

namespace System.IOFrancis
{
    /// <summary>
    /// 表示在进行IO操作时引发的异常
    /// </summary>
    public class ExceptionIO : IOException
    {
        #region 获取和检查异常的静态方法
        #region 错误原因：源文件或目录不存在
        #region 返回异常
        /// <summary>
        /// 获取一个异常预设值，错误原因是源文件不存在，或路径不合法
        /// </summary>
        /// <param name="path">发生异常的路径</param>
        /// <returns></returns>
        public static ExceptionIO BecauseExist(string path)
            => new(path, "源文件或目录不存在，或者路径不合法");
        #endregion
        #region 抛出异常
        /// <summary>
        /// 检查一个路径，如果它不存在，则抛出异常
        /// </summary>
        /// <param name="path">要检查的路径</param>
        /// <param name="checkMod">如果这个值为<see langword="true"/>，代表检查文件是否存在，
        /// 如果这个值为<see langword="false"/>，代表检查目录是否存在，
        /// 如果这个值为<see langword="null"/>，代表上述两者皆可</param>
        public static void CheckExist(string path, bool? checkMod = null)
        {
            if (!ToolPath.CheckPathExist(path, checkMod))
                throw BecauseExist(path);
        }
        #endregion
        #endregion
        #region 错误原因：路径文本不合法
        #region 返回异常
        /// <summary>
        /// 获取一个异常预设值，错误原因是路径不合法
        /// </summary>
        /// <param name="path">发生错误的路径</param>
        /// <returns></returns>
        public static ExceptionIO BecauseNotLegal(string path)
            => new(path, "该路径不是合法路径");
        #endregion
        #region 抛出异常
        /// <summary>
        /// 检查一个路径文本，如果它不合法，则抛出一个异常
        /// </summary>
        /// <param name="path">发生错误的路径</param>
        public static void CheckNotLegal(string path)
        {
            if (ToolPath.GetPathState(path) is PathState.NotLegal)
                throw BecauseNotLegal(path);
        }
        #endregion
        #endregion
        #region 错误原因：文件不是指定的类型
        #region 获取异常
        /// <summary>
        /// 获取一个异常预设值，错误原因是文件类型不受支持
        /// </summary>
        /// <param name="path">发生异常的路径</param>
        /// <param name="fileType">受支持的文件类型</param>
        /// <returns></returns>
        public static ExceptionIO BecauseFileType(string path, IFileType fileType)
            => new(path, $"{IO.Path.GetExtension(path).TrimStart('.')}文件不是受支持的类型，" +
                $"受支持的类型包括：{fileType.ExtensionName.Join("，")}");
        #endregion
        #region 抛出异常
        /// <summary>
        /// 检查一个路径，如果是目录，
        /// 或者它不与指定的文件类型兼容，则抛出一个异常
        /// </summary>
        /// <param name="path">要检查的路径</param>
        /// <param name="supported">受支持的文件类型</param>
        public static void CheckFileType(string path, IFileType supported)
        {
            if (!supported.IsCompatible(path))
                throw BecauseFileType(path, supported);
        }
        #endregion
        #endregion
        #region 错误原因：路径已被占用
        /// <summary>
        /// 获取一个异常预设值，错误原因是路径已被占用
        /// </summary>
        /// <param name="path">被占用的路径</param>
        /// <param name="another">指示路径被谁占用了，如果不填，在错误描述中不会出现</param>
        /// <returns></returns>
        public static ExceptionIO BecauseOccupied(string path, string? another = null)
            => new(path, $"该路径已被{another}占用");
        #endregion
        #endregion
        #region 发生异常的路径
        /// <summary>
        /// 获取发生异常的路径
        /// </summary>
        public string Path { get; }
        #endregion
        #region 构造函数
        /// <summary>
        /// 用指定的路径和异常原因初始化IO异常
        /// </summary>
        /// <param name="path">发生异常的路径</param>
        /// <param name="message">发生异常的原因</param>
        public ExceptionIO(string path, string message)
            : base(message)
        {
            this.Path = path;
        }
        #endregion
    }
}
