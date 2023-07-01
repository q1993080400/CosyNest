using System.Design;
using System.Maths;

namespace System.IOFrancis.Bit;

/// <summary>
/// 这个接口是所有二进制管道的基接口
/// </summary>
public interface IBitPipeBase : IInstruct, IDisposable
{
    #region 说明文档
    /*实现本接口请遵循以下规范：
      #虽然本接口实现了IDisposable，但是如果接口实现依赖于非托管类型，
      仍然需要在析构函数中释放掉本对象，这是由于对本接口的释放按照以下规则进行：

      1.一般情况下，不需要也不推荐主动释放对象，这是因为：

      本类型不同于Stream，它只有单一的读写数据的功能，
      但是本类型的派生接口是一个管道，经常出现这样一种做法：
      假设有一个IBitRead，记作A，
      另一个IBitRead转换A的输出，记作B,
      第三个IBitRead转换B的输出，记作C，
      这样一来，如何正确的释放这三个对象变成了一个非常复杂的问题，
      如果释放掉A，那么依赖它的B和C将无法工作，
      同时，ABC有可能具有不同的作用域，
      假设A和C是局部变量，B是类的字段，
      那么释放C的时候，A和B不应该被释放，因为B仍然有用处

      2.如果确实需要立即释放掉一个管道，例如：
      通过管道读取文件，然后立即删除掉这个文件，
      只有在这种情况下，才建议显式释放本接口

      综上所述，主动和被动释放对象都具有各自的优缺点，
      主动释放可以保证对象立即被回收，但会使依赖于它的管道无法工作，
      被动释放更方便，更安全，但是也更难以控制*/
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
    /// 如果格式未知，则为<see langword="null"/>
    /// </summary>
    string? Format { get; }
    #endregion
    #region 数据的总长度
    #region 返回字节数量
    /// <summary>
    /// 返回二进制数据的总长度（以字节为单位），
    /// 如果长度未知或者没有确定长度，则为<see langword="null"/>
    /// </summary>
    long? Length { get; }
    #endregion
    #region 返回IUnit
    /// <summary>
    /// 返回二进制数据的总长度，
    /// 如果长度未知或者没有确定长度，则为<see langword="null"/>
    /// </summary>
    IUnit<IUTStorage>? LengthUnit => Length is null ? null : CreateBaseMath.Unit(Length.Value, IUTStorage.ByteMetric);
    #endregion
    #endregion
}
