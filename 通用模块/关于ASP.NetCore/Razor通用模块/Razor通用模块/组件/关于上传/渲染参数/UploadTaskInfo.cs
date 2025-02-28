namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个记录表示一个上传任务
/// </summary>
public sealed record UploadTaskInfo
{
    #region 所有合法文件
    /// <summary>
    /// 获取所有选择，且大小没有超出限制的文件
    /// </summary>
    public required IReadOnlyList<IHasUploadFile> UploadFiles { get; init; }
    #endregion
    #region 大小超出限制的文件
    /// <summary>
    /// 获取所有因为大小超出限制，
    /// 而不能上传的文件
    /// </summary>
    public required IReadOnlyList<IBrowserFile> HugeFiles { get; init; }
    #endregion
    #region 是否存在大小超出限制的文件
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示存在因为大小超出限制，
    /// 而不能上传的文件
    /// </summary>
    public bool HasHugeFile
        => HugeFiles.Count > 0;
    #endregion
    #region 是否选择了文件
    /// <summary>
    /// 获取是否选择了任何文件，
    /// 包括选择了大小超出限制的文件
    /// </summary>
    public bool HasFile
        => UploadFiles.Count > 0 || HasHugeFile;
    #endregion
    #region 上传文件的选项
    /// <summary>
    /// 获取上传文件的选项
    /// </summary>
    public required UploadFileOptions UploadFileOptions { get; init; }
    #endregion
    #region 枚举所有BlobUri
    /// <summary>
    /// 枚举本次上传任务的所有BlobUri，
    /// 它们需要在合适的时机被释放
    /// </summary>
    public IEnumerable<string> AllBlobUri
        => UploadFiles.Select(static x => x.Uri);
    #endregion
}
