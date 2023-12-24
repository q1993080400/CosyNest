using System.IOFrancis.FileSystem;

using Xabe.FFmpeg;

namespace System.Media.Play;

/// <summary>
/// 这个类型是底层使用FFmpeg实现的<see cref="IMediaStream"/>
/// </summary>
/// <param name="stream">封装的媒体流对象</param>
abstract class MediaStreamFFmpeg(IStream stream) : IMediaStream
{
    #region 公开成员
    #region 保存媒体流
    public async Task<string> Save(string path)
    {
        var newPath = ToolPath.RefactoringPath(path, newExtension: extensionName => extensionName ?? DefaultExtensionName);
        ToolIO.CreateFather(newPath);
        await FFmpeg.Conversions.New().
            AddStream(stream).
            SetOutput(newPath).
            Start();
        return newPath;
    }
    #endregion
    #endregion
    #region 内部成员
    #region 默认扩展名
    /// <summary>
    /// 获取这个流的默认扩展名，
    /// 它影响保存流的时候，如果没有扩展名，则使用什么扩展名
    /// </summary>
    protected abstract string DefaultExtensionName { get; }
    #endregion
    #endregion
}
