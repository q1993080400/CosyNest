using System.Text.Json.Serialization;

namespace System.DataFrancis;

/// <summary>
/// 这个记录是<see cref="IHasUploadFile"/>的实现，
/// 可以视为一个封装了上传文件的对象
/// </summary>
sealed record HasUploadFile : CanCancelPreviewFile, IHasUploadFile
{
    #region 封面Uri
    public required string CoverUri { get; init; }
    #endregion
    #region 本体Uri
    public required string Uri { get; init; }
    #endregion
    #region 要上传的文件
    [JsonIgnore]
    public IUploadFile UploadFile { get; init; }
    #endregion
    #region 文件名
    public string FileName
        => UploadFile?.FileName ?? "";
    #endregion
}
