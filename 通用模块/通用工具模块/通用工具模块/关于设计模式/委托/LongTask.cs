namespace System.Design;

/// <summary>
/// 调用这个委托可以执行一个需要很长时间的任务
/// </summary>
/// <typeparam name="Progress">用来表示进度的对象</typeparam>
/// <param name="reportProgress">这个委托允许报告当前任务的进度</param>
/// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
/// <returns>返回这个长期任务是否成功完成</returns>

public delegate Task<bool> LongTask<Progress>(Func<Progress, Task>? reportProgress = null, CancellationToken cancellationToken = default);
