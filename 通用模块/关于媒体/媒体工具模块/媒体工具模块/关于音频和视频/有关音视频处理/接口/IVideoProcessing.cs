using System.IOFrancis.Bit;
using System.IOFrancis.FileSystem;

namespace System.Media.Play;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以执行音视频处理功能
/// </summary>
public interface IVideoProcessing
{
    #region 音视频混流
    #region 传入流
    /// <summary>
    /// 将音频和视频进行混流
    /// </summary>
    /// <param name="video">视频流</param>
    /// <param name="audio">音频流</param>
    /// <returns></returns>
    Task<IBitRead> MixedFlow(IBitRead video, IBitRead audio);
    #endregion
    #region 传入文件
    /// <param name="videoFile">视频文件</param>
    /// <param name="audioFile">音频文件</param>
    /// <inheritdoc cref="MixedFlow(IBitRead, IBitRead)"/>
    Task<IBitRead> MixedFlow(IFile videoFile, IFile audioFile);
    #endregion
    #endregion
    #region 格式转换
    #region 传入流
    /// <summary>
    /// 将一个音视频转换为另一种格式
    /// </summary>
    /// <param name="read">用来读取音视频的管道</param>
    /// <param name="currentFormat">音视频的当前格式，不带点号</param>
    /// <param name="targetPath">转换后的目标路径，它的扩展名指示格式</param>
    /// <param name="info">用于转换的参数，如果为<see langword="null"/>，则使用默认值</param>
    Task FormatConversion(IBitRead read, string currentFormat, string targetPath, FormatConversionInfo? info = null);
    #endregion
    #region 传入文件
    /// <param name="file">要转换的音视频文件</param>
    /// <inheritdoc cref="FormatConversion(IBitRead, string, string, FormatConversionInfo?)"/>
    Task FormatConversion(IFile file, string targetPath, FormatConversionInfo? info = null);
    #endregion
    #endregion
    #region 视频截图
    #region 传入流
    /// <summary>
    /// 对一个视频文件进行截图
    /// </summary>
    /// <param name="read">用来读取视频文件的管道</param>
    /// <param name="currentFormat">视频的当前格式，不带点号</param>
    /// <param name="infos">这个集合元素的第一个项是要截图的时间，第二个项是截图文件保存路径</param>
    /// <returns></returns>
    /// <inheritdoc cref="FormatConversion(IBitRead, string, string, bool, Func{decimal, Task}?, CancellationToken)"/>
    Task Screenshot(IBitRead read, string currentFormat, IEnumerable<(TimeSpan Fragment, string Path)> infos, Func<decimal, Task>? reportProgress = null, CancellationToken cancellationToken = default);
    #endregion
    #region 传入文件
    /// <param name="file">要截图的视频文件</param>
    /// <inheritdoc cref="Screenshot(IBitRead, string, IEnumerable{ValueTuple{TimeSpan, string}}, Func{decimal, Task}?, CancellationToken)"/>
    Task Screenshot(IFile file, IEnumerable<(TimeSpan Fragment, string Path)> infos, Func<decimal, Task>? reportProgress = null, CancellationToken cancellationToken = default);
    #endregion
    #endregion
}
