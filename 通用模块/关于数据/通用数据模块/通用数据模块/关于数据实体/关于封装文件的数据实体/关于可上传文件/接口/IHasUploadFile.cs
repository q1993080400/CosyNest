namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个封装了上传文件的API数据实体
/// </summary>
public interface IHasUploadFile : IHasPreviewFile
{
    #region 待上传的文件
    /// <summary>
    /// 获取要上传的文件
    /// </summary>
    IUploadFile UploadFile { get; }
    #endregion
}
