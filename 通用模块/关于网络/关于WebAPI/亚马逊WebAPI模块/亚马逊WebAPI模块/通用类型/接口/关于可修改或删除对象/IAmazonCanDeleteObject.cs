namespace System.NetFrancis.Amazon;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个亚马逊可删除对象，
/// 它可以向亚马逊服务器提交对自身的删除请求
/// </summary>
public interface IAmazonCanDeleteObject
{
    #region 删除对象
    /// <summary>
    /// 指示亚马逊服务器删除这个对象
    /// </summary>
    /// <param name="cancellationToken">一个用于取消异步请求的令牌</param>
    /// <returns></returns>
    Task Delete(CancellationToken cancellationToken = default);
    #endregion
}
