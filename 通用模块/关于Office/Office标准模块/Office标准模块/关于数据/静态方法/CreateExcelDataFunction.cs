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
    /// <inheritdoc cref="ExcelImportInfo{Data}"/>
    public static ExcelImport<Data> ExcelImport<Data>(ExcelImportInfo<Data> info)
        => async (datas, asynchronousPackage) =>
        {
            var newAsynchronousPackage = asynchronousPackage ?? new();
            var newCancellationToken = newAsynchronousPackage.CancellationToken;
            newCancellationToken.ThrowIfCancellationRequested();
            var paths = new List<string>();
            try
            {
                var dataCache = new LinkedList<Data>();
                await foreach (var (index, elements, isLast) in datas.PackIndex())
                {
                    dataCache.AddLast(elements);
                    if (isLast || (index + 1) % info.MaxBookDataCount is 0)
                    {
                        newCancellationToken.ThrowIfCancellationRequested();
                        var path = info.GetExcelBookPath(paths.Count);
                        paths.Add(path);
                        await using var excel = info.CreateExcelBook(path);
                        await info.Import(excel, dataCache, newAsynchronousPackage);
                        dataCache = [];
                    }
                }
            }
            catch (Exception)
            {
                foreach (var filePath in paths)
                {
                    File.Delete(filePath);
                }
                throw;
            }
            return paths;
        };
    #endregion
}
