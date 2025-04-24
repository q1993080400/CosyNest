namespace System.NetFrancis.Amazon;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个亚马逊CDN的WebAPI
/// </summary>
public interface IAmazonCloudFront : IAmazonAPI
{
    #region 获取所有CDN分配
    /// <summary>
    /// 获取所有CDN分配
    /// </summary>
    /// <param name="cancellationToken">一个用来取消异步操作的令牌</param>
    /// <returns></returns>
    Task<IReadOnlyList<IAmazonCloudFrontDistribution>> GetAllDistributions(CancellationToken cancellationToken = default);
    #endregion
    #region 创建新的分配
    /// <summary>
    /// 创建一个新的分配对象，
    /// 并将更改提交到亚马逊服务器
    /// </summary>
    /// <param name="configurationDistribution">一个用来配置新创建的分配对象的委托</param>
    /// <param name="cancellationToken">一个用来取消异步操作的令牌</param>
    /// <returns>新创建的分配对象</returns>
    Task<IAmazonCloudFrontDistribution> CreateDistribution(Action<IAmazonCloudFrontDistributionDraft> configurationDistribution, CancellationToken cancellationToken = default);
    #endregion
}
