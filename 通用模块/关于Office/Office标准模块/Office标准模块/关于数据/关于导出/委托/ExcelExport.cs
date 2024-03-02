using System.Office.Excel;

namespace System.Office.Data;

/// <summary>
/// 这个委托可以从Excel工作簿导出数据
/// </summary>
/// <typeparam name="Data">要导出的数据的类型</typeparam>
/// <param name="books">数据所在的Excel工作簿</param>
/// <returns>一个异步枚举器，枚举所有被导出的数据</returns>
public delegate IAsyncEnumerable<Data> ExcelExport<out Data>(IEnumerable<IExcelBook> books,
    AsynchronousPackage<Progress>? asynchronousPackage = null);