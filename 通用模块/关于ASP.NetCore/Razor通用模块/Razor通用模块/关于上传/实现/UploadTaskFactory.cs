using System.Collections.Immutable;
using System.Design;
using System.IOFrancis;

using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Components.Forms;

/// <summary>
/// 这个类型可以用来创建上传任务工厂
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="serviceProvider">一个用来请求服务的对象</param>
/// <param name="uploadTaskFactoryInfo">创建上传任务工厂的参数</param>
sealed class UploadTaskFactory(IServiceProvider serviceProvider, UploadTaskFactoryInfo uploadTaskFactoryInfo)
{
    #region 公开成员
    #region 创建上传任务工厂
    #region 正式方法
    /// <inheritdoc cref="UploadTaskFactory{Progress}"/>
    public LongTask<decimal> CreateUploadTask(UploadInfo info)
        => async (reportProgress, cancellationToken) =>
        {
            try
            {
                var files = info.Files;
                var uploads = info.TargetDirectory;
                reportProgress ??= _ => Task.CompletedTask;
                var totalSize = files.Sum(x => x.Size);
                var uploaded = 0L;
                var list = new List<Task>();
                var state = new Tag<ImmutableDictionary<string, object>>()
                {
                    Content = ImmutableDictionary<string, object>.Empty
                };
                var processedPath = new Tag<ImmutableHashSet<string>>()
                {
                    Content = []
                };
                foreach (var (item, index, _) in files.PackIndex())
                {
                    #region 本地函数
                    async Task Fun()
                    {
                        await UploadSingle(uploads, item, index, totalSize, uploaded, reportProgress, state, processedPath, cancellationToken);
                        uploaded += item.Size;
                    };
                    #endregion
                    list.Add(Fun());
                }
                await Task.WhenAll(list);
                var uploadCompletedInfo = new UploadCompletedInfo()
                {
                    ProcessedPath = processedPath.Content,
                    ServiceProvider = ServiceProvider,
                    State = state,
                };
                await UploadTaskFactoryInfo.OnUploadCompleted(uploadCompletedInfo);
                await reportProgress(1);
                return true;
            }
            catch (Exception ex)
            {
                if (ex is not OperationCanceledException)
                    throw;
                ex.Log(ServiceProvider);
                return false;
            }
        };
    #endregion
    #region 上传单个文件
    /// <summary>
    /// 上传单个文件
    /// </summary>
    /// <param name="path">上传的目标目录</param>
    /// <param name="file">要上传的文件</param>
    /// <param name="index">文件的顺序</param>
    /// <param name="totalSize">文件的总大小</param>
    /// <param name="uploaded">已经上传的文件大小</param>
    /// <param name="reportProgress">用来报告上传进度的委托</param>
    /// <param name="state">这个不可变字典用来维护上传过程中的状态</param>
    /// <param name="processedPath">这个对象携带了一个不可变哈希表，
    /// 它维护了已经被处理的文件路径，也就是因为这次上传而产生的新文件的路径</param>
    /// <param name="token">一个用来取消异步操作的令牌</param>
    private async Task UploadSingle(string path, IBrowserFile file, int index, long totalSize,
        long uploaded, Func<decimal, Task> reportProgress, Tag<ImmutableDictionary<string, object>> state,
        Tag<ImmutableHashSet<string>> processedPath, CancellationToken token)
    {
        #region 上传单个文件的本地函数
        async Task Fun(string path)
        {
            var maxAllowedSize = UploadTaskFactoryInfo.MaxLength.ConvertToUnit(IUTStorage.ByteMetric).Value.Value;
            using var stream = file.OpenReadStream(maxAllowedSize, token);
            var pipe = stream.ToBitPipe();
            const int stages = 1024 * 16;
            var count = 0;
            using var fileStream = CreateIO.FileStream(path);
            await foreach (var item in pipe.Read.Read(stages, token))
            {
                await fileStream.WriteAsync(item, token);
                uploaded += item.Length;
                count++;
                if (count % 40 is 0)
                    await reportProgress(decimal.Divide(uploaded, totalSize));
            }
        };
        #endregion
        var info = new UploadMiddlewareInfo()
        {
            CancellationToken = token,
            ServiceProvider = ServiceProvider,
            Upload = Fun,
            Index = new[] { index },
            TrueName = file.Name,
            Path = path,
            ProcessedPath = processedPath,
            State = state,
        };
        foreach (var item in UploadTaskFactoryInfo.Middlewares)
        {
            var uploadReturnValue = await item(info);
            if (uploadReturnValue is not UploadReturnValue.NotSupported)
                return;
        }
        throw new NotSupportedException("没有任何中间件可以处理该上传请求");
    }
    #endregion
    #endregion
    #endregion
    #region 内部成员
    #region 服务请求对象
    /// <summary>
    /// 获取一个用来请求服务的对象
    /// </summary>
    private IServiceProvider ServiceProvider { get; } = serviceProvider;
    #endregion
    #region 创建上传任务工厂的参数
    /// <summary>
    /// 获取创建上传任务工厂的参数
    /// </summary>
    private UploadTaskFactoryInfo UploadTaskFactoryInfo { get; } = uploadTaskFactoryInfo;

    #endregion
    #endregion
}
