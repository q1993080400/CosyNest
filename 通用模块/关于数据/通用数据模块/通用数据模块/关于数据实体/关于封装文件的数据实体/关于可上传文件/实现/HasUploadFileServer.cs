using System.Text.Json.Serialization;

namespace System.DataFrancis;

/// <summary>
/// 这个记录是<see cref="IHasUploadFileServer"/>的实现，
/// 可以视为一个专用于服务端，且封装了上传文件的对象
/// </summary>
record HasUploadFileServer : HasUploadFile, IHasUploadFileServer
{
    #region 文件实际存储位置
    [JsonIgnore]
    public IFilePosition? SavePosition { get; private set; }
    #endregion
    #region 建议用来保存的名称
    [JsonIgnore]
    public string? SuggestionSaveName { get; set; }
    #endregion
    #region 指示保存已经完毕
    public void SaveCompleted(string coverUri, string uri, IFilePosition savePosition)
    {
        CoverUri = coverUri;
        Uri = uri;
        SavePosition = savePosition;
    }
    #endregion
}
