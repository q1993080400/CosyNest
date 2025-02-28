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
    #region 获取数据流
    public Stream Stream
        => new MemoryStream(Date);
    #endregion
    #region 转换为上传文件
    public IUploadFile ToUploadFile(string? fileName)
    {
        var fileInfo = new FileNameInfo()
        {
            Simple = fileName ?? Guid.CreateVersion7().ToString(),
            Extended = ToolMediaType.Extensions(Type),
        };
        return new UploadFileMemory(fileInfo.FullName, Type, Size, Stream);
    }
    #endregion 
}
