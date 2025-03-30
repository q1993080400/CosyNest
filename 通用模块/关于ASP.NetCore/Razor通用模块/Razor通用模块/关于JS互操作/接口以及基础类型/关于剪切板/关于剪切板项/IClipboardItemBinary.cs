namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个剪切板中的二进制数据项
/// </summary>
public interface IClipboardItemBinary : IClipboardItem
{
    #region 读取数据
    /// <summary>
    /// 获取一个可以读取这个数据的流
    /// </summary>
    Stream Stream { get; }
    #endregion
    #region 获取数据的Uri形式
    /// <summary>
    /// 获取数据的Uri形式，
    /// 它可用于图片预览等用途，
    /// 除非显式指定，否则一般不初始化这个属性
    /// </summary>
    string? ObjectURL { get; }
    #endregion
    #region 转换为浏览器文件
    /// <summary>
    /// 将二进制项转换为一个<see cref="IBrowserFile"/>
    /// </summary>
    /// <param name="fileName">为这个文件取的文件名，
    /// 不包括扩展名，如果为<see langword="null"/>，则自动生成一个</param>
    /// <returns></returns>
    IBrowserFile ToBrowserFile(string? fileName = null);
    #endregion
    #region 转换为上传文件
    /// <summary>
    /// 将二进制项转换为一个可用于上传的文件
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="ToBrowserFile(string?)"/>
    IUploadFile ToUploadFile(string? fileName = null);
    #endregion
    #region 转换为客户端上传文件
    /// <summary>
    /// 将二进制项转换为一个客户端上传文件
    /// </summary>
    /// <param name="uploadFileClientFactory">用来创建客户端上传文件的工厂</param>
    /// <returns></returns>
    /// <inheritdoc cref="ToBrowserFile(string?)"/>
    IHasUploadFileClient ToUploadFileClient(UploadFileClientFactory uploadFileClientFactory, string? fileName = null);
    #endregion
}
