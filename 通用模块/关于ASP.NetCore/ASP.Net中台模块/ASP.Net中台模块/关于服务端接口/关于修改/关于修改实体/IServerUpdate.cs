using System.NetFrancis.Api;

namespace Microsoft.AspNetCore;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以在服务端实现添加或更新功能
/// </summary>
/// <typeparam name="Parameter">执行添加或更新的参数</typeparam>
public interface IServerUpdate<Parameter>
{
    #region 添加或更新
    /// <summary>
    /// 执行添加或更新
    /// </summary>
    /// <param name="parameter">用来进行添加或更新的参数</param>
    /// <returns></returns>
    Task<APIPack> AddOrUpdate(Parameter parameter);
    #endregion
}
