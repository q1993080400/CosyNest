using System.NetFrancis.Api;

namespace Microsoft.AspNetCore;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来修改实体的部分属性
/// </summary>
public interface IServerUpdatePart<in Parameter>
    where Parameter : class
{
    #region 更新实体的部分属性
    /// <summary>
    /// 更新实体的部分属性
    /// </summary>
    /// <param name="parameter">执行修改需要的参数</param>
    /// <returns></returns>
    Task<APIPack> UpdateProperty(Parameter parameter);
    #endregion
}
