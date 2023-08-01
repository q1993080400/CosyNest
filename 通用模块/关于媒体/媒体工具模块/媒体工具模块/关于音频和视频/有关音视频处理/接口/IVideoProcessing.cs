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
    /// <summary>
    /// 对音视频进行格式转换
    /// </summary>
    /// <param name="mediumPath">要转换的音视频文件的路径</param>
    /// <param name="targetPath">转换的目标路径</param>
    /// <param name="info">转换参数</param>
    /// <returns></returns>
    Task FormatConversion(string mediumPath, string targetPath, FormatConversionInfo? info = null);
    #endregion
    #region 视频截图
    /// <summary>
    /// 对音视频进行截图
    /// </summary>
    /// <param name="mediumPath">要截图的视频文件</param>
    /// <param name="infos">这个集合元素的第一个项是要截图的时间，第二个项是截图文件保存路径</param>
    /// <param name="reportProgress">一个用来报告进度的委托，它的参数就是当前进度</param>
    /// <param name="cancellationToken">一个用来取消异步操作的令牌</param>
    /// <returns></returns>
    Task Screenshot(string mediumPath, IEnumerable<(TimeSpan Fragment, string Path)> infos, Func<decimal, Task>? reportProgress = null, CancellationToken cancellationToken = default);
    #endregion
}
