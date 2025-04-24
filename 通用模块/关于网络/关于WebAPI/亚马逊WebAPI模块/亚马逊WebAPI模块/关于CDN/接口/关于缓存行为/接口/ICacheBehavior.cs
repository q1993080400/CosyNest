namespace System.NetFrancis.Amazon;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个亚马逊CDN分配的缓存行为
/// </summary>
public interface ICacheBehavior
{
    #region 函数关联
    /// <summary>
    /// 获取这个CDN缓存行为的函数关联
    /// </summary>
    ICollectionFactory<IFunctionAssociation> FunctionAssociations { get; }
    #endregion
}
