using System.Office.Excel;

namespace System.Office.Data;

/// <summary>
/// 这个记录可以作为Excel导入的参数
/// </summary>
/// <typeparam name="Data">数据的类型</typeparam>
public sealed record ExcelImportInfo<Data>
{
    #region 单个工作簿数据的上限
    /// <summary>
    /// 获取单个工作簿储存的数据的数量上限
    /// </summary>
    public int MaxBookDataCount { get; init; } = int.MaxValue;
    #endregion
    #region 根据页数返回工作簿路径
    /// <summary>
    /// 这个委托的第一个参数是工作簿的编号，
    /// 从0开始，每个工作簿最多有<see cref="MaxBookDataCount"/>条数据，
    /// 返回值是工作簿的路径
    /// </summary>
    public required Func<int, string> GetExcelBookPath { get; init; }
    #endregion
    #region 导入数据的函数
    /// <summary>
    /// 这个函数可以用来导入数据，
    /// 它的第一个项是数据所在的工作簿，
    /// 第二个项是要导入的数据，
    /// 第三个项是异步额外功能
    /// </summary>
    public required Func<IExcelBook, IEnumerable<Data>, AsynchronousPackage<Progress>, Task> Import { get; init; }
    #endregion
    #region 用来创建工作簿的函数
    /// <summary>
    /// 这个函数用来创建工作簿，
    /// 它的参数是工作簿路径，
    /// 返回值是新创建的工作簿
    /// </summary>
    public required Func<string, IExcelBook> CreateExcelBook { get; init; }
    #endregion
}
