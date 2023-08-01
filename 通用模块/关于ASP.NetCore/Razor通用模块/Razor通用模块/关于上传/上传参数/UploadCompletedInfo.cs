using System.Collections.Immutable;
using System.Design;

namespace Microsoft.AspNetCore.Components.Forms;

/// <summary>
/// 这个记录是上传完成事件的参数
/// </summary>
public sealed record UploadCompletedInfo
{
    #region 新上传的文件路径
    /// <summary>
    /// 这个对象枚举了新上传的文件路径
    /// </summary>
    public required IEnumerable<string> ProcessedPath { get; init; }
    #endregion
    #region 上传状态
    /// <summary>
    /// 这个对象携带了一个不可变字典，它是上传的状态
    /// </summary>
    public required Tag<ImmutableDictionary<string, object>> State { get; init; }
    #endregion
    #region 服务提供者对象
    /// <summary>
    /// 获取服务提供者对象
    /// </summary>
    public required IServiceProvider ServiceProvider { get; init; }
    #endregion
}
