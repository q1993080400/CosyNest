
using Microsoft.AspNetCore.Components.Forms;

namespace BootstrapBlazor.Components;

/// <summary>
/// 这个类型是<see cref="IBrowserFile"/>的实现，
/// 它代表一个不能上传的文件
/// </summary>
sealed class HugeBrowserFile : IBrowserFile
{
    #region 读取文件
    public Stream OpenReadStream(long maxAllowedSize = 512000, CancellationToken cancellationToken = default)
        => throw new NotImplementedException("这个文件不能上传，所以也不允许读取它");
    #endregion
    #region 文件名
    public required string Name { get; init; }
    #endregion
    #region 修改时间
    public required DateTimeOffset LastModified { get; init; }
    #endregion
    #region 文件大小
    public required long Size { get; init; }
    #endregion
    #region 文件内容
    public required string ContentType { get; init; }
    #endregion 
}
