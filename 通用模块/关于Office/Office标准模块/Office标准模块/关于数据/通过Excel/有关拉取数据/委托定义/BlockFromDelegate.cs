using System.Office.Excel;

namespace System.DataFrancis.Office.Block;

#region 用来枚举数据的委托
/// <summary>
/// 该委托枚举每条数据所在的单元格
/// </summary>
/// <param name="sheet">数据所在的工作表</param>
/// <returns></returns>
public delegate IEnumerable<IExcelCells> BlockGetDatas(IExcelSheet sheet);
#endregion
#region 用来获取数据的委托
/// <summary>
/// 该委托用来从单元格中获取数据
/// </summary>
/// <param name="cell">某条数据所在的单元格</param>
/// <returns></returns>
public delegate IData BlockGet(IExcelCells cell);
#endregion
