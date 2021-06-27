using System.Collections.Generic;
using System.Linq;
using System.Mapping.Settlement;
using System.Maths;

namespace System.Mapping
{
    /// <summary>
    /// 这个静态工具类可以用来帮助创建和测绘有关的对象
    /// </summary>
    public static class CreateMapping
    {
        #region 创建沉降观测根节点
        #region 可指定任意长度单位
        /// <summary>
        /// 创建一个沉降观测基准点，
        /// 它是整个沉降观测记录的第一站
        /// </summary>
        /// <param name="name">基准点的名称</param>
        /// <param name="high">基准点的高程</param>
        ///  <param name="known">索引本次沉降观测中，高程已知的点的名称和高程，
        /// 如果为<see langword="null"/>，则代表除基准点外没有已知点</param>
        /// <param name="isIntermediary">通过点的名称，判断是否为转点的委托，
        /// 如果为<see langword="null"/>，则通过点前缀是否带tp判断</param>
        /// <returns></returns>
        public static ISettlementPoint SettlementPointRoot
            (string name, IUnit<IUTLength> high,
            IEnumerable<KeyValuePair<string, IUnit<IUTLength>>>? known = null,
            Func<string, bool>? isIntermediary = null)
        {
            known ??= CreateCollection.Empty(known);
            isIntermediary ??= x => x.StartsWith("tp", StringComparison.CurrentCultureIgnoreCase);
            return new SettlementPoint(name, high, known.ToDictionary(false), isIntermediary);
        }
        #endregion
        #region 只能使用米
        /// <inheritdoc cref="SettlementPointRoot(string, IUnit{IUTLength}, IEnumerable{KeyValuePair{string, IUnit{IUTLength}}}?, Func{string, bool})"/>
        /// <param name="highMetre">使用米作为单位的基准点高程</param>
        public static ISettlementPoint SettlementPointRoot
            (string name, Num highMetre,
            IEnumerable<KeyValuePair<string, IUnit<IUTLength>>>? known = null,
            Func<string, bool>? isIntermediary = null)
            => SettlementPointRoot(name, CreateBaseMath.UnitMetric<IUTLength>(highMetre), known, isIntermediary);
        #endregion
        #endregion
        #region 返回沉降观测专用长度单位
        /// <summary>
        /// 返回沉降观测专用长度单位，
        /// 它等于百分之一毫米
        /// </summary>
        /// <returns></returns>
        public static IUTLength UTSettlement { get; }
            = IUTLength.Create("沉降观测长度单位", 0.00001);
        #endregion
    }
}
