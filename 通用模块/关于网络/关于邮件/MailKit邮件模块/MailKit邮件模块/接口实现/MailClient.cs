using System.Collections.Generic;
using System.Design;
using System.Linq;
using System.SafetyFrancis.Authentication;
using System.Threading;
using System.Threading.Tasks;

using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;

namespace System.NetFrancis.Mail
{
    /// <summary>
    /// 这个类型是使用MailKit实现的电子邮件客户端
    /// </summary>
    class MailClient : AutoRelease, IMailClient
    {
        #region 辅助成员
        #region 对邮件客户端执行方法，然后释放掉它
        /// <summary>
        /// 对邮件客户端执行委托，返回委托的返回值，然后释放掉它
        /// </summary>
        /// <typeparam name="Server">邮件客户端类型</typeparam>
        /// <typeparam name="Ret">返回值类型</typeparam>
        /// <param name="connection">用于连接邮件客户端的信息</param>
        /// <param name="delegate">对邮件客户端执行的委托</param>
        /// <returns></returns>
        private async Task<Ret> Request<Server, Ret>(ConnectionInfo connection, Func<Server, Task<Ret>> @delegate)
            where Server : IMailService, new()
        {
            using var server = new Server();
            var (host, port, ssl) = connection;
            await server.ConnectAsync(host, port, ssl);
            var (id, pas) = Credentials;
            await server.AuthenticateAsync(id, pas);
            var @return = await @delegate(server);
            await server.DisconnectAsync(true);
            return @return;
        }
        #endregion
        #region 对IMAP客户端执行方法，然后释放掉它
        /// <summary>
        /// 对IMAP客户端执行委托，返回委托的返回值，然后释放掉它
        /// </summary>
        /// <typeparam name="Ret">返回值类型</typeparam>
        /// <param name="delegate">待执行的委托，它的参数分别是IMAP客户端，以及收件箱文件夹</param>
        /// <returns></returns>
        private Task<Ret> Request<Ret>(Func<ImapClient, IMailFolder, Task<Ret>> @delegate)
            => Request<ImapClient, Ret>(ImapConnection, async server =>
             {
                 var inBox = server.Inbox;
                 await inBox.OpenAsync(FolderAccess.ReadWrite);
                 var @return = await @delegate(server, inBox);
                 await inBox.CloseAsync();
                 return @return;
             });
        #endregion
        #region 用于登录的凭据
        /// <summary>
        /// 获取用于登录服务器的凭据
        /// </summary>
        private UnsafeCredentials Credentials { get; }
        #endregion
        #region SMTP连接信息
        /// <summary>
        /// 获取用来连接SMTP服务器的信息
        /// </summary>
        private ConnectionInfo SmtpConnection { get; }
        #endregion
        #region IMAP连接信息
        /// <summary>
        /// 获取用来连接IMAP服务器的信息
        /// </summary>
        private ConnectionInfo ImapConnection { get; }
        #endregion
        #endregion
        #region 关于邮件
        #region 关于邮件集合
        #region 返回元素数量
        public Task<int> CountAsync
            => Request((server, inbox) => Task.FromResult(inbox.Count));
        #endregion
        #region 添加元素
        public Task AddAsync(IMailServed item)
            => throw new NotImplementedException("不支持显式向邮箱中添加邮件，请使用发送邮件完成这个功能");
        #endregion
        #region 移除元素
        public Task<bool> RemoveAsync(IMailServed item)
            => Request(async (server, inbox) =>
            {
                if (item is MailServed m && m.Client == this)
                {
                    await inbox.AddFlagsAsync(m.ID, MessageFlags.Deleted, true);
                    await inbox.ExpungeAsync(new[] { m.ID });
                    return true;
                }
                return false;
            });
        #endregion
        #region 移除全部元素
        public Task ClearAsync()
            => Request(async (server, inbox) =>
            {
                foreach (var item in await inbox.FetchAsync(0, -1, MessageSummaryItems.UniqueId))
                {
                    await inbox.AddFlagsAsync(item.UniqueId, MessageFlags.Deleted, true);
                }
                await inbox.ExpungeAsync();
                return 0;
            });
        #endregion
        #region 枚举所有邮件
        public async IAsyncEnumerator<IMailServed> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            var mails = Request(async (server, inbox) =>
            {
                var mails = await inbox.FetchAsync(0, -1, MessageSummaryItems.UniqueId);
                return mails.Select(m =>
                {
                    var id = m.UniqueId;
                    var mail = inbox.GetMessage(id);
                    return new MailServed(mail, this, id);
                }).Sort(x => x.Data, false);
            });
            foreach (var item in await mails)
            {
                yield return item;
            }
        }
        #endregion
        #region 检查指定信件是否在邮箱中
        public Task<bool> ContainsAsync(IMailServed item)
            => Task.FromResult(item is MailServed m && m.Client == this);
        #endregion
        #endregion
        #region 发送邮件
        public Task MailsSend(params IMailDraft[] draft)
            => Request<SmtpClient, int>(SmtpConnection, async server =>
            {
                await Task.WhenAll(draft.Select
                    (mail => server.SendAsync
                   (mail.ToMail(Credentials.ID))));
                return 0;
            });
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
        #region 释放资源
        protected override void DisposeRealize()
            => NewMailEvent.Dispose();
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="smtp">用来发送邮件的SMTP服务器连接方式</param>
        /// <param name="imap">接收邮件的IMAP服务器连接方式</param>
        /// <param name="credentials">用来登录邮件服务器的凭据</param>
        /// <param name="checkInterval">指定用来检查新邮件的间隔，
        /// 如果为<see langword="null"/>，默认为1分钟</param>
        public MailClient(ConnectionInfo smtp, ConnectionInfo imap, UnsafeCredentials credentials, TimeSpan? checkInterval)
        {
            SmtpConnection = smtp;
            ImapConnection = imap;
            this.Credentials = credentials;
            NewMailEvent = new NewMailEvent(this, checkInterval ?? TimeSpan.FromMinutes(1));
        }
        #endregion
    }
}
