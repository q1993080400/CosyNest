using System.Collections.Generic;
using System.IOFrancis.Bit;

namespace System.NetFrancis.Mail
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以作为一封电子邮件
    /// </summary>
    public interface IMail
    {
        #region 标题
        /// <summary>
        /// 获取电子邮件的标题
        /// </summary>
        string Title { get; }
        #endregion
        #region 正文
        /// <summary>
        /// 这个元组的第一个项如果为<see langword="true"/>，
        /// 代表正文是纯文本，否则代表正文是HTML文档，
        /// 第二个项是邮件的正文部分
        /// </summary>
        (bool IsText, string Body) Body { get; }
        #endregion
        #region 附件
        /// <summary>
        /// 获取一个枚举邮件中所有附件的枚举器
        /// </summary>
        IEnumerable<IBitRead> Attachment { get; }
        #endregion
    }
}
