using System.Collections.Generic;
using System.Design;
using System.Threading.Tasks;

namespace System.NetFrancis.Mail
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个电子邮件客户端，
    /// 它可以用来收发邮件
    /// </summary>
    public interface IMailClient : IAsyncCollection<IMailServed>, IDisposablePro
    {
        #region 说明文档
        /*实现本接口请遵循以下规范：
          #在枚举电子邮件时，应该按收信日期降序排列，
          这是为了符合大多数人的思维习惯
         
          #本接口没有声明有关连接服务器的API，
          因此这些操作应该被程序自动管理*/
        #endregion
        #region 发送邮件
        /// <summary>
        /// 发送电子邮件
        /// </summary>
        /// <param name="draft">电子邮件的草稿</param>
        /// <returns></returns>
        Task MailsSend(params IMailDraft[] draft);
        #endregion
        #region 新邮件到达时的事件
        /// <summary>
        /// 新邮件到达时，触发这个事件，
        /// 事件的第一个参数是当前邮件客户端，
        /// 第二个参数是接收到的新邮件
        /// </summary>
        event Action<IMailClient, IMailServed>? NewMail;

        /*实现本API请遵循以下规范：
          #在调用这个事件时，不要阻塞当前线程，
          因为这个事件中很可能包含回复邮件等耗时操作
          
          #应该尽量通过轮询检查新邮件，而不是通过IDLE接收推送，
          这个规范比较费解，但是具有充分的理由：

          1.有的邮件服务器不支持IDLE，
          或者没有严格按照规范实现该协议（在此点名QQ邮箱），
          这会导致事件无法被触发
        
          2.IDLE需要与服务器保持长连接，
          期间不能请求其他服务（包括收发邮件），
          这会导致对连接的管理变得非常复杂，
          并且很容易带来麻烦的线程同步问题*/
        #endregion
    }
}
