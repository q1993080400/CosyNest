using System.NetFrancis.Api;

namespace Microsoft.AspNetCore;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以在服务端执行删除操作
/// </summary>
/// <typeparam name="Parameter">执行删除的参数</typeparam>
public interface IServerDelete<in Parameter>
{
    #region 执行删除操作
    /// <summary>
    /// 执行删除操作
    /// </summary>
    /// <param name="parameter">用来执行删除的参数</param>
    /// <returns></returns>
    Task<APIPack> Delete(Parameter parameter);
    #endregion
}
