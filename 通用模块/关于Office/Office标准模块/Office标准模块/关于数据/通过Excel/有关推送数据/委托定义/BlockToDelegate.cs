using System.Maths.Plane;
using System.Office.Excel;

namespace System.DataFrancis.Office.Block;

#region 用来寻找块的委托
/// <summary>
/// 该委托可以用来寻找<see cref="IData"/>在工作表中对应的单元格，
/// 它适用于将数据推送到Excel工作表的情况
/// </summary>
/// <param name="data">要寻找的数据</param>
/// <returns>该数据所在的工作表范围</returns>
public delegate ISizePosPixel BlockFind(IData data);
#endregion
#region 用来写入块的委托
/// <summary>
/// 该委托用来将属性的值写入单元格
/// </summary>
/// <param name="data">待写入的数据对象</param>
/// <param name="cells">待写入的单元格</param>
public delegate void BlockSet(IData data, IExcelCells cells);
#endregion
#region 用来过滤数据的委托
/// <summary>
/// 该委托用于过滤数据，在数据被推送到工作表之前，
/// 会先调用本方法对其进行转换
/// </summary>
/// <param name="datas">待过滤的数据</param>
/// <returns>过滤后的数据</returns>
public delegate IEnumerable<IData> BlockFilter(IEnumerable<IData> datas);
#endregion