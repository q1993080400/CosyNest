using System.Design;
using System.IOFrancis.Bit;
using System.IOFrancis.FileSystem;

namespace System.NetFrancis.FTP;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个FTP客户端
/// </summary>
public interface IFTPClient : IInstruct, IDisposable
{
    #region 上传文件或目录
    /// <summary>
    /// 将文件或目录上传到服务器
    /// </summary>
    /// <param name="io">待上传的文件或目录</param>
    /// <param name="serverDirectory">要将其上传到的服务器目录</param>
    /// <param name="addName">如果这个值为<see langword="true"/>，
    /// 则会在<paramref name="serverDirectory"/>后面自动加上文件或目录的名字</param>
    /// <param name="progress">一个用来报告进度的对象</param>
    /// <param name="cancellation">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    Task Upload(IIO io, string serverDirectory, bool addName = true, IProgress<IOSchedule>? progress = null, CancellationToken cancellation = default);
    #endregion
}
