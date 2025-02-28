namespace System.DataFrancis;

/// <summary>
/// 这个记录是<see cref="IHasUploadFileFusion"/>的实现，
/// 可以视为一个封装了可预览文件，
/// 并且可以同时在服务端和客户端使用的实体
/// </summary>
sealed record HasUploadFileFusion : HasUploadFileServer, IHasUploadFileFusion
{
    #region 是否上传完毕
    private bool IsUploadCompletedField;

    public bool IsUploadCompleted => IsUploadCompletedField || SavePosition is { };
    #endregion
    #region 指示上传完毕
    public void UploadCompleted()
        => IsUploadCompletedField = true;
    #endregion
}
