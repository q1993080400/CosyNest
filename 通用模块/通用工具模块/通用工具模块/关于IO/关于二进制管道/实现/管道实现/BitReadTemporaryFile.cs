using System.Design;
using System.IOFrancis.FileSystem;

namespace System.IOFrancis.Bit;

/// <summary>
/// 表示一个用来读取且只能读取临时文件的管道，
/// 当这个管道被释放时，临时文件会被删除
/// </summary>
/// <remarks>
/// 使用指定的路径创建临时文件
/// </remarks>
/// <param name="path">临时文件的路径，如果它不存在，会引发异常</param>
sealed class BitReadTemporaryFile(PathText path) : AutoRelease, IBitRead
{
    #region 公开成员
    #region 释放对象
    protected override void DisposeRealize()
    {
        Stream.Dispose();
        File.Delete(Path);
    }
    #endregion
    #region 读取文件
    public IAsyncEnumerable<byte[]> Read(int bufferSize = 1024, CancellationToken cancellation = default)
    {
        if (IsRead)
            throw new NotSupportedException($"这个临时文件已经被读取，无法再次读取它");
        IsRead = true;
        var pipe = Stream.ToBitPipe();
        return pipe.Read.Read(bufferSize, cancellation);
    }
    #endregion
    #region 管道的长度
    public long? Length => Stream.Length;
    #endregion
    #region 文件的格式
    public string? Format => ToolPath.SplitPathFile(Path).Extended;
    #endregion
    #region 对管道的描述
    public string? Describe => ToolPath.SplitPathFile(Path).FullName;
    #endregion
    #region 转换为流
    public Stream ToStream()
        => Stream;
    #endregion
    #endregion
    #region 内部成员
    #region 是否被读取
    /// <summary>
    /// 获取是否已经读取该临时文件
    /// </summary>
    private bool IsRead { get; set; }
    #endregion
    #region 临时文件的路径
    /// <summary>
    /// 获取临时文件的路径
    /// </summary>
    private string Path { get; } = path;
    #endregion
    #region 读取临时文件的流
    /// <summary>
    /// 获取用来读取临时文件的流
    /// </summary>
    private Stream Stream { get; } = new FileStream(path, FileMode.Open);

    #endregion
    #endregion
}
