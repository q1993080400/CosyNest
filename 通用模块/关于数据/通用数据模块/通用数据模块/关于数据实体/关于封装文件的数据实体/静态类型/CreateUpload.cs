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
    /// <returns></returns>
    public static IHasReadOnlyPreviewFile ReadOnlyPreviewFile(string coverUri, string uri, string fileName)
        => new HasReadOnlyPreviewFile(coverUri, uri, fileName);
    #endregion
    #region 创建包含可预览文件的对象
    /// <summary>
    /// 创建一个包含可预览文件的对象
    /// </summary>
    /// <param name="coverUri">封面的Uri</param>
    /// <param name="uri">本体的Uri</param>
    /// <param name="fileName">文件的名称</param>
    /// <returns></returns>
    public static IHasPreviewFile PreviewFile(string coverUri, string uri, string fileName)
        => new HasPreviewFile(coverUri, uri, fileName);
    #endregion
    #region 创建包含上传文件的对象
    /// <summary>
    /// 创建一个包含上传文件的对象
    /// </summary>
    /// <param name="uploadFile">要上传的文件</param>
    /// <returns></returns>
    /// <inheritdoc cref="PreviewFile(string, string, string)"/>
    public static IHasUploadFile UploadFile(string coverUri, string uri, IUploadFile uploadFile)
        => new HasUploadFile()
        {
            CoverUri = coverUri,
            Uri = uri,
            UploadFile = uploadFile
        };
    #endregion
}
