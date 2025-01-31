namespace System.DataFrancis;

/// <summary>
/// 这个类型是<see cref="IHasUploadFileMiddle"/>的实现，
/// 可以视为一个特殊的可上传文件
/// </summary>
sealed record HasUploadFileMiddle : HasPreviewFile, IHasUploadFileMiddle
{
    #region 文件ID
    public required Guid FileID { get; init; }
    #endregion
}
