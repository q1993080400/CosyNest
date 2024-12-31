namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个封装了可预览文件，
/// 而且仅用于客户端的API数据实体
/// </summary>
public interface IHasUploadFileClient : IHasUploadFile
{
    #region 是否上传完成
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示该文件已经上传完成，
    /// 否则代表尚未上传完成
    /// </summary>
    bool IsUploadCompleted { get; }
    #endregion
    #region 指示上传完成
    /// <summary>
    /// 指示这个文件已经上传完成
    /// </summary>
    void UploadCompleted();
    #endregion
}
