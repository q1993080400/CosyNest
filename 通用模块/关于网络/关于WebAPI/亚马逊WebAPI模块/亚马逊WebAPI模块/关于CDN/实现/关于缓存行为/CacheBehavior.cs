using System.Design;

using Amazon.CloudFront.Model;

namespace System.NetFrancis.Amazon;

/// <summary>
/// 这个类型是<see cref="ICacheBehavior"/>的实现，
/// 可以视为一个亚马逊CDN分配的缓存行为
/// </summary>
/// <param name="amazonCacheBehavior">封装的底层对象，本对象的功能就是通过它实现的</param>
sealed class CacheBehavior(DefaultCacheBehavior amazonCacheBehavior) : ICacheBehavior, IProjection<DefaultCacheBehavior>
{
    #region 函数关联
    public ICollectionFactory<IFunctionAssociation> FunctionAssociations { get; }
        = CreateCollection.CollectionFactory
        ([.. amazonCacheBehavior.FunctionAssociations?.Items.Select(x => x.FromAmazon()) ?? []],
        FunctionAssociation.Create);
    #endregion
    #region 转换为DefaultCacheBehavior
    public DefaultCacheBehavior Projection()
    {
        amazonCacheBehavior.FunctionAssociations = FunctionAssociations.ToAmazon();
        return amazonCacheBehavior;
    }
    #endregion
}
