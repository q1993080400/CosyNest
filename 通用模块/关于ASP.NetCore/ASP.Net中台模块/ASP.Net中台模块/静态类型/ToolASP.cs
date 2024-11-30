using System.DataFrancis;
using System.DataFrancis.EntityDescribe;
using System.NetFrancis.Api;

namespace Microsoft.AspNetCore;

/// <summary>
/// 有关ASP.Net的工具类
/// </summary>
public static class ToolASP
{
    #region 验证WebApi的参数
    /// <summary>
    /// 验证WebApi的参数，
    /// 如果验证通过，返回<see langword="null"/>，
    /// 否则返回一个返回值，它告知验证不通过的原因
    /// </summary>
    /// <typeparam name="Return">WebApi的返回值</typeparam>
    /// <param name="parameter">WebApi的参数</param>
    /// <param name="verify">用于验证的委托，
    /// 如果为<see langword="null"/>，则使用默认方法</param>
    /// <returns></returns>
    public static Return? VerifyParameter<Return>(object parameter, DataVerify? verify = null)
        where Return : APIPack, new()
    {
        verify ??= CreateDataObj.DataVerifyDefault();
        var verificationResults = verify(parameter);
        return verificationResults.IsSuccess ?
            null :
            new()
            {
                FailureReason = verificationResults.FailureReason.Join(static x => x.Prompt, Environment.NewLine)
            };
    }
    #endregion
    #region 有关JS互操作
    #region 获取前端基准超时时间
    /// <summary>
    /// 获取前端基准超时时间，
    /// 它可以用来全局地控制等待前端DOM渲染的时间，默认为100毫秒
    /// </summary>
    public static TimeSpan BaseTimeOut { get; set; } = TimeSpan.FromMilliseconds(100);

    /*问：为什么需要本属性？
      答：有的时候，前端需要等待DOM元素渲染完毕后再执行某些操作，
      但是，这个时间是无法预测的，需要估计，
      如果写死的话，后续网络和硬件条件发生改变时会非常不方便，
      因此作者设计了本属性，所有的超时等待都是以它为单位，解决了这个问题*/
    #endregion
    #endregion
}
