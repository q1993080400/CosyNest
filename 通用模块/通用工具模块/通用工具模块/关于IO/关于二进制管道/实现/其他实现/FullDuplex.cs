using System.Design;

namespace System.IOFrancis.Bit;

/// <summary>
/// 该类型是<see cref="IFullDuplex"/>的实现，
/// 可以视为一个全双工的二进制管道
/// </summary>
/// <inheritdoc cref="CreateIO.FullDuplex(IBitRead, IBitWrite)"/>
sealed class FullDuplex(IBitRead read, IBitWrite write) : Release, IFullDuplex
{
    #region 读取管道
    public IBitRead Read { get; } = read;
    #endregion
    #region 写入管道
    public IBitWrite Write { get; } = write;
    #endregion
    #region 释放对象
    protected override void DisposeRealize()
    {
        Read.Dispose();
        Write.Dispose();
    }
    #endregion
}
