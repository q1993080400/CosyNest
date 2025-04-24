namespace System.NetFrancis.Amazon;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个亚马逊可修改对象，
/// 它可以向亚马逊服务器提交对自身的修改请求
/// </summary>
public interface IAmazonCanUpdateObject
{
    #region 修改对象
    /// <summary>
    /// 将自身的改动提交到亚马逊服务器
    /// </summary>
    /// <param name="cancellationToken">一个用于取消异步请求的令牌</param>
    /// <returns></returns>
    Task Update(CancellationToken cancellationToken = default);
    #endregion
}
