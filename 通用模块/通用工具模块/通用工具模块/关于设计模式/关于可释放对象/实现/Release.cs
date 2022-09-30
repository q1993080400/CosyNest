namespace System.Design;

/// <summary>
/// 本类型是实现<see cref="IInstruct"/>和<see cref="IDisposable"/>的可选基类
/// </summary>
public abstract class Release : IInstruct, IDisposable
{
    #region 关于释放对象
    #region 进程锁
    private object Lock { get; } = new();
    #endregion
    #region 正式方法
    public void Dispose()
    {
        lock (Lock)
        {
            if (IsAvailable)
            {
                IsAvailable = false;
                GC.SuppressFinalize(this);
                DisposeRealize();
                IsFreed = true;
            }
        }
    }
    #endregion
    #region 模板方法
    /// <summary>
    /// 释放资源的实际操作在这个函数中
    /// </summary>
    protected abstract void DisposeRealize();
    #endregion
    #endregion
    #region 指示对象是否可用
    public bool IsAvailable { get; private set; } = true;
    #endregion
    #region 指示对象是否释放完毕
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 代表释放已经执行完毕
    /// </summary>
    protected bool IsFreed { get; private set; }

    /*问：本属性有什么用处？
      它和IsAvailable有什么不同？
      答：在开始执行DisposeRealize方法后，
      IsAvailable被设置为false，此时对象实际上仍然是可用的，
      但是外界以为它不可用，这是为这种情况准备的：
      某些方法在DisposeRealize中被调用，但是它们也会被独立调用，所以需要检测对象是否可用，
      如果只使用IsAvailable的话，由于IsAvailable在方法开头就会被设置为false，
      它们会误以为对象已经被释放，会停止工作
    
      IsFreed在DisposeRealize方法被调用完毕后被设置为false，此时对象将真正不可用
    
      问：那么，为什么不只声明IsAvailable，
      然后在DisposeRealize执行完毕后将他设置为false？
      答：这是因为作者考虑到，如果DisposeRealize的执行时间较长，
      外界会误以为它没有被释放，在这种情况下很容易执行一些危险的操作，
      因此应当尽快告知外界，本对象已经不再可用，不要再执行任何代码*/
    #endregion
}
