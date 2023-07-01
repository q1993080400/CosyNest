using System.Design;
using System.IOFrancis;
using System.IOFrancis.FileSystem;
using System.Media.Play;

using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Components.Forms;

/// <summary>
/// 这个类型可以用来创建上传任务工厂，
/// 它能够上传文件，并且正确地为图片和视频创建封面
/// </summary>
sealed class UploadTaskFactory
{
    #region 公开成员
    #region 创建上传任务工厂
    #region 正式方法
    /// <inheritdoc cref="UploadTaskFactory{Progress}"/>
    public LongTask<decimal> CreateUploadTask(string uploads, IEnumerable<IBrowserFile> files)
        => async (reportProgress, cancellationToken) =>
        {
            try
            {
                reportProgress ??= _ => Task.CompletedTask;
                var totalSize = files.Sum(x => x.Size);
                var uploaded = 0L;
                var list = new List<Task>();
                foreach (var (item, index, _) in files.PackIndex())
                {
                    #region 本地函数
                    async Task Fun()
                    {
                        await UploadSingle(uploads, item, index, totalSize, uploaded, reportProgress, cancellationToken);
                        uploaded += item.Size;
                    };
                    #endregion
                    list.Add(Fun());
                }
                await Task.WhenAll(list);
                await reportProgress(1);
                return true;
            }
            catch (Exception ex)
            {
                if (ex is not OperationCanceledException)
                    throw;
                LoggerProvider.CreateLogger("").LogError(ex, "");
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
    /// <param name="token">一个用来取消异步操作的令牌</param>
    private async Task UploadSingle(string path, IBrowserFile file, int index, long totalSize, long uploaded, Func<decimal, Task> reportProgress, CancellationToken token)
    {
        var parameters = new FilePathGenerateParameters()
        {
            Sort = index,
            Path = path,
            PathOrName = file.Name,
            CoverExtension = UploadTaskFactoryInfo.CoverFormat
        };
        var fileSource = Generate(parameters);
        #region 上传单个文件的本地函数
        async Task Fun(IBrowserFile file, string path, bool report)
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
                if (report && count % 40 is 0)
                    await reportProgress(decimal.Divide(uploaded, totalSize));
            }
        }
        #endregion
        switch (fileSource)
        {
            case MediaSource { MediaSourceType: FileSourceType.WebImage, CoverPath: var converPath }:
                await Fun(file, fileSource.FilePath, true);
                var (width, height) = UploadTaskFactoryInfo.MaxImageCoverSize;
                var conver = await file.RequestImageFileAsync(Path.GetExtension(file.Name), width, height);
                await Fun(conver, converPath, false);
                break;
            case MediaSource { MediaSourceType: FileSourceType.WebVideo, FilePath: { } filePath, CoverPath: var converPath }:
                var extended = ToolPath.SplitPathFile(filePath).Extended;
                var temporaryFilesPath = ToolTemporaryFile.CreateTemporaryPath(extended);
                await Fun(file, temporaryFilesPath, true);
                var targetPath = ToolPath.RefactoringPath(filePath, newExtension: _ => "mp4");
                await VideoProcessing.FormatConversion(CreateIO.File(temporaryFilesPath), targetPath, new()
                {
                    MaxDefinition = UploadTaskFactoryInfo.MaxDefinition,
                    CancellationToken = token,
                    ReportProgress = reportProgress,
                });
                File.Delete(temporaryFilesPath);
                await VideoProcessing.Screenshot(CreateIO.File(targetPath), new[]
                {
                    (TimeSpan.Zero,converPath)
                }, reportProgress, token);
                break;
            case var fs:
                await Fun(file, fs.FilePath, true);
                break;
        }
    }
    #endregion
    #endregion
    #endregion
    #region 内部成员
    #region 日志记录器
    /// <summary>
    /// 获取日志记录器
    /// </summary>
    private ILoggerProvider LoggerProvider { get; }
    #endregion
    #region 路径生成器
    /// <summary>
    /// 获取一个用于生成媒体路径的生成器
    /// </summary>
    private GenerateFilePathProtocol<FilePathGenerateParameters, FileSource> Generate { get; }
    #endregion
    #region 视频处理器
    /// <summary>
    /// 获取视频处理器，它负责执行视频转换和压缩
    /// </summary>
    private IVideoProcessing VideoProcessing { get; }
    #endregion
    #region 创建上传任务工厂的参数
    /// <summary>
    /// 获取创建上传任务工厂的参数
    /// </summary>
    private UploadTaskFactoryInfo UploadTaskFactoryInfo { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="loggerProvider">日志记录器</param>
    /// <param name="generate">一个用于生成媒体路径的生成器</param>
    /// <param name="videoProcessing">视频处理器，它负责执行视频转换和压缩</param>
    /// <param name="uploadTaskFactoryInfo">创建上传任务工厂的参数</param>
    public UploadTaskFactory(ILoggerProvider loggerProvider, GenerateFilePathProtocol<FilePathGenerateParameters, FileSource> generate,
        IVideoProcessing videoProcessing, UploadTaskFactoryInfo uploadTaskFactoryInfo)
    {
        LoggerProvider = loggerProvider;
        Generate = generate;
        VideoProcessing = videoProcessing;
        UploadTaskFactoryInfo = uploadTaskFactoryInfo;
    }
    #endregion
}
