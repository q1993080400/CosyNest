namespace System.Geography;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个行政区划
/// </summary>
public interface IAdministrativeArea
{
    #region 行政区
    /// <summary>
    /// 枚举行政区以及它的所有上级，
    /// 这个集合的第一个元素是国家，第二个元素（如果有）是一级行政区，
    /// 第三个元素是二级行政区，依此类推
    /// </summary>
    IReadOnlyList<string> Area { get; }
    #endregion
    #region 行政区名字
    /// <summary>
    /// 获取这个行政区的名称
    /// </summary>
    string Name => Area[^1];
    #endregion
    #region 行政级别
    /// <summary>
    /// 获取行政区的级别，从0开始，
    /// 数字越小级别越高
    /// </summary>
    int Level => Area.Count - 1;
    #endregion
}
