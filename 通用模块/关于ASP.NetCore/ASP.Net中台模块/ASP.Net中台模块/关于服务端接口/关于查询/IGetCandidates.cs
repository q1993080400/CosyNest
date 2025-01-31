namespace Microsoft.AspNetCore;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来获取候选项，
/// 候选项是某数据所有合法值的集合，
/// 获取它不需要任何筛选条件
/// </summary>
/// <typeparam name="Candidates">候选项的类型</typeparam>
public interface IGetCandidates<Candidates>
{
    #region 获取所有候选项
    /// <summary>
    /// 获取所有候选项
    /// </summary>
    /// <returns></returns>
    Task<Candidates[]> AllCandidates();
    #endregion
}
