namespace System.Threading.Tasks;

/// <summary>
/// 这个记录封装了异步编程中的一些额外功能，
/// 例如取消和进度报告
/// </summary>
/// <typeparam name="Progress">用来报告进度的对象</typeparam>
public sealed record AsynchronousPackage<Progress>
{
    #region 用来报告进度的委托
    /// <summary>
    /// 这个委托可以用来报告进度，
    /// 它的参数就是当前进度
    /// </summary>
    public Func<Progress, Task> ReportProgress { get; init; } = _ => Task.CompletedTask;
    #endregion
    #region 取消令牌
    /// <summary>
    /// 一个用来取消异步任务的令牌
    /// </summary>
    public CancellationToken CancellationToken { get; init; }
    #endregion
    #region 解构对象
    /// <summary>
    /// 将对象解构为报告进度的委托和取消令牌
    /// </summary>
    /// <param name="reportProgress">用来报告进度的委托</param>
    /// <param name="cancellationToken">取消令牌</param>
    public void Deconstruct(out Func<Progress, Task> reportProgress, out CancellationToken cancellationToken)
    {
        reportProgress = this.ReportProgress;
        cancellationToken = this.CancellationToken;
    }
    #endregion
}
