namespace System.Design;

/// <summary>
/// 本类型是实现<see cref="IInstruct"/>和<see cref="IDisposable"/>的可选基类
/// </summary>
public abstract class Release : IInstruct, IDisposable
{
    #region 关于释放对象
    #region 正式方法
    public void Dispose()
    {
        if (IsAvailable)
        {
            IsAvailable = false;
            GC.SuppressFinalize(this);
            DisposeRealize();
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
}
