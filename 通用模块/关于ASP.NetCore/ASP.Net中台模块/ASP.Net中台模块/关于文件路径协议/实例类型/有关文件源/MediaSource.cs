namespace Microsoft.AspNetCore;

/// <summary>
/// 这个记录封装了媒体的封面和路径
/// </summary>
public sealed record MediaSource : FileSource
{
    #region 封面路径
    /// <summary>
    /// 获取媒体封面的路径，
    /// 封面可以是图片的缩略图，
    /// 也可以是视频的封面
    /// </summary>
    public required string CoverPath { get; init; }
    #endregion
    #region 删除文件
    /// <summary>
    /// 将这个文件删除，
    /// 本方法只能在服务器上调用
    /// </summary>
    public override void Delete()
    {
        File.Delete(FilePath.Op().ToLocalPath(true));
        File.Delete(CoverPath.Op().ToLocalPath(true));
    }
    #endregion
    #region 转换文件路径
    #region 转换为路径模式
    public override MediaSource ToPathMod()
    {
        var path = FilePath.Op().ToLocalPath();
        var coverPath = CoverPath.Op().ToLocalPath();
        return new()
        {
            FilePath = path,
            CoverPath = coverPath,
            TrueName = TrueName,
        };
    }
    #endregion
    #region 转换为Uri模式
    public override MediaSource ToUriMod()
    {
        var path = FilePath.Op().ToUriPath();
        var coverPath = CoverPath.Op().ToUriPath();
        return new()
        {
            FilePath = path,
            CoverPath = coverPath,
            TrueName = TrueName,
        };
    }
    #endregion
    #endregion
}
