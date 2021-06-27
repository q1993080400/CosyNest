using System.Collections.Generic;
using System.Linq;
using System.Maths;

namespace System.Mapping.Settlement
{
    /// <summary>
    /// 这个类型是所有沉降观测的抽象类型
    /// </summary>
    abstract class SettlementBase : ISettlement
    {
        #region 高程
        public abstract IUnit<IUTLength> High { get; }
        #endregion
        #region 记录
        public IUnit<IUTLength>? Recording { get; protected init; }
        #endregion
        #region 父节点
        public INode? Father { get; protected set; }
        #endregion
        #region 子节点
        /// <summary>
        /// 枚举该沉降观测点或观测站的所有后代
        /// </summary>
        protected LinkedList<SettlementBase> SonField { get; } = new();

        public IEnumerable<INode> Son => SonField;
        #endregion
        #region 移除所有后代
        public void RemoveOffspring()
        {
            SonField.ForEach(x => x.Father = null);
            SonField.Clear();
            #region 用于清除闭合差的本地函数
            static void Fun(INode? node)
            {
                switch (node)
                {
                    case SettlementPoint p:
                        if (p.ClosedDifference.Equals(IUnit<IUTLength>.Zero) || p.Closed is { })
                            return;
                        p.ClosedDifference = IUnit<IUTLength>.Zero;
                        Fun(p.Father);
                        break;
                    case SettlementObservatory o:
                        Fun(o.Father);
                        break;
                }
            }
            #endregion
            Fun(this);
        }
        #endregion
    }
}
