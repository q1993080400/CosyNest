using System.Design;
using System.IOFrancis.Bit;
using System.IOFrancis.FileSystem;
using System.SafetyFrancis.Authentication;

using FluentFTP;

namespace System.NetFrancis.FTP;

/// <summary>
/// 这个类型是使用FluentFTP实现的FTP客户端
/// </summary>
sealed class FluentFTPClient : AutoRelease, IFTPClient
{
    #region 公开成员
    #region 上传文件
    public Task Upload(IIO io, string serverDirectory, bool addName = true, IProgress<IOSchedule>? progress = null, CancellationToken cancellation = default)
    {
        throw new NotSupportedException("暂时未实现此API");
        //serverDirectory = addName ? $"{serverDirectory}/{io.NameFull}" : serverDirectory;
        //return io switch
        //{
        //    IFile => Client.UploadFileAsync(io.Path, serverDirectory, createRemoteDir: true, token: cancellation),
        //    IDirectory => Client.UploadDirectoryAsync(io.Path, serverDirectory, token: cancellation),
        //    null => throw new ArgumentNullException(nameof(io)),
        //    _ => throw new NotSupportedException($"{io.GetType()}既不是文件，也不是目录")
        //};
    }
    #endregion
    #endregion
    #region 内部成员
    #region 封装的FTP对象
    /// <summary>
    /// 获取封装的FTP对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    internal IFtpClient Client { get; }
    #endregion
    #region 释放对象
    protected override void DisposeRealize()
        => Client.Dispose();
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="webCredentials">用于连接到服务器的凭据</param>
    public FluentFTPClient(UnsafeWebCredentials webCredentials)
    {
        Client = new FtpClient(webCredentials.Host, webCredentials.ID, webCredentials.Password);
    }
    #endregion
}
