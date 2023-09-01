namespace Microsoft.AspNetCore;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个携带资源的实体，
/// 它在服务器上拥有一个专属的文件夹用来存放资源文件
/// </summary>
public interface IWithResource
{
    #region 删除资源文件夹
    /// <summary>
    /// 删除这个实体的资源文件夹，
    /// 在实体被删除后，可选执行这个操作
    /// </summary>
    void DeleteResourceFolder();
    #endregion
    #region 获取资源文件夹路径
    /// <summary>
    /// 获取资源文件夹的路径
    /// </summary>
    string ResourceFolderPath { get; }
    #endregion
    #region 获取资源的Uri
    /// <summary>
    /// 获取资源文件夹下面所有资源的Uri，
    /// 注意，获取到的是Uri，不是路径，
    /// 它可以直接被前端所使用
    /// </summary>
    /// <returns></returns>
    string[] ResourceUri { get; }
    #endregion
}
