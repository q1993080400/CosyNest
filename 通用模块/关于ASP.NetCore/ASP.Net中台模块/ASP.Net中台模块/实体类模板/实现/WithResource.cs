using System.ComponentModel.DataAnnotations.Schema;
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
    #region 资源文件夹
    public required string ResourceFolderPath { get; set; }
    #endregion
    #endregion
    #region 非实体成员
    #region 资源文件Uri
    [NotMapped]
    public string[] ResourceFileUris { get; set; } = Array.Empty<string>();
    #endregion
    #region 删除资源文件夹
    public void DeleteResourceFolder()
    {
        var io = CreateIO.IO<IDirectory>(ResourceFolderPath);
        io?.Delete();
    }
    #endregion
    #region 初始化资源文件夹
    public virtual void InitializationResourceFile()
    {
        if (ResourceFolderPath is null || ResourceFileUris.Any())
            return;
        ResourceFileUris = CreateIO.IO<IDirectory>(ResourceFolderPath)?.Son.OfType<IFile>().
            Select(x => x.Path.Op().ToUriPath()).ToArray() ?? Array.Empty<string>();
    }
    #endregion
    #region 解析资源文件的路径
    public IEnumerable<Output> Classify<Output>(Func<IEnumerable<string>, IEnumerable<Output>> pathProtocol)
        => pathProtocol(ResourceFileUris);
    #endregion
    #endregion
}
