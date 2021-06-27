using System.Design;
using System.Linq;
using System.Time;

namespace System.NetFrancis.Mail
{
    /// <summary>
    /// 这个类型为<see cref="MailClient.NewMail"/>事件提供支持
    /// </summary>
    class NewMailEvent : DelayEvent<Action<IMailClient, IMailServed>>
    {
        #region 封装的对象
        #region 电子邮件客户端
        /// <summary>
        /// 用来检查新邮件的电子邮件客户端
        /// </summary>
        private IMailClient MailClient { get; }
        #endregion
        #region 用于检查新邮件的计时器
        /// <summary>
        /// 用来检查新邮件的计时器
        /// </summary>
        private ITimer? CheckTimer { get; set; }
        #endregion
        #region 检查新邮件的间隔
        /// <summary>
        /// 获取用来检查新邮件的间隔
        /// </summary>
        private TimeSpan CheckInterval { get; }
        #endregion
        #region 用于缓存旧邮件的集合
        /// <summary>
        /// 用于缓存旧邮件的集合
        /// </summary>
        private IMailServed[]? OldMail { get; set; }
        #endregion
        #endregion
        #region 抽象类实现
        #region 初始化事件
        protected override void Initialization()
        {
            #region 用来检查新邮件的本地函数
            void CheckNewMail()
            {
                var newMails = MailClient.Fit().ToArray();
                foreach (var item in newMails.Except(OldMail, FastRealize.EqualityComparer<IMailServed>(x => x.Data)))
                {
                    Delegate?.Invoke(MailClient, item);
                }
                OldMail = newMails;
            }
            #endregion
            OldMail = MailClient.Fit().ToArray();
            CheckTimer = CreateTimer.Timer(CheckInterval, null);
            CheckTimer.Due += CheckNewMail;
            CheckTimer.Start();
        }
        #endregion
        #region 清理事件
        public override void Dispose()
        {
            CheckTimer?.Dispose();
            CheckTimer = null;
            OldMail = null;
        }
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="mailClient">用来检查新邮件的电子邮件客户端</param>
        /// <param name="checkInterval">用来检查新邮件的间隔</param>
        public NewMailEvent(IMailClient mailClient, TimeSpan checkInterval)
        {
            this.MailClient = mailClient;
            this.CheckInterval = checkInterval;
        }
        #endregion
    }
}
