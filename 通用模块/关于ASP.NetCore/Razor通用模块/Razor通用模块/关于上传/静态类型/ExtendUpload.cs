using System.NetFrancis.Http;

namespace System;

public static partial class ExtendRazor
{
    //这个部分类专门声明有关上传的扩展方法

    #region 关于IUploadFile
    #region 从IBrowserFile集合读取
    /// <summary>
    /// 读取一个<see cref="IBrowserFile"/>集合中的所有文件
    /// </summary>
    /// <param name="files">待读取文件的<see cref="IBrowserFile"/>集合</param>
    /// <param name="maxAllowedSize">可以读取的最大字节数，
    /// 超过这个限制会引发异常，默认为500KB</param>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    public static IReadOnlyList<IUploadFile> ReadUploadFile(this IEnumerable<IBrowserFile> files,
        long maxAllowedSize = 512000, CancellationToken cancellationToken = default)
        => files.Select(x => new UploadFile(x, maxAllowedSize, cancellationToken)).ToArray();
    #endregion
    #region 从InputFileChangeEventArgs读取
    /// <summary>
    /// 读取一个<see cref="InputFileChangeEventArgs"/>中的所有文件
    /// </summary>
    /// <param name="e">待读取文件的<see cref="InputFileChangeEventArgs"/></param>
    /// <returns></returns>
    /// <inheritdoc cref="ReadUploadFile(IEnumerable{IBrowserFile}, long, CancellationToken)"/>
    public static IReadOnlyList<IUploadFile> ReadUploadFile(this InputFileChangeEventArgs e,
        long maxAllowedSize = 512000, CancellationToken cancellationToken = default)
        => e.GetMultipleFiles(e.FileCount).ReadUploadFile(maxAllowedSize, cancellationToken);
    #endregion
    #endregion 
}
