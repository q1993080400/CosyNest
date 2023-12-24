namespace System.Media.Play;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个媒体流
/// </summary>
public interface IMediaStream
{
    #region 保存媒体流
    /// <summary>
    /// 将媒体流保存到文件中
    /// </summary>
    /// <param name="path">要保存的文件路径，
    /// 路径的扩展名可以省略，在这种情况下，
    /// 不改变原有的格式，并自动加上扩展名</param>
    /// <returns>保存后的文件所在的路径，考虑到自动加上扩展名的情况，
    /// 它不一定和<paramref name="path"/>等同</returns>
    Task<string> Save(string path);
    #endregion
}
