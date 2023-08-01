using System.Collections.Immutable;
using System.Design;

namespace Microsoft.AspNetCore.Components.Forms;

/// <summary>
/// 这个参数是上传任务的参数
/// </summary>
public sealed record UploadInfo
{
    #region 上传的目标目录
    /// <summary>
    /// 获取要上传的目标目录
    /// </summary>
    public required string TargetDirectory { get; init; }
    #endregion
    #region 要上传的文件
    /// <summary>
    /// 获取要上传的文件
    /// </summary>
    public required IEnumerable<IBrowserFile> Files { get; init; }
    #endregion
    #region 上传状态
    /// <summary>
    /// 获取一个不可变字典，
    /// 它可以用来传递上传过程中的状态
    /// </summary>
    public Tag<ImmutableDictionary<string, object>> State { get; init; }
    = new()
    {
        Content = ImmutableDictionary<string, object>.Empty
    };
    #endregion
}
