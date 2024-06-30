namespace System.Office.Data;

/// <summary>
/// 这个委托可以将数据导入Excel工作簿
/// </summary>
/// <typeparam name="Data">要导入的数据类型</typeparam>
/// <param name="datas">要导入的数据</param>
/// <param name="asynchronousPackage">额外异步功能</param>
/// <remarks>一个集合，它指示导入的工作簿所在的路径</remarks>
public delegate Task<IReadOnlyList<string>> ExcelImport<in Data>(IAsyncEnumerable<Data> datas,
    AsynchronousPackage<Progress>? asynchronousPackage = null);