using System.IOFrancis.FileSystem;

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
    #region 创建特殊可上传文件
    /// <summary>
    /// 创建一个特殊的可上传文件，
    /// 它只用于在前端和后端之间作为中介
    /// </summary>
    /// <param name="previewFile">函数会从这个可预览文件中复制信息到新创建的对象</param>
    /// <returns></returns>
    public static IHasUploadFileMiddle UploadFileMiddle(IHasPreviewFile previewFile)
        => new HasUploadFileMiddle()
        {
            CoverUri = previewFile.CoverUri,
            FileName = previewFile.FileName,
            ID = previewFile.ID,
            Uri = previewFile.Uri,
            FileID = Guid.CreateVersion7()
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
    #region 创建共享上传文件对象
    /// <summary>
    /// 创建一个可以同时在服务端和客户端使用的可预览文件，
    /// 它只在某些特殊的情况下有用
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="UploadFileClient(string, string, IUploadFile, Guid)"/>
    public static IHasUploadFileFusion UploadFileFusion(string coverUri, string uri, IUploadFile uploadFile, Guid id = default)
        => new HasUploadFileFusion()
        {
            CoverUri = coverUri,
            Uri = uri,
            FileName = uploadFile.FileName,
            ID = id,
            UploadFile = uploadFile
        };
    #endregion
    #region 创建文件物理位置
    /// <summary>
    /// 创建一个文件物理位置，
    /// 它指示某文件实际存储的地方
    /// </summary>
    /// <param name="coverPath">文件的封面路径</param>
    /// <param name="path">文件的本体路径</param>
    /// <returns></returns>
    public static IFilePosition FilePosition(FilePathInfo? coverPath, FilePathInfo path)
        => new FilePosition()
        {
            CoverPath = coverPath,
            Path = path
        };
    #endregion
    #region 创建具有ID的文件物理位置
    /// <summary>
    /// 创建一个文件物理位置，
    /// 它指示某文件实际存储的地方，
    /// 并且它还具有一个ID，
    /// 可以和其他存储文件位置的对象关联起来
    /// </summary>
    /// <param name="id">文件的ID，它通常被映射到数据库等对象的ID</param>
    /// <returns></returns>
    /// <inheritdoc cref="FilePosition(FilePathInfo?, FilePathInfo)"/>
    public static IFileObjectPosition FileObjectPosition(FilePathInfo? coverPath, FilePathInfo path, Guid id)
        => new FileObjectPosition()
        {
            CoverPath = coverPath,
            ID = id,
            Path = path
        };
    #endregion
}
