using System.DataFrancis;
using System.NetFrancis;

namespace System;

/// <summary>
/// 这个静态类可以用来创建从数据源拉取数据的管道
/// </summary>
public class CreatePipeFrom
{
    #region 从数据库拉取
    /// <summary>
    /// 获取从数据库拉取数据的管道
    /// </summary>
    /// <returns></returns>
    public static IDataPipeFrom DB()
        => CreateEFCoreDB.Pipe<DB>();
    #endregion
    #region 获取从WebApi拉取数据的管道
    public static IDataPipeFrom WebApi()
    {
        static async IAsyncEnumerable<Student> Fun()
        {
            var students = await (await CreateNet.HttpClientShared.Request("http://localhost/Student")).Content.ToObject<Student[]>();
            foreach (var item in students!)
            {
                yield return item;
            }
        }
        return CreateDataObj.PipeFromFactory(() => Fun().Fit().AsQueryable());
    }
    #endregion
}
