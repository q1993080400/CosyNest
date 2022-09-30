using System.SafetyFrancis.Authentication;

namespace System.NetFrancis.Mail;

/// <summary>
/// 这个静态类可以用来帮助创建邮件客户端
/// </summary>
public static class CreateMailClientKit
{
    #region 创建任意邮件客户端
    /// <summary>
    /// 创建任意电子邮件客户端
    /// </summary>
    /// <param name="smtp">指定连接Smtp服务器的方式</param>
    /// <param name="imap">指定连接IMAP服务器的方式</param>
    /// <param name="checkInterval">指定用来检查新邮件的间隔，
    /// 如果为<see langword="null"/>，默认为1分钟</param>
    /// <returns></returns>
    public static IMailClient Client(UnsafeWebCredentials smtp, UnsafeWebCredentials imap, TimeSpan? checkInterval = null)
        => (smtp.Host.Split(':').Length, imap.Host.Split(':').Length) is (2, 2) ?
        new MailClient(smtp, imap, checkInterval) :
        throw new ArgumentException($"参数{nameof(smtp)}和{nameof(imap)}的{nameof(UnsafeWebCredentials.Host)}必须包含端口信息");
    #endregion
    #region 创建QQ邮箱客户端
    /// <summary>
    /// 创建一个QQ邮箱客户端
    /// </summary>
    /// <param name="credentials">用来登录邮件服务器的凭据</param>
    /// <returns></returns>
    /// <inheritdoc cref="Client(UnsafeWebCredentials, UnsafeWebCredentials, TimeSpan?)"/>
    public static IMailClient ClientQQ(UnsafeCredentials credentials, TimeSpan? checkInterval = null)
        => Client
        (new(credentials.ID, credentials.Password, "smtp.qq.com:465", true),
            new(credentials.ID, credentials.Password, "imap.qq.com:993", true), checkInterval);
    #endregion
    #region 创建网易邮箱客户端
    /// <summary>
    /// 创建一个网易邮箱客户端
    /// </summary>
    /// <param name="credentials">用来登录邮件服务器的凭据，用户名不需要加上@163.com</param>
    /// <param name="checkInterval">指定用来检查新邮件的间隔，
    /// 如果为<see langword="null"/>，默认为1分钟</param>
    /// <returns></returns>
    /// <inheritdoc cref="Client(UnsafeWebCredentials, UnsafeWebCredentials, TimeSpan?)"/>
    public static IMailClient Client163(UnsafeCredentials credentials, TimeSpan? checkInterval = null)
        => Client
        (new(credentials.ID, credentials.Password, "smtp.163.com:465", true),
            new(credentials.ID, credentials.Password, "imap.163.com:993", true), checkInterval);
    #endregion
    #region 创建微软邮箱客户端
    /// <summary>
    /// 创建一个微软邮箱客户端
    /// </summary>
    /// <param name="credentials">用来登录邮件服务器的凭据，用户名不需要加上@outlook.com</param>
    /// <param name="checkInterval">指定用来检查新邮件的间隔，
    /// 如果为<see langword="null"/>，默认为1分钟</param>
    /// <returns></returns>
    /// <inheritdoc cref="Client(UnsafeWebCredentials, UnsafeWebCredentials, TimeSpan?)"/>
    public static IMailClient ClientOutlook(UnsafeCredentials credentials, TimeSpan? checkInterval = null)
        => Client
        (new(credentials.ID, credentials.Password, "smtp.office365.com:587", true),
            new(credentials.ID, credentials.Password, "outlook.office365.com:993", true), checkInterval);
    #endregion
}
