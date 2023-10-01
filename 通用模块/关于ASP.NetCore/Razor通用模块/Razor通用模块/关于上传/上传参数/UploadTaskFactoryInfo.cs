using System.IOFrancis;
using System.MathFrancis;

namespace Microsoft.AspNetCore.Components.Forms;

/// <summary>
/// 这个记录是创建上传任务工厂的参数
/// </summary>
public sealed record UploadTaskFactoryInfo
{
    #region 最大上传大小限制
    /// <summary>
    /// 获取文件的最大上传大小限制
    /// </summary>
    public required IUnit<IUTStorage> MaxLength { get; init; }
    #endregion
    #region 上传中间件集合
    /// <summary>
    /// 获取上传中间件的集合，
    /// 函数会按照顺序，
    /// 询问每个中间件是否能够处理上传文件
    /// </summary>
    public IReadOnlyList<UploadMiddleware> Middlewares { get; init; } = new List<UploadMiddleware>()
    {
        UploadMiddlewareCommon.UploadAll()
    };
    #endregion
    #region 上传后事件
    /// <summary>
    /// 当上传完毕后，执行这个事件，
    /// 它的参数是一个记录，描述了上传结果
    /// </summary>
    public Func<UploadCompletedInfo, Task> OnUploadCompleted { get; init; } = _ => Task.CompletedTask;
    #endregion
}
