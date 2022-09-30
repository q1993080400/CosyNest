using System.Mapping.Settlement;
using System.Maths;

namespace System.Mapping;

/// <summary>
/// 这个静态工具类可以用来帮助创建和测绘有关的对象
/// </summary>
public static class CreateMapping
{
    #region 创建沉降观测根节点
    /// <summary>
    /// 创建一个沉降观测基准点，
    /// 它是整个沉降观测记录的第一站
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="SettlementPointRoot.SettlementPointRoot(SettlementRootBuild)"/>
    public static ISettlementPoint SettlementPointRoot(SettlementRootBuild build)
        => new SettlementPointRoot(build);
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
