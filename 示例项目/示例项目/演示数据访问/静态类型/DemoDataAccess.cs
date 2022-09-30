using System.Office.Excel;
using System.Office;
using System.DataFrancis;

namespace System;

/// <summary>
/// 这个静态类用于演示数据访问
/// </summary>
public static class DemoDataAccess
{
    /// <summary>
    /// 调用本方法以演示数据访问
    /// </summary>
    /// <returns></returns>
    public static async Task Demo()
    {
        //需求：在数据库中筛选出年龄大于18岁的实体，然后将它导入Excel

        //创建从数据库获取实体的管道

        IDataPipeFrom db = CreatePipeFrom.WebApi();
        IQueryable<Student> entitys = db.Query<Student>().Where(x => (DateTimeOffset.Now.Year - x.Birthday.Year) >= 18);

        //创建将实体推送到Excel的管道

        IExcelBook excel = CreateOfficeNpoi.ExcelBook("excel.xlsx");
        IDataPipeTo pipe = CreatePipeTo.ExcelMark(excel[0]);

        //推送完成，总共5行代码，非常简单

        await pipe.AddOrUpdate(entitys);

        //以下代码用于打开并展示导入的Excel表格
        string path = excel.Path!;
        await excel.DisposeAsync();
        ToolThread.Open(path);
    }
}
