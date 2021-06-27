using System.IOFrancis.Bit;
using System.IOFrancis.FileSystem;

namespace System.DrawingFrancis.Graphics
{
    /// <summary>
    /// 代表一个可以将自身输出为流的画布
    /// </summary>
    public interface ICanvasOutput : ICanvas
    {
        #region 合法输出格式
        /// <summary>
        /// 返回本画布的合法输出格式
        /// </summary>
        IFileType Format { get; }
        #endregion
        #region 输出画布
        /// <summary>
        /// 将画布输出为强类型流，并返回
        /// </summary>
        /// <param name="format">输出的格式，
        /// 如果为<see langword="null"/>，则使用默认格式</param>
        /// <exception cref="NotSupportedException"><paramref name="format"/>不是合法的格式</exception>
        /// <returns></returns>
        IBitRead Output(string? format = null);
        #endregion
    }
}
