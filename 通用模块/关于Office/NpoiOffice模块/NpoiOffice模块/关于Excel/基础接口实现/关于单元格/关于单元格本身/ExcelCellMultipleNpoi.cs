using System.MathFrancis;

using NPOI.SS.Util;

namespace System.Office.Excel;

/// <summary>
/// 该类型表示由Npoi实现，
/// 包含多个单元格的单元格
/// </summary>
/// <param name="address">单元格地址</param>
/// <inheritdoc cref="ExcelCellNpoi(IExcelSheet)"/>
sealed class ExcelCellMultipleNpoi(IExcelSheet sheet, (int, int, int, int) address) : ExcelCellNpoi(sheet)
{
    #region 关于单元格本身
    #region 单元格地址
    public override (int BeginRow, int BeginCol, int EndRwo, int EndCol) Address { get; } = address;
    #endregion
    #region 单元格的值
    public override RangeValue Value
    {
        get
        {
            var (r, c) = Interface.RCCount;
            return Cells.Select(x => x.Value).ToArray(r, c);
        }
        set
        {
            var array = value.ToArray?.OfType();
            if (array is { } a1)
            {
                foreach (var (cell, v) in Cells.Zip(a1, true))
                {
                    cell!.Value = new(v);
                }
                return;
            }
            Cells.ForEach(x => x.Value = value);
        }
    }
    #endregion
    #region 单元格公式（A1格式）
    public override string? FormulaA1
    {
        get => IsMerge ? Cells.First().FormulaA1 : null;
        set => SetFormulaAssist(value, value => Cells.ForEach(x => x.FormulaA1 = value), false);
    }
    #endregion
    #endregion
    #region 关于子单元格与其他单元格
    #region 关于合并单元格
    #region 返回本单元格所在合并单元格的范围
    /// <summary>
    /// 返回一个元组，它的项分别是完全包括本单元格的合并单元格的范围，
    /// 以及本单元格在所有合并单元格中的索引，如果本单元格不在合并单元格内部，则返回(null,-1)
    /// </summary>
    private (ISizePosPixel? Address, int Index) MergeRangeMay
    {
        get
        {
            var addressMath = Interface.AddressMath;
            var (index, address, _) = SheetNpoi.MergedRegions.Select(x =>
              {
                  var br = x.MinRow;
                  var bc = x.MinColumn;
                  return CreateMath.SizePosPixel(bc, br, x.MaxColumn - bc + 1, x.MaxRow - br + 1);
              }).PackIndex().FirstOrDefault(x => x.Elements.Contains(addressMath));
            return (address, address is null ? -1 : index);
        }
    }
    #endregion
    #region 获取或写入是否合并
    public override bool IsMerge
    {
        get => MergeRangeMay.Address?.Equals(Interface.AddressMath) ?? false;
        set
        {
            var index = MergeRangeMay.Index;
            var isMerge = index >= 0;
            if (value)
            {
                if (!isMerge)
                {
                    var (br, bc, er, ec) = Address;
                    var add = new CellRangeAddress(br, er, bc, ec);
                    SheetNpoi.AddMergedRegion(add);
                }
                return;
            }
            if (isMerge)
                SheetNpoi.RemoveMergedRegion(index);
        }
    }
    #endregion
    #endregion
    #region 返回子单元格
    protected override IExcelCells IndexTemplate(int beginRow, int beginColumn, int endRow, int endColumn)
    {
        var (br, bc, _, _) = Address;
        return Sheet[beginRow + br, beginColumn + bc, endRow + br, endColumn + bc];
    }
    #endregion
    #region 替换单元格
    public override void Replace(string content, string replace)
         => throw new NotImplementedException();

    #endregion
    #endregion
    #region 构造函数
    #endregion
}
