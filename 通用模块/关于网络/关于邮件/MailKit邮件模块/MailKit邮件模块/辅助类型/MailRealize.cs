
using MimeKit;
using MimeKit.Text;

namespace System.NetFrancis.Mail;

/// <summary>
/// 这个静态类是帮助实现电子邮件的辅助类型
/// </summary>
static class MailRealize
{
    #region 将电子邮件草稿转换为MimeMessage
    /// <summary>
    /// 将电子邮件草稿转换为<see cref="MimeMessage"/>
    /// </summary>
    /// <param name="mail">待转换的电子邮件草稿</param>
    /// <param name="from">邮件的发件人</param>
    /// <returns></returns>
    public static MimeMessage ToMail(this IMailDraft mail, string from)
    {
        var (isText, body) = mail.Body;
        var body2 = new Multipart
        {
            new TextPart(isText ? TextFormat.Text : TextFormat.Html)
            {
                Text = body
            },
            mail.Attachment.Select(x =>
            new MimePart()
            {
                FileName = x.Describe ?? "附件" + (x.Format.IsVoid() ? null : $".{x.Format}"),
                Content = new MimeContent(x.ToStream())
            })
        };
        var m = new MimeMessage()
        {
            Subject = mail.Title,
            Body = body2
        };
        m.To.Add(mail.To.Select(x => MailboxAddress.Parse(x)));
        m.From.Add(MailboxAddress.Parse(from));
        return m;
    }
    #endregion
}
