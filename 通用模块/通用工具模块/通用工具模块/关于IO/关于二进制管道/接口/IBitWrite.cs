using System.Threading.Tasks;

namespace System.IOFrancis.Bit
{
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
        /// <returns></returns>
        Task Write(byte[] data);
        #endregion
    }
}
