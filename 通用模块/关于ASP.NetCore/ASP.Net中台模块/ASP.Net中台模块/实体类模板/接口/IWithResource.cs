using System.DataFrancis;

namespace Microsoft.AspNetCore;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个携带资源的实体，
/// 它在服务器上拥有一个专属的文件夹用来存放资源文件
/// </summary>
public interface IWithResource : IData
{
    #region 实体属性
    #region 资源文件夹
    /// <summary>
    /// 获取服务器上一个文件夹的地址（不是Uri），
    /// 它里面储存了专属于这个实体的资源文件
    /// </summary>
    string ResourceFolderPath { get; set; }
    #endregion
    #endregion
    #region 非实体成员
    #region 资源文件
    /// <summary>
    /// 获取资源文件夹里面的所有资源文件，
    /// 它的路径是Uri的形式，可以在序列化后直接被前端所使用
    /// </summary>
    string[] ResourceFileUris { get; set; }
    #endregion
    #region 删除资源文件夹
    /// <summary>
    /// 删除这个实体的资源文件夹，
    /// 在实体被删除后，可选执行这个操作
    /// </summary>
    void DeleteResourceFolder();
    #endregion
    #region 初始化资源文件夹
    /// <summary>
    /// 根据<see cref="ResourceFolderPath"/>初始化<see cref="ResourceFileUris"/>，
    /// 这个方法只能在服务器上调用，不能在客户端调用，否则无法得出正确的结果
    /// </summary>
    void InitializationResourceFile();
    #endregion
    #region 解析资源文件的路径
    /// <summary>
    /// 解析资源文件的路径，并返回结果
    /// </summary>
    /// <typeparam name="Output">返回结果的类型</typeparam>
    /// <param name="pathProtocol">用来解析路径的协议，
    /// 它的参数是待解析的路径，返回值是解析结果</param>
    /// <returns></returns>
    IEnumerable<Output> Classify<Output>(Func<IEnumerable<string>, IEnumerable<Output>> pathProtocol);
    #endregion
    #endregion
}
