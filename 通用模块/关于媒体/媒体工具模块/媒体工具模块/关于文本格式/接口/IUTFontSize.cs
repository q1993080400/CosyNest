using System.MathFrancis;

namespace System.Media.Drawing.Text;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个描述文本大小的单位
/// </summary>
public interface IUTFontSize : IUT
{
    #region 预设单位
    #region 返回代表磅的单位（公制单位）
    /// <summary>
    /// 返回代表磅的字体大小单位，
    /// 它是公制单位
    /// </summary>
    public static IUTFontSize PoundsMetric { get; }
    = new UTFontSize("磅", 1);
    #endregion
    #region 创建具有指定磅值的字体大小单位
    /// <summary>
    /// 创建具有指定磅值的字体大小单位
    /// </summary>
    /// <param name="num">字体大小单位的磅值</param>
    /// <returns></returns>
    public static IUnit<IUTFontSize> Pounds(Num num)
        => CreateBaseMath.Unit(num, PoundsMetric);
    #endregion
    #endregion
    #region 创建单位
    #region 指定转换方法
    /// <summary>
    /// 指定文本大小单位的名称和转换方法，
    /// 并创建这个单位
    /// </summary>
    /// <param name="name">本单位的名称</param>
    /// <param name="toMetric">从本单位转换为公制单位的委托</param>
    /// <param name="fromMetric">从公制单位转换为本单位的委托</param>
    /// <returns></returns>
    public static IUTFontSize Create(string name, Func<Num, Num> toMetric, Func<Num, Num> fromMetric)
        => new UTFontSize(name, toMetric, fromMetric);
    #endregion
    #region 指定常数
    /// <summary>
    /// 指定文本大小单位的名称，
    /// 以及和公制单位的转换比率，
    /// 然后创建这个单位
    /// </summary>
    /// <param name="name">本单位的名称</param>
    /// <param name="size">一个用来和公制单位进行换算的常数，
    /// 假设本单位为a，常数为b，公制单位为c，c=a*b，a=c/b</param>
    /// <returns></returns>
    public static IUTFontSize Create(string name, Num size)
          => new UTFontSize(name, size);
    #endregion
    #endregion
}
