using System.Maths.Plane;

namespace System.DataFrancis.Office.Block;

/// <summary>
/// 该类型是一个数据地图，
/// 它描述数据在工作表中的排列方式
/// </summary>
public sealed class DataMap
{
#pragma warning disable CS8618

    #region 对数据位置的描述
    #region 数据的界限
    /// <summary>
    /// 指示第一条数据的界限，
    /// 也就是完全包括该数据，并与其他数据隔离开的单元格，
    /// 该属性是通过数据地图处理数据时，唯一一个必填信息
    /// </summary>
    public ISizePosPixel Boundary { get; init; }
    #endregion
    #region 每一列的范围
    /// <summary>
    /// 这个字典的键是列的名称，
    /// 值是每一列所在的位置，该位置是相对位置，
    /// 且以第一条数据为准
    /// </summary>
    public IReadOnlyDictionary<string, ISizePosPixel> ColumnBoundary { get; init; }

    /*问：如果将这个字典的值重构为DataMap，
      也就是将本对象设计为递归对象，就能够处理更加复杂的数据结构，
      那么，为什么本框架不这么设计？
      答：这是因为本框架本来就不推荐使用Excel处理复杂数据，
      这是费力不讨好的事情，如果有这个需求，请使用更加专业的工具*/
    #endregion
    #region 是否为垂直排列
    /// <summary>
    /// 如果为<see langword="true"/>，
    /// 代表数据是垂直排列的，否则代表水平排列
    /// </summary>
    public bool IsVertical { get; init; } = true;
    #endregion
    #region 是否垂直排列元素
    /// <summary>
    /// 当本对象的列是一个集合时，
    /// 如果该属性为<see  langword="true"/>，
    /// 则垂直排列集合中的元素，否则水平排列
    /// </summary>
    public bool IsVerticalElement { get; init; } = true;
    #endregion
    #endregion
    #region 返回最大可容纳集合元素数量
    private int? MaxElementsCountFiled;

    /// <summary>
    /// 返回最大可容纳的集合元素数量，
    /// 如果数据的某一列为集合，且元素数量超过了这个属性，
    /// 则块很有可能容纳不下它，此时需要使用<see cref="BlockFilter"/>对数据进行拆分
    /// </summary>
    public int MaxElementsCount
        => MaxElementsCountFiled ??=
        ColumnBoundary.Values.Max(x =>
        {
            var (h, v) = x;
            return IsVerticalElement ? v : h;
        });

    /*问：本方法的作用是什么？
      答：本方法为拆分数据提供帮助，
      举例说明，数据的某一列是一个集合，它有2个元素，
      但是，在块中分配给这一列的只有1行1列总共1个单元格，
      因此数据就会溢出，此时需要将数据拆分成两条不同的数据*/
    #endregion
    #region 返回第N条数据的位置
    #region 返回数据位置
    /// <summary>
    /// 返回第N条数据的绝对位置
    /// </summary>
    /// <param name="index">数据的索引，从0开始</param>
    /// <returns></returns>
    public ISizePosPixel GetPosition(int index)
        => ToolPlane.Arranged(Boundary.ToSizePos(), index + 1, IsVertical, !IsVertical).
            ToSizePosPixel();
    #endregion
    #region 返回列位置
    #region 根据索引
    /// <summary>
    /// 返回第N条数据的所有列的列名和绝对位置
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="GetPosition(int)"/>
    public IEnumerable<(string ColumnName, ISizePosPixel Position)> GetColumnPosition(int index)
        => GetColumnPosition(GetPosition(index).FirstPixel);
    #endregion
    #region 根据左上角位置
    /// <summary>
    /// 根据界限的左上角位置，
    /// 返回数据的所有列的列名和绝对位置
    /// </summary>
    /// <param name="topLeft">界限左上角位置的坐标</param>
    /// <returns></returns>
    public IEnumerable<(string ColumnName, ISizePosPixel Position)> GetColumnPosition(IPoint topLeft)
    {
        foreach (var (name, pos) in ColumnBoundary)
        {
            var point = pos.FirstPixel.ToAbs(topLeft);
            yield return (name, pos.Transform(point));
        }
    }
    #endregion
    #endregion
    #endregion
}
