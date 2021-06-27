using System.IO;

namespace System.IOFrancis.Bit
{
    /// <summary>
    /// 这个接口是所有二进制管道的基接口
    /// </summary>
    public interface IBitPipeBase
    {
        #region 说明文档
        /*问：为什么本类型不实现IDisposable？
          答：以下原因分先后：

          1.本类型不同于Stream，它只有单一的读写数据的功能，
          但是本类型的派生接口是一个管道，经常出现这样一种做法：
          假设有一个IBitRead，记作A，
          另一个IBitRead转换A的输出，记作B,
          第三个IBitRead转换B的输出，记作C，
          这样一来，如何正确的释放这三个对象变成了一个非常复杂的问题，
          如果释放掉A，那么依赖它的B和C将无法工作，
          同时，ABC有可能具有不同的作用域，
          假设A和C是局部变量，B是类的字段，
          那么释放C的时候，A和B不应该被释放，因为B仍然有用处
          这个问题无法妥善解决，除非专门为了它实现自己的垃圾回收机制，
          但是这既不可能，也没有必要
        
          2.本类型及其派生接口，一般情况下不依赖于非托管代码
        
          问：那么如果本接口的实现确实需要释放，
          例如，它依赖于Stream，应该如何处理？
          答：可以在析构函数中释放掉它所依赖的非托管对象，
          这种做法会浪费一点CPU资源，但是能够保证对象最后仍然会被回收，
          而且这种操作非常安全，可以避免上一个问题所说的第1种情况*/
        #endregion
        #region 转换为流
        /// <summary>
        /// 将这个二进制管道转换为等效的<see cref="Stream"/>
        /// </summary>
        /// <returns></returns>
        Stream ToStream();

        /*实现本API请遵循以下规范：
          #本方法应该是一个纯函数，换言之，
          在本方法返回的Stream对象被释放后，IBitPipeBase对象不受影响*/
        #endregion
        #region 数据的描述
        /// <summary>
        /// 对数据的描述，如果没有描述，
        /// 则为<see langword="null"/>
        /// </summary>
        string? Describe { get; }
        #endregion
        #region 数据的格式
        /// <summary>
        /// 返回二进制数据的格式，
        /// 如果格式未知，则为<see cref="string.Empty"/>
        /// </summary>
        string Format { get; }
        #endregion
        #region 数据的总长度
        /// <summary>
        /// 返回二进制数据的总长度（以字节为单位）
        /// </summary>
        long Length { get; }
        #endregion
    }
}
