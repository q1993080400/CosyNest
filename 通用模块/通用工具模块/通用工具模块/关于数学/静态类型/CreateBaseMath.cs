using System.Maths.Tree;

namespace System.Maths;

/// <summary>
/// 这个静态类可以用来帮助创建基本数学对象
/// </summary>
public static class CreateBaseMath
{
    #region 创建Unit
    #region 指定公制单位的数量
    #region 创建指定公制单位数量的单位
    /// <summary>
    /// 创建具有指定公制单位数量的单位
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="UnitMetric{Template}(Num, Template)"/>
    public static IUnit<Template> UnitMetric<Template>(Num metricValue)
        where Template : IUT
        => Unit(metricValue, IUT.GetMetric<Template>());
    #endregion
    #region 创建具有指定模板和指定公制单位数量的单位
    /// <summary>
    /// 创建具有指定模板和指定公制单位数量的单位
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="Unit{Template}.Unit(Template, Num)"/>
    /// <inheritdoc cref="IUnit{Template}"/>
    public static IUnit<Template> UnitMetric<Template>(Num metricValue, Template template)
        where Template : IUT
        => new Unit<Template>(template, metricValue);
    #endregion
    #endregion
    #region 创建指定数量的单位
    /// <summary>
    /// 创建指定数量的单位，并返回该计量单位
    /// </summary>
    /// <param name="value">指定单位的数量，不是公制单位的数量</param>
    /// <returns></returns>
    /// <inheritdoc cref="UnitMetric{Template}(Num, Template)"/>
    public static IUnit<Template> Unit<Template>(Num value, Template template)
        where Template : IUT
        => UnitMetric(template.ToMetric(value), template);
    #endregion
    #endregion
    #region 创建IRandom
    #region 获取公用的随机数生成器
    [ThreadStatic]
    private static IRandom? RandomSharedFiled;

    /// <summary>
    /// 获取一个公用的随机数生成器，
    /// 它是线程安全的
    /// </summary>
    public static IRandom RandomShared => RandomSharedFiled ??= new Random(System.Random.Shared);
    #endregion
    #region 使用指定的种子创建
    /// <summary>
    /// 使用指定的种子创建一个随机数生成器
    /// </summary>
    /// <param name="seed">用来生成随机数的种子，
    /// 如果为<see langword="null"/>，则默认使用时间作为种子</param>
    /// <returns></returns>
    public static IRandom Random(int? seed = null)
        => new Random(seed);
    #endregion
    #endregion
    #region 创建树形结构
    #region 创建INodeContent
    /// <summary>
    /// 创建一个封装指定类型内容的树形结构节点，
    /// 并以它作为树形结构的根节点
    /// </summary>
    /// <typeparam name="Obj">树形结构节点所封装的对象类型</typeparam>
    /// <param name="root">指定根节点所封装的对象</param>
    /// <param name="getSon">这个委托传入节点封装的对象，
    /// 返回它的直接子节点所封装的对象</param>
    /// <returns>新创建的树形结构根节点</returns>
    public static INodeContent<Obj> NodeContent<Obj>(Obj root, Func<Obj, IEnumerable<Obj>> getSon)
        => new NodeContent<Obj>(null, root, getSon);
    #endregion
    #endregion 
}
