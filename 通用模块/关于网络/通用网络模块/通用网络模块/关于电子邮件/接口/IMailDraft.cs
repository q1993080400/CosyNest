using System.Collections.Generic;
using System.IOFrancis.Bit;

namespace System.NetFrancis.Mail
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以作为一个电子邮件草稿
    /// </summary>
    public interface IMailDraft : IMail
    {
        #region 说明文档
        /*问：既然本接口实现IDisposable是为了释放附件，
          那么为什么IMailServed不实现这个接口？
          答：因为作者考虑到，本接口Attachment一般是一个集合，
          而IMailServed的Attachment一般通过迭代器延迟返回，
          如果IMailServed也实现IDisposable的话，
          调用Dispose时会枚举迭代器，反而造成不必要的性能损失，有画蛇添足之嫌*/
        #endregion
        #region 标题
        /// <summary>
        /// 获取或设置电子邮件的标题
        /// </summary>
        new string Title { get; }
        #endregion
        #region 正文
        /// <summary>
        /// 这个元组的第一个项如果为<see langword="true"/>，
        /// 代表正文是纯文本，否则代表正文是HTML文档，
        /// 第二个项是邮件的正文部分
        /// </summary>
        new(bool IsText, string Body) Body { get; }
        #endregion
        #region 附件
        /// <summary>
        /// 枚举邮件中的所有附件
        /// </summary>
        new IList<IBitRead> Attachment { get; }
        #endregion
        #region 收件人
        /// <summary>
        /// 枚举所有收件人的邮箱地址
        /// </summary>
        IList<string> To { get; }
        #endregion
    }
}
