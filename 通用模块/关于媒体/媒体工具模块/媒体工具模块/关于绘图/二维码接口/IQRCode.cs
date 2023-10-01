using System.IOFrancis;
using System.IOFrancis.Bit;
using System.IOFrancis.FileSystem;
using System.MathFrancis.Plane;

namespace System.Media.Drawing.Graphics;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来生成或者读取二维码
/// </summary>
public interface IQRCode
{
    #region 生成二维码
    /// <summary>
    /// 生成二维码
    /// </summary>
    /// <param name="content">二维码的内容</param>
    /// <param name="size">二维码的大小</param>
    /// <returns></returns>
    IBitRead Generate(string content, ISizePixel size);
    #endregion
    #region 读取二维码
    #region 指定管道
    /// <summary>
    /// 读取二维码中的内容，如果识别失败，则返回<see langword="null"/>
    /// </summary>
    /// <param name="read">用来读取二维码的管道</param>
    /// <returns></returns>
    Task<string?> Read(IBitRead read);
    #endregion
    #region 指定文件
    /// <param name="file">二维码所在的文件</param>
    /// <inheritdoc cref="Read(IBitRead)"/>
    Task<string?> Read(PathText file)
    {
        using var read = CreateIO.FileStream(file).ToBitPipe().Read;
        return Read(read);
    }
    #endregion
    #endregion
}
