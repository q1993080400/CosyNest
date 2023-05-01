using System.Design;

namespace System.IOFrancis.Bit;

/// <summary>
/// 该类型是<see cref="IFullDuplex"/>的实现，
/// 可以视为一个全双工的二进制管道
/// </summary>
sealed class FullDuplex : Release, IFullDuplex
{
    #region 读取管道
    public IBitRead Read { get; }
    #endregion
    #region 写入管道
    public IBitWrite Write { get; }
    #endregion
    #region 释放对象
    protected override void DisposeRealize()
    {
        Read.Dispose();
        Write.Dispose();
    }
    #endregion
    #region 构造函数
    /// <inheritdoc cref="CreateIO.FullDuplex(IBitRead, IBitWrite)"/>
    public FullDuplex(IBitRead read, IBitWrite write)
    {
        Read = read;
        Write = write;
    }
    #endregion
}
