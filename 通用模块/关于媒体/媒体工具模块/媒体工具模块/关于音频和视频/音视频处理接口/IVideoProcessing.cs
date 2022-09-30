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
    /// <param name="videoPath">视频文件</param>
    /// <param name="audioPath">音频文件</param>
    /// <inheritdoc cref="MixedFlow(IBitRead, IBitRead)"/>
    Task<IBitRead> MixedFlow(PathText videoPath, PathText audioPath);
    #endregion
    #endregion
}
