using System.IOFrancis;

namespace System;

/// <summary>
/// 关于IO的扩展方法
/// </summary>
public static class ExtendIO
{
    #region 关于流
    #region 读取流的全部内容
    /// <summary>
    /// 读取流中的全部内容
    /// </summary>
    /// <param name="stream">待读取内容的流</param>
    /// <returns></returns>
    public static async Task<byte[]> ReadAll(this Stream stream)
    {
        var memory = await stream.CopyToMemory();
        return memory.ToArray();
    }
    #endregion
    #region 将流复制到一个内存流
    /// <summary>
    /// 将流复制到一个内存流，并返回该内存流
    /// </summary>
    /// <param name="stream">待复制的流</param>
    /// <returns></returns>
    public static async Task<MemoryStream> CopyToMemory(this Stream stream)
    {
        stream.Reset();
        var memory = new MemoryStream();
        await stream.CopyToAsync(memory);
        memory.Reset();
        return memory;
    }
    #endregion
    #region 将流保存到文件中
    /// <summary>
    /// 将流保存到文件中
    /// </summary>
    /// <param name="stream">要读取的流</param>
    /// <param name="path">要保存到的文件路径</param>
    /// <param name="cancellationToken">一个用来取消异步操作的令牌</param>
    /// <returns></returns>
    public static async Task SaveToFile(this Stream stream, string path, CancellationToken cancellationToken = default)
    {
        if (!stream.CanRead)
            throw new NotSupportedException("这个流不可读取");
        stream.Reset();
        if (File.Exists(path))
            File.Delete(path);
        using var fileStream = CreateIO.FileStream(path, FileMode.Create);
        await stream.CopyToAsync(fileStream, cancellationToken);
    }
    #endregion
    #region 重置流
    /// <summary>
    /// 将流重置到开始位置
    /// </summary>
    /// <param name="stream">待重置的流</param>
    public static void Reset(this Stream stream)
    {
        if (stream.CanSeek && stream.Position is not 0)
            stream.Seek(0, SeekOrigin.Begin);
    }
    #endregion
    #endregion
}
