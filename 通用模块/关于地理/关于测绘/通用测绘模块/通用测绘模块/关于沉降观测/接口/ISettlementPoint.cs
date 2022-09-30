using System.Maths;

namespace System.Mapping.Settlement;

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
    #region 关于转点
    #region 是否为转点
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 代表这个点为转点，它不是真正需要监测的沉降点
    /// </summary>
    bool IsIntermediary { get; }
    #endregion
    #region 创建转点名称
    /// <summary>
    /// 创建一个转点的名称
    /// </summary>
    /// <param name="index">转点的编号</param>
    /// <returns></returns>
    string CreateTPName(int index);

    /*问：为什么需要本方法？转点的名称不是一般使用TP吗？
      答：因为要考虑到有的人可能与众不同，
      思维异于常人难道就不能使用本框架吗？

      注意：C#10会推出一个新功能，
      允许接口声明静态虚方法，在这个功能推出后，
      作者会根据该功能的细节对本方法进行评估，
      如果合适，则会将其重构，因为本方法确实更适合声明为静态方法，
      它作为实例方法非常奇怪*/
    #endregion
    #endregion
    #region 父代观测站
    /// <summary>
    /// 获取后视该观测点的观测站，
    /// 如果为基准点，则为<see langword="null"/>
    /// </summary>
    new ISettlementObservatory? Father
        => (ISettlementObservatory?)Base.Father;
    #endregion
    #region 关于子节点
    #region 后代观测站
    /// <summary>
    /// 枚举前视此观测点的所有观测站
    /// </summary>
    new IEnumerable<ISettlementObservatory> Son
        => Base.Son.Cast<ISettlementObservatory>();
    #endregion
    #region 移除所有后代
    /// <summary>
    /// 移除该观测站的所有后代节点
    /// </summary>
    void RemoveOffspring();
    #endregion
    #region 添加下一站观测
    #region 说明文档
    /*实现本API请遵循以下规范：
     #在添加观测点时，应根据名称自动进行判断，
     如果添加的是高程已知的附合或闭合点，应进行特殊处理*/
    #endregion
    #region 可指定任何长度单位
    /// <summary>
    /// 添加连接至本观测点的下一站观测，它包含前视和后视
    /// </summary>
    /// <param name="frontRecording">观测站的前视记录</param>
    /// <param name="behindName">后视观测点的名称</param>
    /// <param name="behindRecording">观测点的后视记录</param>
    /// <returns>新添加的观测点，如果需要获取它的观测站， 可通过<see cref="Father"/>属性</returns>
    ISettlementPoint Add(IUnit<IUTLength> frontRecording, string behindName, IUnit<IUTLength> behindRecording);
    #endregion
    #region 只能使用沉降观测专用单位
    /// <param name="frontRecording">观测站的前视记录，
    /// 单位是沉降观测专用单位，它等于百分之一毫米</param>
    /// <param name="behindRecording">观测点的后视记录，
    /// 单位是沉降观测专用单位，它等于百分之一毫米</param>
    /// <inheritdoc cref="Add(IUnit{IUTLength}, string, IUnit{IUTLength})"/>
    ISettlementPoint Add(Num frontRecording, string behindName, Num behindRecording)
    {
        #region 本地函数
        static IUnit<IUTLength> Fun(Num num)
            => CreateBaseMath.Unit(num, CreateMapping.UTSettlement);
        #endregion
        return Add(Fun(frontRecording), behindName, Fun(behindRecording));
    }
    #endregion
    #endregion
    #endregion
    #region 关于闭合/附合
    #region 闭合/附合点
    /// <summary>
    /// 返回与这个点闭合或附合的点，
    /// 如果没有闭合或附合，则为<see langword="null"/>
    /// </summary>
    ISettlementPoint? Closed { get; }
    #endregion
    #region 闭合/附合差
    /// <summary>
    /// 获取此观测点的闭合/附合差，
    /// 如果没有闭合/附合，则为0
    /// </summary>
    IUnit<IUTLength> ClosedDifference { get; }
    #endregion
    #endregion
}
