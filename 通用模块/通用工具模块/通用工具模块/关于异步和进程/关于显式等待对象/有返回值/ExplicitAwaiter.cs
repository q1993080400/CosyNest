using System.Runtime.CompilerServices;

namespace System.Threading.Tasks;

/// <summary>
/// 表示一个只能显式完成的可等待对象的awaiter
/// </summary>
public sealed class ExplicitAwaiter<T> : INotifyCompletion
{
    #region 公开成员
    #region 是否成功完成
    /// <summary>
    /// 获取任务是否成功完成
    /// </summary>
    public bool IsCompleted { get; private set; }
    #endregion
    #region 注册延续任务
    #region 保存延续任务
    /// <summary>
    /// 这个属性保存延续任务的委托
    /// </summary>
    private Action? Continuation { get; set; }
    #endregion
    #region 正式方法
    public void OnCompleted(Action continuation)
    {
        Continuation = continuation;
    }
    #endregion 
    #endregion
    #region 获取结果
    /// <summary>
    /// 获取可等待对象的结果
    /// </summary>
    public T? GetResult()
        => IsCompleted ?
        Result :
        throw new NotSupportedException("任务尚未完成，无法获取结果");
    #endregion
    #endregion
    #region 内部成员
    #region 完成任务
    /// <summary>
    /// 调用本方法以完成任务
    /// </summary>
    /// <param name="result">任务的返回值</param>
    internal void Completed(T result)
    {
        if (IsCompleted)
            return;
        IsCompleted = true;
        Result = result;
        Continuation?.Invoke();
    }
    #endregion
    #region 结果返回值
    /// <summary>
    /// 这个属性可以用来获取返回结果
    /// </summary>
    private T? Result { get; set; }
    #endregion
    #endregion
}
