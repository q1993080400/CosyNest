using System.Collections.Immutable;
using System.Design;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个类型是上传中间件的参数
/// </summary>
public sealed record UploadMiddlewareInfo
{
    #region 用来上传的委托
    /// <summary>
    /// 执行这个委托可以启动上传过程，
    /// 它的参数就是上传的目标路径，
    /// 它仅包含直接上传的逻辑，不包括任何额外逻辑
    /// </summary>
    public required Func<string, Task> Upload { get; init; }
    #endregion
    #region 上传的目标目录
    /// <summary>
    /// 获取要上传的目标目录
    /// </summary>
    public required string Path { get; init; }
    #endregion
    #region 服务提供者对象
    /// <summary>
    /// 获取服务提供者对象
    /// </summary>
    public required IServiceProvider ServiceProvider { get; init; }
    #endregion
    #region 用于取消异步操作的令牌
    /// <summary>
    /// 获取一个用于取消异步操作的令牌
    /// </summary>
    public required CancellationToken CancellationToken { get; init; }
    #endregion
    #region 该文件的顺序
    /// <summary>
    /// 获取该文件在上传过程中的顺序，
    /// 排在前面的索引具有更高的优先级
    /// </summary>
    public required IReadOnlyList<int> Index { get; init; }
    #endregion
    #region 文件本来的名字
    /// <summary>
    /// 获取文件的本名，
    /// 也就是它未上传之前本来的名字，
    /// 这个本名包含扩展名
    /// </summary>
    public required string TrueName { get; init; }
    #endregion
    #region 已处理文件路径
    /// <summary>
    /// 这个对象携带了一个不可变哈希表，
    /// 它维护了已经被处理的文件路径，
    /// 也就是因为这次上传而产生的新文件的路径
    /// </summary>
    public required Tag<ImmutableHashSet<string>> ProcessedPath { get; init; }
    #endregion
    #region 上传状态
    /// <summary>
    /// 获取一个不可变字典，
    /// 它可以用来传递上传过程中的状态
    /// </summary>
    public required Tag<ImmutableDictionary<string, object>> State { get; init; }
    #endregion
}
