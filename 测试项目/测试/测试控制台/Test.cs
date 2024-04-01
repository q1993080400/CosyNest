using System.IOFrancis;
using System.IOFrancis.FileSystem;
using System.MathFrancis;
using System.Office;
using System.Office.Excel;

namespace System;

/// <summary>
/// 这个类型仅用于测试
/// </summary>
static class Test
{
    #region 补充资料
    /// <summary>
    /// 补充资料的不足部分
    /// </summary>
    /// <param name="excelPath">基准日期所在的工作簿路径</param>
    /// <param name="outputPath">输出文件夹路径</param>
    /// <returns></returns>
    public static async Task AdditionalInfo(string excelPath, string outputPath)
    {
        var baseDate = await BaseDate(excelPath).OrderBy(x => x).ToListAsync();
        var son = CreateIO.Directory(outputPath).Son.Select(x => x.Path).
            Where(CreateOfficeEPPlus.SupportExcel.IsCompatible).ToArray();
        await Parallel.ForEachAsync(son, async (path, _) => await Additional(path, baseDate));
    }
    #endregion
    #region 枚举基准日期
    /// <summary>
    /// 枚举基准日期
    /// </summary>
    /// <param name="excelPath">基准日期所在的工作簿</param>
    /// <returns></returns>
    private static async IAsyncEnumerable<DateTime> BaseDate(string excelPath)
    {
        await using var excel = CreateOfficeEPPlus.ExcelBook(excelPath);
        foreach (var sheet in excel.SheetManage)
        {
            if (DateTime.TryParse(sheet.Name, out var date))
                yield return date;
        }
    }
    #endregion
    #region 遍历并补充
    /// <summary>
    /// 补充单个工作簿的资料次数
    /// </summary>
    /// <param name="exclePath">工作簿路径</param>
    /// <param name="baseDate">基准日期</param>
    /// <returns></returns>
    private static async Task Additional(string exclePath, IEnumerable<DateTime> baseDate)
    {
        await using var excel = CreateOfficeEPPlus.ExcelBook(exclePath);
        var minBaseDate = baseDate.Min();
        var dates = excel.SheetManage.Select(x =>
        {
            _ = DateTime.TryParse(x.Name, out var date);
            return date;
        }).Where(x => x >= minBaseDate).Order().ToArray();
        var enumeratorDate = dates.To<IList<DateTime>>().GetEnumerator();
        enumeratorDate.MoveNext();
        var min = dates[0];
        foreach (var item in dates)
        {
            var need = baseDate.Where(x => x > min && x < item).ToArray();
            if (need.Length > 0)
            {
                Interpolation(excel, min, item, need);
            }
            min = item;
        }
        var sheets = excel.SheetManage.Where(x => DateTime.TryParse(x.Name, out _)).Index().ToArray();
        var excelName = ToolPath.SplitFilePath(excel.Path!).Simple;
        foreach (var (sheet, index) in sheets)
        {
            sheet[1, 8].Value = $"第 {index + 1} 次";
            if (index > 0)
            {
                var maxRow = 0;
                var error = 0;
                var lastName = sheets[index - 1].Elements.Name;
                for (int row = 4; true; row++)
                {
                    if (error >= 10)
                        break;
                    var cell = sheet[row, 1];
                    if (cell.Value.ToDouble is null)
                    {
                        error++;
                        continue;
                    }
                    var r = row + 1;
                    sheet[row, 2].FormulaA1 = $"B{r}-'{lastName}'!B{r}";
                    sheet[row, 3].FormulaA1 = $"C{r}+'{lastName}'!D{r}";
                    sheet[row, 4].FormulaA1 = $"C{r}/(B2-'{lastName}'!B2)";
                    maxRow = row + 1;
                }
                var series = sheet.ChartManage.Single().Series.Single();
                series.XData = $"'{lastName}'!$D$5:$D${maxRow}";
                series.YData = $"='{lastName}'!$A$5:$A${maxRow}";
                series.Name = excelName;
            }
        }
    }
    #endregion
    #region 插值资料
    /// <summary>
    /// 插值资料
    /// </summary>
    /// <param name="book">工作簿</param>
    /// <param name="min">最小日期</param>
    /// <param name="max">最大日期</param>
    /// <param name="middle">中间日期</param>
    private static void Interpolation(IExcelBook book, DateTime min, DateTime max, IEnumerable<DateTime> middle)
    {
        #region 格式化本地函数
        static string Format(DateTime date)
            => date.ToString("yyyy.M.d");
        #endregion
        var sheetManage = book.SheetManage;
        var minSheet = sheetManage.GetSheetOrNull(Format(min)) ?? throw new Exception($"在{book.Path}中未找到名为{min}的工作表");
        var maxSheet = sheetManage.GetSheetOrNull(Format(max)) ?? throw new Exception($"在{book.Path}中未找到名为{max}的工作表");
        var rand = CreateBaseMath.RandomShared;
        var zip = middle.Zip(rand.RandDistribute(1, middle.Count() + 1, 1.5)).ToArray();
        var index = minSheet.Index + 1;
        var column = 1;
        var laseDate = Format(min);
        foreach (var (date, weight) in zip)
        {
            sheetManage.Add(minSheet, index);
            var sheet = sheetManage.GetSheet(index);
            sheet.Name = Format(date);
            sheet[1, 1].Value = date.ToString("D");
            var error = 0;
            for (int row = 4; true; row++)
            {
                if (error >= 10)
                    break;
                var displacement = sheet[row, column];
                if (displacement.Value.ToDouble is null)
                {
                    error++;
                    continue;
                }
                #region 本地函数
                double Fun(IExcelSheet sheet)
                    => sheet[row, column].Value.ToDouble ??
                    throw new Exception($"{sheet.Name}R{row}C{column}出现问题");
                #endregion
                var currentValue = Fun(sheetManage.GetSheet(laseDate));
                var maxValue = Fun(maxSheet);
                var minValue = Fun(minSheet);
                displacement.Value = currentValue +
                    (double)weight * (maxValue - minValue);
            }
            index++;
            laseDate = sheet.Name;
        }
    }
    #endregion
}
