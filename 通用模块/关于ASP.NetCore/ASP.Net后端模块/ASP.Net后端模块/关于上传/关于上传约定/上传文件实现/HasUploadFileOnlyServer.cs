namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录是一个仅供服务端专用的<see cref="IHasUploadFile"/>实现，
/// 它没有<see cref="CoverUri"/>和<see cref="Uri"/>属性，
/// 因为刚刚上传到服务器，尚未保存的文件没有Uri
/// </summary>
/// <param name="UploadFile">要上传的文件</param>
sealed record HasUploadFileOnlyServer(IUploadFile UploadFile) : IHasUploadFile
{
    #region 封面Uri
    public string CoverUri => "该文件尚未保存到服务器，不能获取它的Uri或封面Uri";
    #endregion
    #region 本体Uri
    public string Uri => "该文件尚未保存到服务器，不能获取它的Uri或封面Uri";
    #endregion
    #region 文件名
    public string FileName => UploadFile.FileName;
    #endregion
    #region 是否启用对象
    public bool IsEnable => true;
    #endregion
    #region 取消选择
    public void Cancel()
        => throw new NotImplementedException($"这个实现专门用于服务端，不支持这个功能");
    #endregion
}
