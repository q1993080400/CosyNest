namespace System.Media.Play;

/// <summary>
/// 这个类型是底层使用FFmpeg实现的<see cref="IAudioStream"/>
/// </summary>
/// <param name="audioStream">封装的音频流</param>
sealed class AudioStreamFFmpeg(Xabe.FFmpeg.IAudioStream audioStream)
    : MediaStreamFFmpeg(audioStream), IAudioStream
{
    #region 内部成员
    #region 默认扩展名
    protected override string DefaultExtensionName => "mp3";
    #endregion
    #endregion
}
