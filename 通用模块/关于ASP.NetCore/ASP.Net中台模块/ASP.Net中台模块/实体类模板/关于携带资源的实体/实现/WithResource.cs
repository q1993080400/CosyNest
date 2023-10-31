using System.DataFrancis;
using System.IOFrancis;
using System.IOFrancis.FileSystem;

namespace Microsoft.AspNetCore;

/// <summary>
/// 表示一个携带资源的实体，
/// 它在服务器上拥有一个专属的文件夹用来存放资源文件
/// </summary>
public abstract class WithResource : Entity, IWithResource
{
    #region 实体属性
    #region 路径标识
    /// <summary>
    /// 获取一个路径标识，
    /// 它是用来生成路径的种子
    /// </summary>
    public Guid PathIdentifying { get; set; } = Guid.NewGuid();
    #endregion
    #endregion
    #region 非实体成员
    #region 删除资源文件夹
    public void DeleteResourceFolder()
        => CreateIO.IO<IDirectory>(ResourceFolderPath)?.Delete();
    #endregion
    #region 获取资源文件夹路径
    public abstract string ResourceFolderPath { get; }
    #endregion
    #region 获取资源文件夹所有文件子路径
    /// <summary>
    /// 获取资源文件夹的所有文件子路径
    /// </summary>
    public string[] ResourceFolderSonPath
    {
        get
        {
            var directory = CreateIO.IO<IDirectory>(ResourceFolderPath);
            if (directory is null)
                return [];
            return directory.Son.OfType<IFile>().Select(x => x.Path).ToArray();
        }
    }
    #endregion
    #region 获取资源的Uri
    public virtual string[] ResourceUri
        => CreateIO.IO<IDirectory>(ResourceFolderPath)?.Son.OfType<IFile>().
            Select(x => x.Path.Op().ToUriPath()).ToArray() ?? [];
    #endregion
    #region 解析资源文件夹
    /// <summary>
    /// 解析资源文件夹
    /// </summary>
    /// <typeparam name="Ret">返回值类型</typeparam>
    /// <param name="analysis">用来解析的委托</param>
    /// <returns></returns>
    public Ret Analysis<Ret>(AnalysisFilePathProtocol<IEnumerable<string>, Ret> analysis)
    {
        var paths = CreateIO.IO<IDirectory>(ResourceFolderPath)?.Son.OfType<IFile>().
            Select(x => x.Path).ToArray() ?? [];
        return analysis(paths);
    }
    #endregion
    #endregion
}
