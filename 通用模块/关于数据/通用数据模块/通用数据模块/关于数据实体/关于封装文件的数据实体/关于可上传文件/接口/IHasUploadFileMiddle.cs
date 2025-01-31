namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个特殊的可上传文件，
/// 它只用于在前端和后端之间作为中介
/// </summary>
public interface IHasUploadFileMiddle : IHasPreviewFile
{
    #region 文件ID
    /// <summary>
    /// 获取文件的ID，
    /// 这个文件通常储存在Http报文等其他的地方，
    /// 通过ID可以找到这个文件
    /// </summary>
    Guid FileID { get; }
    #endregion
}
