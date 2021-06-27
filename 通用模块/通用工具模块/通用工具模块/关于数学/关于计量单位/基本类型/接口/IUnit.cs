using System.Collections.Generic;
using System.Linq;

namespace System.Maths
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个不可变的复合计量单位
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
          1000kg和1t的两个重量单位应该被视为相等*/
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
        public static IUnit<Template>? operator -(IUnit<Template>? u)
            => u?.Create(-u.ValueMetric);
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
            => u.Create(u.ValueMetric * num);
        public static IUnit<Template> operator /(IUnit<Template> u, Num num)
            => u.Create(u.ValueMetric / num);
        #endregion
        #region 只能和另一个IUnit运算
        public static IUnit<Template> operator +(IUnit<Template> a, IUnit<Template> b)
            => a.Create(a.ValueMetric + b.ValueMetric);
        public static IUnit<Template> operator -(IUnit<Template> a, IUnit<Template> b)
            => a.Create(a.ValueMetric - b.ValueMetric);
        public static IUnit<Template> operator *(IUnit<Template> a, IUnit<Template> b)
            => a.Create(a.ValueMetric * b.ValueMetric);
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
        #region 复合单位形式的值
        /// <summary>
        /// 获取一个枚举所有单位和单位的值的枚举器，
        /// 单位根据其规模从大到小排列
        /// </summary>
        IEnumerable<(Template Template, Num Value)> Value { get; }
        #endregion
        #region 返回单位的模板
        /// <summary>
        /// 枚举这个单位的模板
        /// </summary>
        IEnumerable<Template> Templates => Value.Select(x => x.Template);
        #endregion
        #endregion
        #region 关于单位转换
        #region 转换为复合单位
        /// <summary>
        /// 将本单位转换为一个值相同的复合单位
        /// </summary>
        /// <param name="templates">新复合单位的模板</param>
        /// <returns></returns>
        IUnit<Template> Convert(params Template[] templates);
        #endregion
        #region 转换为单一单位
        /// <summary>
        /// 将本单位转换为一个单一单位，并返回它的数量
        /// </summary>
        /// <param name="template">新单位的模板</param>
        /// <returns></returns>
        Num ConvertSingle(Template template)
            => template.FromMetric(ValueMetric);
        #endregion
        #endregion
        #region 创建计量单位
        /// <summary>
        /// 创建一个模板相同，但值不同的新计量单位
        /// </summary>
        /// <param name="num">新计量单位的数量，以公制单位计算</param>
        /// <returns></returns>
        protected IUnit<Template> Create(Num num);
        #endregion
    }
}
