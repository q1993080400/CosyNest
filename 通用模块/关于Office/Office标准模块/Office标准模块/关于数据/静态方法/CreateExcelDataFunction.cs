namespace System.Office.Data;

/// <summary>
/// 这个静态类可以用来创建Excel数据函数
/// </summary>
public static class CreateExcelDataFunction
{
    #region 创建导入函数
    /// <summary>
    /// 根据参数，创建一个导入函数
    /// </summary>
    /// <param name="info">用来创建导入函数的参数</param>
    /// <returns></returns>
    /// <inheritdoc cref="ExcelImportInfo{Data, CategorizedData}"/>
    public static ExcelImport<Data> ExcelImport<Data, CategorizedData>(ExcelImportInfo<Data, CategorizedData> info)
        => async (datas, asynchronousPackage) =>
        {
            var (reportProgress, cancellationToken) = asynchronousPackage ?? new();
            cancellationToken.ThrowIfCancellationRequested();
            var paths = new List<string>();
            var split = info.Split(datas).PackIndex().ToArray();
            var count = split.Length;
            foreach (var (index, (path, categorizedData), _) in split)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var progress = new Progress()
                {
                    TotalCount = count,
                    CompletedCount = index
                };
                await reportProgress(progress);
                #region 报告子阶段进度的本地函数
                Task ReportSonProgress(Progress sonProgress)
                => reportProgress(progress.TotalProgress(sonProgress));
                #endregion
                await using var excel = info.CreateExcelBook(path);
                await info.Import(excel, categorizedData, new()
                {
                    CancellationToken = cancellationToken,
                    ReportProgress = ReportSonProgress
                });
                paths.Add(path);
            }
            await reportProgress(new Progress()
            {
                TotalCount = count,
                CompletedCount = count
            });
            return paths;
        };
    #endregion
}
