using System.Collections.Generic;
using System.Linq;

namespace System.IOFrancis.FileSystem
{
    /// <summary>
    /// 表示文件的类型（例如音乐文件），不考虑文件是否存在
    /// </summary>
    class FileType : IFileType
    {
        #region 对文件类型的说明
        public string Description { get; }
        #endregion
        #region 受支持的扩展名
        public IEnumerable<string> ExtensionName { get; }
        #endregion
        #region 合并文件类型
        public IFileType Merge(IEnumerable<string> fileType, string description = "")
            => CreateIO.FileType(description, ExtensionName.Union(fileType).ToArray());
        #endregion
        #region 构造函数
        /// <summary>
        /// 用指定的扩展名集合封装文件类型，
        /// 注意：初始化本类型会将文件类型注册
        /// </summary>
        /// <param name="description">对文件类型的说明</param>
        /// <param name="extension">指定的扩展名集合，不带点号</param>
        public FileType(string description, params string[] extension)
        {
            var registered = IFileType.RegisteredFileTypePR;
            ExtensionName = new HashSet<string>(extension);
            this.Description = description;
            foreach (var item in ExtensionName)                     //注册所有扩展名
            {
                if (registered.TryGetValue(item, out var value))
                    value.Add(this);
                else
                    registered.Add(item, new() { this });
            }
        }
        #endregion
    }
}
