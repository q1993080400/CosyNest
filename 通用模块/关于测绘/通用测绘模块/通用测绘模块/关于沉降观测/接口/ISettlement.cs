using System.Maths;

namespace System.Mapping.Settlement
{
    /// <summary>
    /// 这个接口是沉降观测站和观测点的基接口
    /// </summary>
    public interface ISettlement : INode<ISettlement>
    {
        #region 返回父接口形式
        /// <summary>
        /// 返回本接口的父接口形式
        /// </summary>
        private protected INode Base => this;
        #endregion
        #region 基准点
        /// <summary>
        /// 获取沉降观测的基准点
        /// </summary>
        new ISettlementPoint Ancestors
            => (ISettlementPoint)Base;
        #endregion
        #region 高程
        /// <summary>
        /// 获取沉降观测的高程
        /// </summary>
        IUnit<IUTLength> High { get; }
        #endregion
        #region 记录
        /// <summary>
        /// 获取沉降观测的记录，
        /// 如果是基准点，则为<see langword="null"/>
        /// </summary>
        IUnit<IUTLength>? Recording { get; }
        #endregion
        #region 移除所有后代
        /// <summary>
        /// 移除该观测站或观测点的所有后代节点
        /// </summary>
        void RemoveOffspring();
        #endregion
    }
}
