using System.IOFrancis.FileSystem;
using System.Text.Json.Serialization;

namespace System.DataFrancis;

/// <summary>
/// 这个记录是<see cref="IHasUploadFileServer"/>的实现，
/// 可以视为一个专用于服务端，且封装了上传文件的对象
/// </summary>
record HasUploadFileServer : HasUploadFile, IHasUploadFileServer
{
    #region 保存状态
    [JsonIgnore]
    public UploadFileSaveState SaveState { get; set; }
    #endregion
    #region 存储层文件名称
    private string? FileNameStorageField;

    public override string FileNameStorage
        => FileNameStorageField ??
        base.FileNameStorage;
    #endregion
    #region 更新上传文件名
    public void UpdateUploadFileName(string? fileName = null, string? extendName = null)
    {
        if (SaveState is not UploadFileSaveState.NotSave)
            throw new NotSupportedException($"{nameof(SaveState)}的状态不是{nameof(UploadFileSaveState.NotSave)}，不能调用这个方法");
        var extended = Path.GetExtension(FileName);
        FileNameStorageField = FileNameInfo.Create(fileName, extendName ?? extended).FullName;
        SaveState = UploadFileSaveState.HasUploadFileName;
    }
    #endregion
    #region 指示保存已经完毕
    public void SaveCompleted(string coverUri, string uri)
    {
        if (SaveState is UploadFileSaveState.NotSave)
            throw new NotSupportedException($"{nameof(SaveState)}的状态是{nameof(UploadFileSaveState.NotSave)}，不能调用这个方法" +
                $"请先调用{nameof(UpdateUploadFileName)}");
        CoverUri = coverUri;
        Uri = uri;
        SaveState = UploadFileSaveState.SaveCompleted;
    }
    #endregion
}
