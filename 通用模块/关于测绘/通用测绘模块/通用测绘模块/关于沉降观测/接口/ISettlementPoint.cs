using System.Collections.Generic;
using System.Linq;
using System.Maths;

namespace System.Mapping.Settlement
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个沉降观测点
    /// </summary>
    public interface ISettlementPoint : ISettlement
    {
        #region 名称
        /// <summary>
        /// 获取沉降观测点的名称
        /// </summary>
        string Name { get; }
        #endregion
        #region 是否为已知点
        /// <summary>
        /// 如果该属性返回<see langword="true"/>，代表这个观测点高程已知，
        /// 它可能是基准点，附合点或闭合点，
        /// 反之则代表高程未知，需要通过计算得出
        /// </summary>
        bool IsKnown { get; }
        #endregion
        #region 是否为转点
        /// <summary>
        /// 如果这个值为<see langword="true"/>，
        /// 代表这个点为转点，它不是真正需要监测的沉降点
        /// </summary>
        bool IsIntermediary { get; }
        #endregion
        #region 父代观测站
        /// <summary>
        /// 获取后视该观测点的观测站，
        /// 如果为基准点，则为<see langword="null"/>
        /// </summary>
        new ISettlementObservatory? Father
            => (ISettlementObservatory?)Base.Father;
        #endregion
        #region 子代观测站
        /// <summary>
        /// 枚举前视此观测点的所有观测站
        /// </summary>
        new IEnumerable<ISettlementObservatory> Son
            => Base.Son.Cast<ISettlementObservatory>();
        #endregion
        #region 闭合/附合差
        /// <summary>
        /// 获取此观测点的闭合/附合差，
        /// 如果没有闭合/附合，则为0
        /// </summary>
        IUnit<IUTLength> ClosedDifference { get; }
        #endregion
        #region 添加后代观测站
        #region 可指定任何长度单位
        /// <summary>
        /// 添加一个前视此观测点的观测站
        /// </summary>
        /// <param name="recording">观测站的前视记录</param>
        /// <returns>新添加的观测站</returns>
        ISettlementObservatory Add(IUnit<IUTLength> recording);
        #endregion
        #region 只能使用沉降观测专用单位
        /// <inheritdoc cref="Add(IUnit{IUTLength})"/>
        /// <param name="recordingSettlement">前视记录，
        /// 单位是沉降观测专用单位，它等于百分之一毫米</param>
        ISettlementObservatory Add(Num recordingSettlement)
            => Add(CreateBaseMath.Unit(recordingSettlement, CreateMapping.UTSettlement));
        #endregion
        #endregion
    }
}
