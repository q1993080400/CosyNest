namespace System.NetFrancis.Mail
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一封已经送达的邮件
    /// </summary>
    public interface IMailServed : IMail
    {
        #region 收件时间
        /// <summary>
        /// 获取收到邮件的时间
        /// </summary>
        DateTimeOffset Data { get; }
        #endregion
        #region 发件人
        /// <summary>
        /// 获取发件人的邮箱地址
        /// </summary>
        string From { get; }
        #endregion
    }
}
