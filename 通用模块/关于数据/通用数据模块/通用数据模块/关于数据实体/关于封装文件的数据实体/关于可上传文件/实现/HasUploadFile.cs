namespace System.DataFrancis;

/// <summary>
/// 这个记录是<see cref="IHasUploadFile"/>的实现，
/// 可以视为一个封装了上传文件的对象
/// </summary>
abstract record HasUploadFile : HasPreviewFile, IHasUploadFile
{
    #region 要上传的文件
    public required IUploadFile UploadFile { get; init; }
    #endregion
}
