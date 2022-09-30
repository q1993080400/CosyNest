using System.IOFrancis;
using System.IOFrancis.Bit;

namespace System.Media.Play;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个播放面板，它可以用来播放音频和视频
/// </summary>
public interface IPlayPanel
{
    #region 从流中播放
    /// <summary>
    /// 从流中播放音频或视频
    /// </summary>
    /// <param name="read">用来读取音频或视频的流</param>
    /// <returns></returns>
    IPlay Play(IBitRead read);
    #endregion
    #region 从Uri中播放
    /// <summary>
    /// 从Uri中播放音频或视频
    /// </summary>
    /// <param name="uri">音频或视频所在的Uri</param>
    /// <returns></returns>
    IPlay Play(string uri)
        => Play(CreateIO.FullDuplexFile(uri, FileMode.Open).Read);

    //默认实现仅支持播放文件
    #endregion
}
