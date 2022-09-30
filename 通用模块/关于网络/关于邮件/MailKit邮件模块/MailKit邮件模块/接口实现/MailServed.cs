using System.IOFrancis.Bit;

using MailKit;

using MimeKit;

namespace System.NetFrancis.Mail;

/// <summary>
/// 这个类型是使用MailKit实现的电子邮件
/// </summary>
sealed class MailServed : IMailServed
{
    #region 公开成员
    #region 发件人
    public string From
        => PackMessage.From.First().
        To<MailboxAddress>().Address;
    #endregion
    #region 标题
    public string Title
         => PackMessage.Subject;
    #endregion
    #region 正文
    public (bool IsText, string Body) Body
    {
        get
        {
            var text = PackMessage.TextBody;
            return text == null ?
                (false, PackMessage.HtmlBody) :
                (true, text);
        }
    }
    #endregion
    #region 附件
    public IEnumerable<IBitRead> Attachment
         => PackMessage.Attachments.Select(x =>
         {
             var att = x.To<MimePart>();
             return att.Content.Open().ToBitPipe(att.FileName).Read;
         });
    #endregion
    #region 收件时间
    public DateTimeOffset Data
        => PackMessage.Date;
    #endregion
    #region 重写ToString
    public override string ToString()
        => Title;
    #endregion
    #endregion 
    #region 内部成员
    #region 封装的邮件
    /// <summary>
    /// 获取封装的邮件，本对象的功能就是通过它实现的
    /// </summary>
    private MimeMessage PackMessage { get; }
    #endregion
    #region 封装的邮件客户端
    /// <summary>
    /// 获取邮件客户端
    /// </summary>
    internal IMailClient Client { get; }
    #endregion
    #region 邮件的唯一ID
    /// <summary>
    /// 获取邮件的唯一ID
    /// </summary>
    internal UniqueId ID { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="packMessage">封装的邮件</param>
    /// <param name="client">邮件所在的邮件客户端</param>
    /// <param name="id">邮件的唯一ID</param>
    public MailServed(MimeMessage packMessage, IMailClient client, UniqueId id)
    {
        this.PackMessage = packMessage;
        this.Client = client;
        this.ID = id;
    }
    #endregion
}
