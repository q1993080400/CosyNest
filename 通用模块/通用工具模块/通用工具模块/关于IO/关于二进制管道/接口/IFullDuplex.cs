using System.Design;

namespace System.IOFrancis.Bit;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个全双工的二进制管道，
/// 它可以同时进行读写
/// </summary>
public interface IFullDuplex : IInstruct, IDisposable
{
    #region 返回读管道
    /// <summary>
    /// 返回用来读取的管道
    /// </summary>
    IBitRead Read { get; }
    #endregion
    #region 返回写管道
    /// <summary>
    /// 返回用来写入的管道
    /// </summary>
    IBitWrite Write { get; }
    #endregion
}
