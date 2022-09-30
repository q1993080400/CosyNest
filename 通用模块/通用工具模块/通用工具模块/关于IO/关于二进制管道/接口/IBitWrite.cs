namespace System.IOFrancis.Bit;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以直接写入二进制数据
/// </summary>
public interface IBitWrite : IBitPipeBase
{
    #region 写入数据
    /// <summary>
    /// 写入二进制数据
    /// </summary>
    /// <param name="data">待写入的二进制数据</param>
    /// <param name="cancellation">用于取消异步任务的令牌</param>
    /// <returns></returns>
    ValueTask Write(IEnumerable<byte> data, CancellationToken cancellation = default);

    /*实现本API请遵循以下规范：
      #虽然在外界看来并不存在这个概念，
      但是在底层应该使用缓冲，来避免一次性大量写入数据*/
    #endregion
}
