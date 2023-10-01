using System.MathFrancis;

namespace System.Media.Drawing.Text;

/// <summary>
/// 代表字体的大小单位
/// </summary>
sealed class UTFontSize : UT, IUTFontSize
{
    #region 返回单位的类型
    protected override Type UTType
        => typeof(IUTFontSize);
    #endregion
    #region 构造函数
    #region 指定名称与转换委托
    /// <summary>
    /// 指定本单位的名称和转换方法，然后初始化单位
    /// </summary>
    /// <param name="name">本单位的名称</param>
    /// <param name="toMetric">从本单位转换为公制单位的委托</param>
    /// <param name="fromMetric">从公制单位转换为本单位的委托</param>
    public UTFontSize(string name, Func<Num, Num> toMetric, Func<Num, Num> fromMetric)
        : base(name, toMetric, fromMetric)
    {
    }
    #endregion
    #region 指定名称与转换常数
    /// <summary>
    /// 指定本单位的名称，
    /// 以及一个和公制单位进行转换的常数，
    /// 然后初始化本单位
    /// </summary>
    /// <param name="name">本单位的名称</param>
    /// <param name="size">一个常数，代表1单位的本单位，等于多少公制单位，
    /// 本对象使用它进行单位转换</param>
    public UTFontSize(string name, Num size)
        : base(name, size)
    {

    }
    #endregion
    #endregion
}
