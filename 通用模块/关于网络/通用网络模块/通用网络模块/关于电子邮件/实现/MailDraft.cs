﻿namespace System.NetFrancis.Mail;

/// <summary>
/// 这个类型是<see cref="IMailDraft"/>的实现，
/// 可以作为一个电子邮件草稿
/// </summary>
public sealed record MailDraft : IMailDraft
{
    #region 关于邮件的信息
    #region 标题
    public string Title { get; init; } = "";
    #endregion
    #region 正文
    public (bool IsText, string Body) Body { get; init; } = (true, "");
    #endregion
    #region 收件人
    public IList<string> To { get; }
    = [];
    #endregion
    #endregion
}
