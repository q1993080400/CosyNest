namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型成对封装了媒体在服务器上的封面和本体路径
/// </summary>
public sealed record MediaServerPosition
{
    #region 封面路径
    /// <summary>
    /// 获取媒体封面的路径，
    /// 封面可以是图片的缩略图，
    /// 也可以是视频的封面
    /// </summary>
    public required string CoverPath { get; init; }
    #endregion
    #region 媒体路径
    /// <summary>
    /// 获取媒体本体的路径
    /// </summary>
    public required string MediaPath { get; init; }
    #endregion
    #region 删除这个媒体
    /// <summary>
    /// 删除这个媒体的封面和本体，
    /// 这个方法只能在服务器上调用
    /// </summary>
    public void Delete()
    {
        File.Delete(CoverPath);
        File.Delete(MediaPath);
    }
    #endregion
}
