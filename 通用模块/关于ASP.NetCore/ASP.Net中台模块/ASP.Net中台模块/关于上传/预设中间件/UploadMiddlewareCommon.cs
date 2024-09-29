using System.IOFrancis;
using System.IOFrancis.FileSystem;
using System.Media.Drawing;
using System.Media.Drawing.PDF;
using System.Media.Play;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore;

/// <summary>
/// 获取预设的上传中间件
/// </summary>
public static class UploadMiddlewareCommon
{
    #region 公开成员
    #region 用来上传视频的中间件
    /// <summary>
    /// 返回一个用来上传视频的中间件，它执行以下操作：
    /// 为视频生成封面，
    /// 压缩视频以加快访问速度，
    /// 将视频转换为mp4格式以提高兼容性
    /// </summary>
    /// <param name="mediumInfo">上传视频的参数</param>
    /// <returns></returns>
    public static UploadMiddleware UploadVideo(UploadMiddlewareMediumInfo mediumInfo)
        => async info =>
        {
            if (GenerateFileSource(info, mediumInfo.CoverFormat) is not MediaSource { MediaSourceType: FileSourceType.WebVideo, FilePath: { } filePath, CoverPath: var converPath } source)
                return UploadReturnValue.NotSupported;
            var extended = source.FileInfo.Extended;
            using var temporaryFile = ToolTemporaryFile.CreateTemporaryPath(extended);
            var temporaryFilesPath = temporaryFile.TemporaryObj;
            var targetPath = ToolPath.RefactoringPath(filePath, newExtension: _ => "mp4");
            try
            {
                var cancellationToken = info.CancellationToken;
                cancellationToken.ThrowIfCancellationRequested();
                await info.Upload(temporaryFilesPath);
                cancellationToken.ThrowIfCancellationRequested();
                var videoProcessing = info.ServiceProvider.GetRequiredService<IVideoProcessing>();
                await videoProcessing.FormatConversion(new()
                {
                    MediaPath = temporaryFilesPath,
                    TargetPath = targetPath,
                    MaxDefinition = mediumInfo.MaxDefinition,
                    CancellationToken = cancellationToken
                });
                cancellationToken.ThrowIfCancellationRequested();
                await videoProcessing.Screenshot(new()
                {
                    Fragment =
                    [
                        (TimeSpan.Zero, converPath)
                    ],
                    MediaPath = targetPath,
                    CancellationToken = cancellationToken
                });
                var set = info.ProcessedPath;
                set.Content = set.Content?.Union([targetPath, converPath]);
                return UploadReturnValue.Success;
            }
            catch (Exception ex)
            {
                File.Delete(targetPath);
                File.Delete(converPath);
                if (ex is not OperationCanceledException)
                    throw;
                return UploadReturnValue.Fail;
            }
        };
    #endregion
    #region 用来上传图像的中间件
    /// <summary>
    /// 返回一个用来上传图像的中间件，
    /// 它执行以下操作：为图像生成封面
    /// </summary>
    /// <param name="mediumInfo">上传图像的参数</param>
    /// <returns></returns>
    public static UploadMiddleware UploadImage(UploadMiddlewareMediumInfo mediumInfo)
        => async info =>
        {
            var source = GenerateFileSource(info, mediumInfo.CoverFormat);
            if (source is not MediaSource { MediaSourceType: FileSourceType.WebImage, FilePath: { } filePath, CoverPath: var converPath })
                return UploadReturnValue.NotSupported;
            var imagePath = ToolPath.RefactoringPath(filePath, newExtension: _ => "webp");
            try
            {
                var cancellationToken = info.CancellationToken;
                cancellationToken.ThrowIfCancellationRequested();
                await info.Upload(filePath);
                cancellationToken.ThrowIfCancellationRequested();
                var imageProcessing = info.ServiceProvider.GetRequiredService<IImageProcessing>();
                await imageProcessing.FormatConversion(filePath, converPath, mediumInfo.MaxImageCoverSize, info.CancellationToken);
                await imageProcessing.FormatConversion(filePath, imagePath, null, info.CancellationToken);
                var set = info.ProcessedPath;
                set.Content = set.Content?.Union([imagePath, converPath]);
                File.Delete(filePath);
                return UploadReturnValue.Success;
            }
            catch (Exception ex)
            {
                File.Delete(filePath);
                File.Delete(imagePath);
                File.Delete(converPath);
                if (ex is not OperationCanceledException)
                    throw;
                return UploadReturnValue.Fail;
            }
        };
    #endregion
    #region 上传PDF并提取其中图片的中间件
    /// <summary>
    /// 返回一个专门用来上传PDF的中间件，
    /// 它执行以下操作：
    /// 提取该PDF中的图片，并为其创建封面，
    /// 然后丢弃该PDF
    /// </summary>
    /// <param name="createPDF">通过文件路径创建PDF文档的委托</param>
    /// <param name="mediumInfo">用来创建图片封面的媒体参数</param>
    /// <returns></returns>
    public static UploadMiddleware UploadPDFExtractingImages(Func<string, Task<IPDFDocument>> createPDF, UploadMiddlewareMediumInfo mediumInfo)
        => async info =>
        {
            if (ToolPath.SplitFilePath(info.TrueName).Extended is not "pdf")
                return UploadReturnValue.NotSupported;
            var filePath = Path.Combine(info.Path, $"{Guid.NewGuid()}.pdf");
            IPDFDocument? pdf = null;
            try
            {
                await info.Upload(filePath);
                pdf = await createPDF(filePath);
                pdf.AutoSave = false;
                var images = pdf.Pages.Select(x => x.Images).SelectMany(x => x).ToArray();
                var processPictures = UploadImage(mediumInfo);
                var serviceProvider = info.ServiceProvider;
                var cancellationToken = info.CancellationToken;
                foreach (var (index, item, _) in images.PackIndex())
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var imageInfo = new UploadMiddlewareInfo()
                    {
                        Upload = async path =>
                        {
                            await item.Save(path);
                        },
                        CancellationToken = cancellationToken,
                        Index = [.. info.Index, index],
                        ServiceProvider = serviceProvider,
                        Path = info.Path,
                        TrueName = $"{index}.{item.Format}",
                        ProcessedPath = info.ProcessedPath,
                        State = info.State
                    };
                    try
                    {
                        await processPictures(imageInfo);
                    }
                    catch (Exception ex)
                    {
                        ex.Log(serviceProvider);
                        if (ex is OperationCanceledException)
                            throw;
                        cancellationToken.ThrowIfCancellationRequested();
                    }
                }
                return UploadReturnValue.Success;
            }
            catch (Exception ex)
            {
                if (ex is not OperationCanceledException)
                    throw;
                return UploadReturnValue.Fail;
            }
            finally
            {
                if (pdf is { })
                    await pdf.DisposeAsync();
                File.Delete(filePath);
            }
        };
    #endregion
    #region 用来上传任何文件的中间件
    /// <summary>
    /// 这个中间件可以用来上传任何文件，
    /// 它仅进行上传和自动命名操作，不执行其他操作
    /// </summary>
    /// <returns></returns>
    public static UploadMiddleware UploadAll()
        => static async info =>
        {
            var source = GenerateFileSource(info);
            var filePath = source.FilePath;
            try
            {
                await info.Upload(filePath);
                var set = info.ProcessedPath;
                set.Content = set.Content?.Add(filePath);
                return UploadReturnValue.Success;
            }
            catch (Exception ex)
            {
                File.Delete(filePath);
                if (ex is not OperationCanceledException)
                    throw;
                return UploadReturnValue.Fail;
            }
        };
    #endregion
    #region 如果上传失败会删除上传目录的中间件
    /// <summary>
    /// 这个中间件借助其他中间件的力量进行上传，
    /// 如果失败，会自动删除整个上传目录，警告：
    /// 仅在使用专用目录上传的时候，才能使用它，
    /// 因为同一目录可能存在其他有用的文件
    /// </summary>
    /// <param name="middlewares">用来进行上传的中间件，
    /// 本中间件能够处理什么类型的文件，由它们决定</param>
    /// <returns></returns>
    public static UploadMiddleware UploadSandbox(IReadOnlyList<UploadMiddleware> middlewares)
        => async info =>
        {
            #region 用来执行删除的本地函数
            void Delete()
            => CreateIO.Directory(info.Path)?.Delete();
            #endregion
            try
            {
                foreach (var item in middlewares)
                {
                    var result = await item(info);
                    switch (result)
                    {
                        case UploadReturnValue.Success:
                            return result;
                        case UploadReturnValue.Fail:
                            Delete();
                            return result;
                    }
                }
                return UploadReturnValue.NotSupported;
            }
            catch (Exception)
            {
                Delete();
                throw;
            }
        };
    #endregion
    #endregion
    #region 内部成员
    #region 根据中间件参数生成文件源
    /// <summary>
    /// 根据中间件参数，生成一个文件源，
    /// 它指示上传文件的最终路径
    /// </summary>
    /// <param name="info">中间件参数</param>
    /// <param name="coverFormat">如果生成封面，则这个参数指定封面格式</param>
    /// <returns></returns>
    private static FileSource GenerateFileSource(UploadMiddlewareInfo info, string coverFormat = "webp")
    {
        var parameters = new FilePathGenerateParameters()
        {
            Sort = info.Index,
            Path = info.Path,
            TrueName = info.TrueName,
            CoverExtension = coverFormat
        };
        var generate = info.ServiceProvider.GetRequiredService<GenerateFilePathProtocol<FilePathGenerateParameters, FileSource>>();
        return generate(parameters);
    }
    #endregion 
    #endregion
}
