namespace Microsoft.AspNetCore.Html;

/// <summary>
/// 该类型是<see cref="IPairedPictures"/>的实现，
/// 可以视为一个包含原图和缩略图的图片
/// </summary>
sealed class PairedPictures : IPairedPictures
{
    #region 图片的名称
    /// <summary>
    /// 获取图片的名称
    /// </summary>
    private string Name { get; }
    #endregion
    #region 图片提供者对象
    /// <summary>
    /// 获取图片提供者对象
    /// </summary>
    private ImageProvided Provided { get; }
    #endregion
    #region 获取图片Uri
    public string Uri(bool isThumbnail, bool isAbs)
    {
        var absPath = Provided.ImagePath(Name, !isThumbnail);
        if (isAbs)
            return absPath;
        var index = absPath.IndexOf("wwwroot");
        return absPath[(index + 7)..];
    }
    #endregion
    #region 构造函数
    public PairedPictures(string name, ImageProvided provided)
    {
        Name = name;
        Provided = provided;
    }
    #endregion
}
