using System.IOFrancis.FileSystem;
using System.Media;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是<see cref="IClipboardItemBinary"/>的实现，
/// 可以视为一个封装二进制值的剪切板项
/// </summary>
/// <param name="Size">数据的大小</param>
/// <param name="Type">数据的MIME类型</param>
/// <param name="Date">数据的二进制值</param>
/// <param name="ObjectURL">数据的Uri形式</param>
sealed record ClipboardItemBinary(long Size, string Type, byte[] Date, string? ObjectURL) : IClipboardItemBinary
{
    #region 公开成员
    #region 获取数据流
    public Stream Stream
        => new MemoryStream(Date);
    #endregion
    #region 转换为浏览器文件
    public IBrowserFile ToBrowserFile(string? fileName = null)
        => new BrowserFileMemory(GetFileName(fileName), DateTimeOffset.Now, Size, Type, Stream);
    #endregion
    #region 转换为上传文件
    public IUploadFile ToUploadFile(string? fileName)
        => new UploadFileMemory(GetFileName(fileName), Type, Size, Stream);
    #endregion
    #region 转换为客户端上传文件
    public IHasUploadFileClient ToUploadFileClient(UploadFileClientFactory uploadFileClientFactory, string? fileName = null)
    {
        var file = ToUploadFile(fileName);
        var uploadFileName = file.FileName;
        return uploadFileClientFactory(uploadFileName, uploadFileName, file);
    }
    #endregion
    #endregion 
    #region 内部成员
    #region 给文件取名
    /// <summary>
    /// 给这个文件取个名字
    /// </summary>
    /// <param name="fileName">为这个文件取的文件名，
    /// 不包括扩展名，如果为<see langword="null"/>，则自动生成一个</param>
    /// <returns></returns>
    private string GetFileName(string? fileName)
    {
        var fileInfo = new FileNameInfo()
        {
            Simple = fileName ?? Guid.CreateVersion7().ToString(),
            Extended = ToolMediaType.Extensions(Type),
        };
        return fileInfo.FullName;
    }
    #endregion
    #endregion
}
