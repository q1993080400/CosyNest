using System.Maths;
using System.Maths.Tree;

namespace System.Mapping.Settlement;

/// <summary>
/// 这个接口是沉降观测站和观测点的基接口
/// </summary>
public interface ISettlement : INode<ISettlement>
{
    #region 说明文档
    /*问：本接口一开始被设计为不可变，
      那么为什么最后记录可以被修改，而且可以移除自己的后代？
      答：这是为了业务而做出的妥协，因为在实际使用中，
      用户确实经常需要修改和移除已经存在的沉降计算站，
      因此作者做出以下调整，在满足该需求的情况下将副作用降低到最小：

      #允许一次性移除某一站沉降观测的所有后代，
      但是不允许仅移除一部分，这样一来移除操作不会影响本节点和祖先节点，
      将对沉降计算树的影响减少到最小

      #允许修改记录，但是不允许修改点的名称，
      这个操作只会影响高程，不会影响沉降计算树的结构*/
    #endregion
    #region 获取观测方向
    /// <summary>
    /// 获取观测的方向，
    /// 也就是它是否是按照正常的流程，从已知点连接过来的
    /// </summary>
    SettlementDirection Direction { get; }
    #endregion
    #region 返回父接口形式
    /// <summary>
    /// 返回本接口的父接口形式
    /// </summary>
    private protected INode Base => this;
    #endregion
    #region 返回起始点
    /// <summary>
    /// 返回沉降观测的起始点
    /// </summary>
    new ISettlementPoint Ancestors
        => (ISettlementPoint)Base.Ancestors;
    #endregion
    #region 高程
    /// <summary>
    /// 获取沉降观测的高程
    /// </summary>
    IUnit<IUTLength> High { get; }
    #endregion
    #region 记录
    /// <summary>
    /// 获取或设置沉降观测的记录，
    /// 如果是基准点，则为<see langword="null"/>
    /// </summary>
    IUnit<IUTLength>? Recording { get; set; }

    /*实现本API请遵循以下规范：
      #起始点没有记录，它的Recording必然为null

      #在该属性不为null的情况下，禁止重复写入null，这毫无意义

      #在写入这个属性后，所有与这个点相关的点的高程，
      闭合差等数据都应该同步发生变化*/
    #endregion
}
