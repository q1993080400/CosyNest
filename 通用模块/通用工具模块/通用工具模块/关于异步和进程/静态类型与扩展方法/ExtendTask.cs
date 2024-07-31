namespace System;

/// <summary>
/// 有关进程与线程的工具类
/// </summary>
public static class ExtendTask
{
    #region 关于ValueTask
    #region 从Task转换
    /// <summary>
    /// 将<see cref="Task"/>转换为<see cref="ValueTask"/>
    /// </summary>
    /// <param name="task">待转换的<see cref="Task"/></param>
    /// <returns></returns>
    public static ValueTask ToValueTask(this Task task)
        => new(task);
    #endregion
    #region 等待ValueTask
    /// <summary>
    /// 等待一个<see cref="ValueTask"/>
    /// </summary>
    /// <param name="task">待等待的<see cref="ValueTask"/></param>
    public static void Wait(this ValueTask task)
        => task.AsTask().Wait();
    #endregion
    #endregion
    #region 关于ValueTask<T>
    #region 从Task<T>转换
    /// <summary>
    /// 将<see cref="Task{TResult}"/>转换为<see cref="ValueTask{TResult}"/>
    /// </summary>
    /// <typeparam name="TResult">返回值类型</typeparam>
    /// <param name="task">待转换的<see cref="Task{TResult}"/></param>
    /// <returns></returns>
    public static ValueTask<TResult> ToValueTask<TResult>(this Task<TResult> task)
        => new(task);
    #endregion
    #region 等待ValueTask<T>
    /// <summary>
    /// 等待一个<see cref="ValueTask{TResult}"/>，
    /// 并获取它的结果
    /// </summary>
    /// <typeparam name="Ret">返回值类型</typeparam>
    /// <param name="task">待等待的<see cref="ValueTask{TResult}"/></param>
    /// <returns></returns>
    public static Ret Result<Ret>(this ValueTask<Ret> task)
        => task.AsTask().Result;
    #endregion
    #endregion
    #region 延迟设置取消令牌
    #region 泛型方法
    /// <summary>
    /// 为一个<see cref="CancellationToken"/>注册事件，
    /// 当它被取消的时候，自动使<see cref="TaskCompletionSource{TResult}"/>进入取消状态
    /// </summary>
    /// <typeparam name="TResult">异步操作的类型</typeparam>
    /// <param name="completionSource">要取消的<see cref="TaskCompletionSource{TResult}"/></param>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    public static void SetCanceledAfter<TResult>(this TaskCompletionSource<TResult> completionSource, CancellationToken cancellationToken)
        => cancellationToken.Register(completionSource.SetCanceled);
    #endregion
    #region 非泛型方法
    /// <summary>
    /// 为一个<see cref="CancellationToken"/>注册事件，
    /// 当它被取消的时候，自动使<see cref="TaskCompletionSource"/>进入取消状态
    /// </summary>
    /// <param name="completionSource">要取消的<see cref="TaskCompletionSource"/></param>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    public static void SetCanceledAfter(this TaskCompletionSource completionSource, CancellationToken cancellationToken)
        => cancellationToken.Register(completionSource.SetCanceled);
    #endregion
    #endregion 
}
