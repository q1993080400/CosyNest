using System.Data.Common;
using System.Diagnostics;
using System.Text;

using Microsoft.EntityFrameworkCore.Diagnostics;

namespace System.DataFrancis.DB.EF;

#if DEBUG
/// <summary>
/// 这个类型可以记录执行的数据库脚本，
/// 并打印到控制台上
/// </summary>
sealed class RecordSQL : DbCommandInterceptor
{
    #region 公开成员
    #region 返回单例对象
    /// <summary>
    /// 返回本类型的单例对象
    /// </summary>
    public static RecordSQL Single { get; } = new();
    #endregion
    #endregion
    #region 内部成员
    #region 写入日志
    /// <summary>
    /// 向控制台写入日志
    /// </summary>
    /// <param name="command">待执行的数据库命令</param>
    /// <param name="result">执行结果，它会被原路返回</param>
    private static InterceptionResult<Ret> WriteLog<Ret>(DbCommand command, InterceptionResult<Ret> result)
    {
        var parameters = command.Parameters.OfType<DbParameter>().ToArray();
        var log = new StringBuilder();
        log.AppendLine("----------");
        log.AppendLine("SQL脚本记录开始");
        if (parameters.Length != 0)
        {
            log.AppendLine("参数：");
            log.AppendLine(parameters.Join(x => $"{x.ParameterName}={x.Value}", "，"));
        }
        log.AppendLine("脚本：");
        log.AppendLine(command.CommandText.Trim());
        log.AppendLine("----------");
        Debug.Write(log);
        return result;
    }
    #endregion
    #region 重写的方法
    public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
        => WriteLog(command, result);

    public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        => ValueTask.FromResult(WriteLog(command, result));

    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        => WriteLog(command, result);

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        => ValueTask.FromResult(WriteLog(command, result));
    #endregion
    #endregion
}
#endif
