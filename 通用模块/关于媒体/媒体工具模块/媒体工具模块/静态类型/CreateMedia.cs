using System.Drawing;
using System.IOFrancis.FileSystem;
using System.MathFrancis;

namespace System.Media;

/// <summary>
/// 这个静态类可以帮助创建一些关于媒体的对象
/// </summary>
public static class CreateMedia
{
    #region 创建Color
    #region 指定16进制字符串
    /// <summary>
    /// 根据一个用16进制表示的字符串，创建一个<see cref="System.Drawing.Color"/>并返回
    /// </summary>
    /// <param name="sys16">指示RGBA值的十六进制字符串，格式为FF001122，
    /// A可以省略，默认为255</param>
    /// <returns></returns>
    public static Color Color(string sys16)
    {
        var array = Convert.FromHexString(sys16.TrimStart('#'));        //为HTML颜色字符串作优化
        if (array.Length is not (4 or 3))
            throw new ArgumentException($"{nameof(sys16)}不是合法的表示颜色的字符串");
        return System.Drawing.Color.FromArgb(array.ElementAt(3, new(255)), array[0], array[1], array[2]);
    }
    #endregion
    #region 生成随机颜色
    /// <summary>
    /// 生成透明度指定，但RGB随机的颜色
    /// </summary>
    /// <param name="alpha">颜色的透明度</param>
    /// <param name="rand">用来生成随机数的对象，如果为<see langword="null"/>，则使用一个默认对象</param>
    /// <returns></returns>
    public static Color ColorRandom(byte alpha = 255, IRandom? rand = null)
    {
        rand ??= CreateBaseMath.RandomShared;
        byte Fun()
            => (byte)rand.Rand(0, 255);
        return System.Drawing.Color.FromArgb(alpha, Fun(), Fun(), Fun());
    }
    #endregion
    #endregion
    #region 创建媒体文件类型
    /// <summary>
    /// 根据路径或扩展名，创建媒体文件类型
    /// </summary>
    /// <param name="pathOrExtended">要创建媒体文件类型的路径或扩展名</param>
    /// <returns></returns>
    public static MediumFileType MediumFileType(string pathOrExtended)
    {
        var extended = ToolPath.SplitFilePath(pathOrExtended).Extended ??
            pathOrExtended.TrimStart('.');
        return extended switch
        {
            "png" or "jpg" or "jpeg" or "gif" or "svg" or "webp" or "bmp" or "avif" => Media.MediumFileType.Image,
            "mp4" or "webm" or "mov" => Media.MediumFileType.Video,
            "mp3" or "acc" or "ogg" or "flac" => Media.MediumFileType.Audio,
            _ => Media.MediumFileType.NotMediumFile
        };
    }
    #endregion
}
