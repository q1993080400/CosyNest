using System.Design;
using System.SafetyFrancis.Authentication;

using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;

namespace System.NetFrancis.Mail;

/// <summary>
/// 这个类型是使用MailKit实现的电子邮件客户端
/// </summary>
sealed class MailClient : AutoRelease, IMailClient
{
    #region 公开成员
    #region 关于邮件集合
    #region 返回元素数量
    public Task<int> CountAsync(CancellationToken cancellation = default)
        => Request((_, inbox, _) => Task.FromResult(inbox.Count), cancellation);
    #endregion
    #region 添加元素
    public Task AddAsync(IMailServed item, CancellationToken cancellation = default)
        => throw new NotImplementedException("不支持显式向邮箱中添加邮件，请使用发送邮件完成这个功能");
    #endregion
    #region 移除元素
    public Task<bool> RemoveAsync(IMailServed item, CancellationToken cancellation = default)
        => Request(async (_, inbox, cancellation) =>
        {
            if (item is MailServed m && m.Client == this)
            {
                await inbox.AddFlagsAsync(m.ID, MessageFlags.Deleted, true, cancellation);
                await inbox.ExpungeAsync(new[] { m.ID }, cancellation);
                return true;
            }
            return false;
        }, cancellation);
    #endregion
    #region 移除全部元素
    public Task ClearAsync(CancellationToken cancellation = default)
        => Request(async (_, inbox, cancellation) =>
        {
            foreach (var item in await inbox.FetchAsync(0, -1, MessageSummaryItems.UniqueId, cancellation))
            {
                await inbox.AddFlagsAsync(item.UniqueId, MessageFlags.Deleted, true, cancellation);
            }
            await inbox.ExpungeAsync(cancellation);
            return 0;
        }, cancellation);
    #endregion
    #region 枚举所有邮件
    public async IAsyncEnumerator<IMailServed> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        var mails = Request(async (client, inbox, cancellationToken) =>
        {
            var mails = await inbox.FetchAsync(0, -1, MessageSummaryItems.UniqueId, cancellationToken);
            return mails.Select(m =>
            {
                var id = m.UniqueId;
                var mail = inbox.GetMessage(id);
                return new MailServed(mail, this, id);
            }).Sort(x => x.Data, false);
        }, cancellationToken);
        foreach (var item in await mails)
        {
            yield return item;
        }
    }
    #endregion
    #region 检查指定信件是否在邮箱中
    public Task<bool> ContainsAsync(IMailServed item, CancellationToken cancellation = default)
        => Task.FromResult(item is MailServed m && m.Client == this);
    #endregion
    #endregion
    #region 发送邮件
    public Task MailsSend(CancellationToken cancellation, params IMailDraft[] draft)
        => Request<SmtpClient, int>(SmtpConnection, async (client, cancellation) =>
         {
             await Task.WhenAll(draft.Select
                 (mail => client.SendAsync
                (mail.ToMail(SmtpConnection.ID), cancellation)));
             return 0;
         }, cancellation);
    #endregion
    #region 有关新邮件到达时的事件
    #region 延迟事件
    /// <summary>
    /// <see cref="NewMail"/>事件的延迟加载封装
    /// </summary>
    private DelayEvent<Action<IMailClient, IMailServed>> NewMailEvent { get; set; }
    #endregion
    #region 事件本体
    public event Action<IMailClient, IMailServed>? NewMail
    {
        add => NewMailEvent += value;
        remove => NewMailEvent -= value;
    }
    #endregion
    #endregion
    #endregion
    #region 内部成员
    #region 对邮件客户端执行方法，然后释放掉它
    /// <summary>
    /// 对邮件客户端执行委托，返回委托的返回值，然后释放掉它
    /// </summary>
    /// <typeparam name="Client">邮件客户端类型</typeparam>
    /// <typeparam name="Ret">返回值类型</typeparam>
    /// <param name="connection">用于连接邮件客户端的信息</param>
    /// <param name="delegate">对邮件客户端执行的委托</param>
    /// <param name="cancellation">用于取消异步任务的令牌</param>
    /// <returns></returns>
    private static async Task<Ret> Request<Client, Ret>(UnsafeWebCredentials connection, Func<Client, CancellationToken, Task<Ret>> @delegate, CancellationToken cancellation = default)
        where Client : IMailService, new()
    {
        using var client = new Client();
        var (id, password, host, ssl) = connection;
        var address = host.Split(':');
        await client.ConnectAsync(address[0], address[1].To<int>(), ssl, cancellation);
        await client.AuthenticateAsync(id, password, cancellation);
        var @return = await @delegate(client, cancellation);
        await client.DisconnectAsync(true, cancellation);
        return @return;
    }
    #endregion
    #region 对IMAP客户端执行方法，然后释放掉它
    /// <summary>
    /// 对IMAP客户端执行委托，返回委托的返回值，然后释放掉它
    /// </summary>
    /// <typeparam name="Ret">返回值类型</typeparam>
    /// <param name="delegate">待执行的委托，它的参数分别是IMAP客户端，收件箱文件夹以及取消令牌</param>
    /// <param name="cancellation">用于取消异步任务的令牌</param>
    /// <returns></returns>
    private Task<Ret> Request<Ret>(Func<ImapClient, IMailFolder, CancellationToken, Task<Ret>> @delegate, CancellationToken cancellation = default)
        => Request<ImapClient, Ret>(ImapConnection, async (client, cancellation) =>
        {
            var inBox = client.Inbox;
            await inBox.OpenAsync(FolderAccess.ReadWrite, cancellation);
            var @return = await @delegate(client, inBox, cancellation);
            await inBox.CloseAsync(cancellationToken: cancellation);
            return @return;
        }, cancellation);
    #endregion
    #region SMTP连接信息
    /// <summary>
    /// 获取用来连接SMTP服务器的信息
    /// </summary>
    private UnsafeWebCredentials SmtpConnection { get; }
    #endregion
    #region IMAP连接信息
    /// <summary>
    /// 获取用来连接IMAP服务器的信息
    /// </summary>
    private UnsafeWebCredentials ImapConnection { get; }
    #endregion
    #region 释放资源
    protected override void DisposeRealize()
        => NewMailEvent.Dispose();
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="smtp">用来发送邮件的SMTP服务器连接方式</param>
    /// <param name="imap">接收邮件的IMAP服务器连接方式</param>
    /// <param name="checkInterval">指定用来检查新邮件的间隔，
    /// 如果为<see langword="null"/>，默认为1分钟</param>
    public MailClient(UnsafeWebCredentials smtp, UnsafeWebCredentials imap, TimeSpan? checkInterval)
    {
        SmtpConnection = smtp;
        ImapConnection = imap;
        NewMailEvent = new NewMailEvent(this, checkInterval ?? TimeSpan.FromMinutes(1));
    }
    #endregion
}
