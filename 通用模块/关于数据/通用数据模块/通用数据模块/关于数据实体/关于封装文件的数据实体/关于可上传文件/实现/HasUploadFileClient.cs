namespace System.DataFrancis;

/// <summary>
/// 这个记录是<see cref="IHasUploadFileClient"/>的实现，
/// 可以视为一个专用于客户端，且封装了上传文件的对象
/// </summary>
sealed record HasUploadFileClient : HasUploadFile, IHasUploadFileClient
{
    #region 是否上传完毕
    private bool IsUploadCompletedField;

    public bool IsUploadCompleted => IsUploadCompletedField;
    #endregion
    #region 指示上传完毕
    public void UploadCompleted()
        => IsUploadCompletedField = true;
    #endregion
}
