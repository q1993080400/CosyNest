using System.NetFrancis.Http;

namespace Microsoft.AspNetCore;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来获取摘要
/// </summary>
/// <typeparam name="Parameter">参数的类型</typeparam>
/// <typeparam name="Summarize">摘要的类型</typeparam>
public interface IServerSummarize<in Parameter, Summarize>
{
    #region 获取摘要
    /// <summary>
    /// 获取对象的摘要
    /// </summary>
    /// <param name="info">用来获取摘要的参数</param>
    /// <returns></returns>
    [HttpMethodPost]
    Task<Summarize> GetSummarize(Parameter info);
    #endregion
}
