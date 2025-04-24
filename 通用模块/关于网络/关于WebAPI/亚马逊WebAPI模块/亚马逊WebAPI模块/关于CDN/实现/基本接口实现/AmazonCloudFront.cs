using Amazon.CloudFront;
using Amazon.CloudFront.Model;

namespace System.NetFrancis.Amazon;

/// <summary>
/// 这个类型是<see cref="IAmazonCloudFront"/>的实现，
/// 可以视为一个亚马逊云CDN
/// </summary>
/// <param name="amazonCloudFrontClient">封装的亚马逊云CDN对象</param>
sealed class AmazonCloudFront(AmazonCloudFrontClient amazonCloudFrontClient) : AmazonAPI(amazonCloudFrontClient), IAmazonCloudFront
{
    #region 返回所有分配
    public async Task<IReadOnlyList<IAmazonCloudFrontDistribution>> GetAllDistributions(CancellationToken cancellationToken = default)
    {
        var listDistributionsResponse = await amazonCloudFrontClient.ListDistributionsAsync(cancellationToken);
        var getDistributionRequests = listDistributionsResponse.DistributionList.Items.
            Select(x => new GetDistributionRequest()
            {
                Id = x.Id,
            }).ToArray();
        var list = new List<IAmazonCloudFrontDistribution>();
        foreach (var getDistributionRequest in getDistributionRequests)
        {
            var getDistributionResponse = await amazonCloudFrontClient.GetDistributionAsync(getDistributionRequest, cancellationToken);
            var distribution = new AmazonCloudFrontDistribution(amazonCloudFrontClient, getDistributionResponse.ETag, getDistributionResponse.Distribution);
            list.Add(distribution);
        }
        return list;
    }
    #endregion
    #region 创建新的分配
    public async Task<IAmazonCloudFrontDistribution> CreateDistribution(Action<IAmazonCloudFrontDistributionDraft> configurationDistribution, CancellationToken cancellationToken = default)
    {
        var distributionDraft = new AmazonCloudFrontDistributionDraft();
        configurationDistribution(distributionDraft);
        var request = distributionDraft.Projection();
        var response = await amazonCloudFrontClient.CreateDistributionAsync(request, cancellationToken);
        return new AmazonCloudFrontDistribution(amazonCloudFrontClient, response.ETag, response.Distribution);
    }
    #endregion
}
