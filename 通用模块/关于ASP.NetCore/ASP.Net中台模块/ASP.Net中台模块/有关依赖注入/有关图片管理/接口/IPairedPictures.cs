using System.IOFrancis;
using System.IOFrancis.Bit;

namespace Microsoft.AspNetCore.Html;

/// <summary>
/// 表示一个成对的图片，它由原图和缩略图组成
/// </summary>
public interface IPairedPictures
{
    #region 删除图片
    /// <summary>
    /// 将图片删除
    /// </summary>
    void Delete()
    {
        File.Delete(Uri(true, true));
        File.Delete(Uri(false, true));
    }
    #endregion
    #region 获取图片Uri
    /// <summary>
    /// 获取图片的Uri
    /// </summary>
    /// <param name="isThumbnail">如果这个值为<see langword="true"/>，
    /// 则获取缩略图，否则获取原图</param>
    /// <param name="isAbs">如果这个值为<see langword="true"/>，
    /// 则返回绝对路径，否则返回相对路径</param>
    /// <returns></returns>
    string Uri(bool isThumbnail, bool isAbs);
    #endregion
    #region 获取图片的管道
    /// <summary>
    /// 获取用来读取图片的管道
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="Uri(bool,bool)"/>
    IBitRead Read(bool isThumbnail)
        => CreateIO.FullDuplexFile(Uri(isThumbnail, true)).Read;
    #endregion
}
