using System.Diagnostics.CodeAnalysis;

namespace System.Maths;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个不可变的计量单位，
/// 它由单位和单位的值组成
/// </summary>
/// <typeparam name="Template">计量单位的类型</typeparam>
public interface IUnit<Template> : IComparable<IUnit<Template>>, IEquatable<IUnit<Template>>
    where Template : IUT
{
    #region 说明文档
    /*实现本接口时，请遵循以下规范：
      #在重写Equals方法时，应当将同时满足以下条件的两个对象视为相等：
      1.都实现IUnit<Template>接口，但类型不必完全相同
      2.双方的ValueMetric相同，不必考虑模板是否相同，
      这是因为作者认为，按照常人的思维习惯，
      1000kg和1t两个重量单位应该被视为相等
    
      问：在本接口的早期版本，曾经允许复合单位，
      如1米25厘米这种形式，那么为什么最后被放弃了？
      答：因为作者认为这种设计非常怪异，而且很少使用，
      1米25厘米的通常写法就是1.25米，而不是刚才那种形式，
      支持它只会白白增加本类型的复杂程度*/
    #endregion
    #region 返回数量为0的单位
    /// <summary>
    /// 返回数量为0的计量单位
    /// </summary>
    public static IUnit<Template> Zero { get; }
        = CreateBaseMath.UnitMetric<Template>(0);
    #endregion
    #region 运算符重载
    #region 重载一元运算符
    [return: NotNullIfNotNull("u")]
    public static IUnit<Template>? operator -(IUnit<Template>? u)
        => u?.With(-u.ValueMetric);
    #endregion
    #region 重载二元运算符
    #region 重载比较运算符
    public static bool operator >(IUnit<Template> a, IUnit<Template> b)
        => a.ValueMetric > b.ValueMetric;
    public static bool operator <(IUnit<Template> a, IUnit<Template> b)
        => a.ValueMetric < b.ValueMetric;
    #endregion
    #region 可以和任意数字运算
    public static IUnit<Template> operator *(IUnit<Template> u, Num num)
        => u.With(u.ValueMetric * num);
    public static IUnit<Template> operator /(IUnit<Template> u, Num num)
        => u.With(u.ValueMetric / num);
    #endregion
    #region 只能和另一个IUnit运算
    public static IUnit<Template> operator +(IUnit<Template> a, IUnit<Template> b)
        => a.With(a.ValueMetric + b.ValueMetric);
    public static IUnit<Template> operator -(IUnit<Template> a, IUnit<Template> b)
        => a.With(a.ValueMetric - b.ValueMetric);
    public static IUnit<Template> operator *(IUnit<Template> a, IUnit<Template> b)
        => a.With(a.ValueMetric * b.ValueMetric);
    public static Num operator /(IUnit<Template> a, IUnit<Template> b)
        => a.ValueMetric / b.ValueMetric;
    public static Num operator %(IUnit<Template> a, IUnit<Template> b)
        => a.ValueMetric % b.ValueMetric;
    #endregion
    #endregion
    #endregion
    #region 关于单位的值和模板
    #region 公制单位形式的值
    /// <summary>
    /// 返回单位换算为公制单位后的数量
    /// </summary>
    Num ValueMetric { get; }
    #endregion
    #region 本单位形式的值
    /// <summary>
    /// 获取一个元组，它描述本单位的模板，
    /// 以及单位的值
    /// </summary>
    (Template Template, Num Value) Value { get; }
    #endregion
    #endregion
    #region 转换为其他单位
    #region 返回单位的数量
    /// <summary>
    /// 将本单位转换为其他单位
    /// </summary>
    /// <param name="template">新单位的模板</param>
    /// <returns>新单位的数量</returns>
    Num Convert(Template template)
        => template.FromMetric(ValueMetric);
    #endregion
    #region 返回新单位
    /// <returns>转换后的新单位</returns>
    /// <inheritdoc cref="Convert(Template)"/>
    IUnit<Template> ConvertToUnit(Template template)
        => CreateBaseMath.UnitMetric(ValueMetric, template);
    #endregion
    #endregion
    #region 创建值不同的单位
    /// <summary>
    /// 创建一个模板相同，但值不同的新计量单位
    /// </summary>
    /// <param name="num">新计量单位的数量</param>
    /// <param name="isMetric">如果为<see langword="true"/>，
    /// 表示数量按公制单位计算，否则按本单位计算</param>
    /// <returns></returns>
    IUnit<Template> With(Num num, bool isMetric = true)
    {
        var template = Value.Template;
        return CreateBaseMath.UnitMetric(isMetric ? num : template.FromMetric(num), template);
    }
    #endregion
}
