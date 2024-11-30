using System.Text.Json.Serialization;

namespace System.DataFrancis;

/// <summary>
/// 这个抽象记录是<see cref="ICanCancelPreviewFile"/>的实现，
/// 可以视为一个封装了可预览文件，
/// 且支持取消选择/上传的对象
/// </summary>
abstract record CanCancelPreviewFile : ICanCancelPreviewFile
{
    #region 是否启用对象
    private bool IsEnableField = true;

    [JsonIgnore]
    public bool IsEnable => IsEnableField;
    #endregion
    #region 取消选择
    public void Cancel()
        => IsEnableField = false;
    #endregion
}
