using System.Linq;

namespace System.IOFrancis.FileSystem
{
    /// <summary>
    /// 所有实现这个接口的类型，
    /// 都可以视为一个目录
    /// </summary>
    public interface IDirectory : IIO
    {
        #region 清除目录
        /// <summary>
        /// 清除目录中的所有子文件和目录，
        /// 但是不将目录本身删除
        /// </summary>
        void Clear()
            => Son.ForEach(x => x.Delete());
        #endregion
        #region 有关创建文件或目录
        #region 说明文档
        /*实现这些API请遵循以下规范：
          #如果新文件或目录的名称已经存在，
          不要引发异常，而是自动将名称重命名*/
        #endregion
        #region 在目录下创建目录
        /// <summary>
        /// 在目录下创建新目录
        /// </summary>
        /// <param name="name">新目录的名称，
        /// 如果为<see langword="null"/>，则给予一个不重复的随机名称，
        /// 如果不为<see langword="null"/>但是名称重复，则自动对名称进行重命名</param>
        /// <returns>新创建的目录</returns>
        IDirectory CreateDirectory(string? name = null);
        #endregion
        #region 在目录下创建文件
        /// <summary>
        /// 在目录下创建新文件
        /// </summary>
        /// <param name="name">新文件的名称，
        /// 如果为<see langword="null"/>，则给予一个不重复的随机名称，
        /// 如果不为<see langword="null"/>但是名称重复，则自动对名称进行重命名</param>
        /// <param name="extension">新文件的扩展名</param>
        /// <returns>新创建的文件</returns>
        IFile CreateFile(string? name = null, string extension = "");
        #endregion
        #endregion
        #region 搜索文件或目录
        /// <summary>
        /// 从目录下搜索具有指定名称的文件或目录
        /// </summary>
        /// <typeparam name="IO">要搜索的文件或目录的类型</typeparam>
        /// <param name="name">要搜索的文件或目录全名</param>
        /// <param name="isRecursive">如果这个值为<see langword="true"/>，
        /// 则执行递归搜索，否则只搜索直接子节点</param>
        /// <returns>搜索到的文件或目录，如果没有找到，则返回<see langword="null"/></returns>
        IO? Find<IO>(string name, bool isRecursive = false)
            where IO : IIO
            => (isRecursive ? SonAll : Son).OfType<IO>().FirstOrDefault(x => x.NameFull == name);
        #endregion
    }
}
