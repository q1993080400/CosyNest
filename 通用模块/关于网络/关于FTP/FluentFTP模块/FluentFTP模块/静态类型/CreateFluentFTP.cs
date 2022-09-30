using System.SafetyFrancis.Authentication;

namespace System.NetFrancis.FTP;

/// <summary>
/// 这个静态类可以用来创建通过FluentFTP实现的FTP客户端
/// </summary>
public static class CreateFluentFTP
{
    #region 创建FTP客户端
    /// <summary>
    /// 创建一个通过FluentFTP实现的FTP客户端，
    /// 当它返回时，已经和服务器建立好了连接，可以直接使用
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="FluentFTPClient(UnsafeWebCredentials)"/>
    public static async Task<IFTPClient> FTP(UnsafeWebCredentials webCredentials)
    {
        throw new NotSupportedException("暂未实现此API");
        //var ftp = new FluentFTPClient(webCredentials);
        //await ftp.Client.ConnectAsync();
        //return ftp;
    }

    /*警告：不要使用AutoConnectAsync方法进行连接，
      它的原理是尝试所有可能的连接属性组合，这非常浪费性能，
      而且不稳定，很多时候使用AutoConnectAsync连接不成功，但是使用ConnectAsync可以连接成功*/
    #endregion
}
