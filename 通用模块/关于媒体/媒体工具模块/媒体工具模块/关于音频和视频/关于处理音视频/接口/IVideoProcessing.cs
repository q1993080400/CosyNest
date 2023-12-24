namespace System.Media.Play;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以执行音视频处理功能
/// </summary>
public interface IVideoProcessing
{
    #region 音视频混流
    /// <summary>
    /// 将音频和视频进行混流
    /// </summary>
    /// <param name="videoPath">视频流文件地址</param>
    /// <param name="audioPath">音频流文件地址</param>
    /// <param name="outputPath">输出地址</param>
    /// <returns></returns>
    Task MixedFlow(string videoPath, string audioPath, string outputPath);
    #endregion
    #region 格式转换
    /// <summary>
    /// 对音视频进行格式转换
    /// </summary>
    /// <param name="info">转换参数</param>
    /// <returns></returns>
    Task FormatConversion(FormatConversionInfo info);
    #endregion
    #region 视频截图
    /// <summary>
    /// 对音视频进行截图
    /// </summary>
    /// <param name="info">用来截图的参数</param>
    /// <returns></returns>
    Task Screenshot(ScreenshotInfo info);
    #endregion
    #region 获取音视频信息
    /// <summary>
    /// 获取音视频的信息
    /// </summary>
    /// <param name="mediaPath">音视频所在的文件路径</param>
    /// <returns></returns>
    Task<IMediaInfo> GetMediaInfo(string mediaPath);
    #endregion
}
