using System.NetFrancis.Api;

namespace Microsoft.AspNetCore;

/// <summary>
/// 有关WebApi的工具类
/// </summary>
public static class ToolWebApi
{
    #region 验证WebApi的参数
    #region 指定验证委托
    /// <summary>
    /// 验证WebApi的参数，
    /// 如果验证通过，返回<see langword="null"/>，
    /// 否则返回一个返回值，它告知验证不通过的原因
    /// </summary>
    /// <typeparam name="Return">WebApi的返回值</typeparam>
    /// <param name="verify">用来进行验证的委托</param>
    /// <param name="parameter">WebApi的参数</param>
    /// <returns></returns>
    public static Return? VerifyParameter<Return>(DataVerify verify, object parameter)
        where Return : APIPack, new()
    {
        var verificationResults = verify(parameter);
        return verificationResults.IsSuccess ?
            null :
            new()
            {
                FailureReason = verificationResults.FailureReason.Join(static x => x.Prompt, Environment.NewLine)
            };
    }
    #endregion
    #region 从服务容器中请求
    /// <param name="serviceProvider">用于请求<see cref="DataVerify"/>的服务容器</param>
    /// <inheritdoc cref="VerifyParameter{Return}(DataVerify, object)"/>
    public static Return? VerifyParameter<Return>(IServiceProvider serviceProvider, object parameter)
        where Return : APIPack, new()
    {
        var verify = serviceProvider.GetService<DataVerify>();
        return verify is null ?
            null :
            VerifyParameter<Return>(verify, parameter);
    }
    #endregion
    #endregion
}
