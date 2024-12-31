namespace System.DataFrancis;

public static partial class CreateDataObj
{
    //这个部分类专门用来声明创建和上传有关的对象的API

    #region 创建包含只读预览文件的对象
    /// <summary>
    /// 创建一个包含只读可预览文件的对象
    /// </summary>
    /// <param name="coverUri">封面的Uri</param>
    /// <param name="uri">本体的Uri</param>
    /// <param name="fileName">文件的名称</param>
    /// <param name="id">文件的ID，它一般被用来将文件映射为数据库对象</param>
    /// <returns></returns>
    public static IHasReadOnlyPreviewFile ReadOnlyPreviewFile(string coverUri, string uri, string fileName, Guid id = default)
        => new HasReadOnlyPreviewFile()
        {
            CoverUri = coverUri,
            Uri = uri,
            FileName = fileName,
            ID = id,
        };
    #endregion
    #region 创建包含可预览文件的对象
    /// <summary>
    /// 创建一个包含可预览文件的对象
    /// </summary>
    /// <param name="coverUri">封面的Uri</param>
    /// <param name="uri">本体的Uri</param>
    /// <param name="fileName">文件的名称</param>
    /// <param name="id">文件的ID，它一般被用来将文件映射为数据库对象</param>
    /// <returns></returns>
    public static IHasPreviewFile PreviewFile(string coverUri, string uri, string fileName, Guid id = default)
        => new HasPreviewFile()
        {
            CoverUri = coverUri,
            Uri = uri,
            FileName = fileName,
            ID = id,
        };
    #endregion
    #region 创建包含客户端上传文件的对象
    /// <summary>
    /// 创建一个包含客户端上传文件的对象
    /// </summary>
    /// <param name="uploadFile">要上传的文件</param>
    /// <returns></returns>
    /// <inheritdoc cref="PreviewFile(string, string, string, Guid)"/>
    public static IHasUploadFileClient UploadFileClient(string coverUri, string uri, IUploadFile uploadFile, Guid id = default)
        => new HasUploadFileClient()
        {
            CoverUri = coverUri,
            Uri = uri,
            FileName = uploadFile.FileName,
            ID = id,
            UploadFile = uploadFile
        };
    #endregion
    #region 创建包含服务端上传文件的对象
    /// <summary>
    /// 创建一个包含服务端上传文件的对象
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="UploadFileClient(string, string, IUploadFile, Guid)"/>
    public static IHasUploadFileServer UploadFileServer(string coverUri, string uri, IUploadFile uploadFile, Guid id = default)
        => new HasUploadFileServer()
        {
            CoverUri = coverUri,
            Uri = uri,
            FileName = uploadFile.FileName,
            ID = id,
            UploadFile = uploadFile
        };
    #endregion
}
