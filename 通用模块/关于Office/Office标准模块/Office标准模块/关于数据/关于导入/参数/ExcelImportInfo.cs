using System.Office.Excel;

namespace System.Office.Data;

/// <summary>
/// 这个记录可以作为Excel导入的参数
/// </summary>
/// <typeparam name="Data">数据的类型</typeparam>
/// <typeparam name="CategorizedData">经过归类和转换的数据的类型，
/// 它经过了初步的清洗，可能比原始数据更适合用来导入</typeparam>
public sealed record ExcelImportInfo<Data, CategorizedData>
{
    #region 拆分数据的函数
    /// <summary>
    /// 这个函数输入数据集，输出一个元组集合，
    /// 它的项分别是用来存放数据的Excel工作簿的路径，
    /// 以及这个工作簿所对应的数据
    /// </summary>
    public required Func<IEnumerable<Data>, IEnumerable<(string ExcelPath, IEnumerable<CategorizedData> Datas)>> Split { get; init; }
    #endregion
    #region 导入数据的函数
    /// <summary>
    /// 这个函数可以用来导入数据，
    /// 它的第一个项是数据所在的工作簿，
    /// 第二个项是要导入的数据，
    /// 第三个项是异步额外功能
    /// </summary>
    public required Func<IExcelBook, IEnumerable<CategorizedData>, AsynchronousPackage<Progress>, Task> Import { get; init; }
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
