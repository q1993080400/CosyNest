namespace System.Threading.Tasks;

/// <summary>
/// 表示一个只能显式完成的可等待对象
/// </summary>
public sealed class ExplicitTask<T>
{
    #region 公开成员
    #region 通知任务已完成
    /// <summary>
    /// 调用本方法以完成任务
    /// </summary>
    /// <param name="result">任务的返回值</param>
    public void Completed(T result)
        => (Awaiter ?? throw new NullReferenceException("无法完成尚未等待的任务")).Completed(result);
    #endregion
    #region 是否成功完成
    /// <summary>
    /// 获取任务是否成功完成
    /// </summary>
    public bool IsCompleted => Awaiter?.IsCompleted ?? false;
    #endregion
    #region 提供可等待对象
    /// <summary>
    /// 提供可等待对象，
    /// 如果该对象被等待两次，会引发异常
    /// </summary>
    /// <returns></returns>
    public ExplicitAwaiter<T> GetAwaiter()
        => Awaiter is null ?
        Awaiter = new() :
        throw new NotSupportedException($"{nameof(ExplicitTask)}对象不允许等待两次");
    #endregion
    #endregion
    #region 内部成员
    #region 缓存可等待对象
    /// <summary>
    /// 缓存可等待对象
    /// </summary>
    private ExplicitAwaiter<T>? Awaiter { get; set; }
    #endregion
    #endregion
}
