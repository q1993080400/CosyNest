namespace System.Design;

/// <summary>
/// 本类型是实现<see cref="IInstruct"/>和<see cref="IAsyncDisposable"/>的可选基类
/// </summary>
public abstract class ReleaseAsync : IInstruct, IAsyncDisposable
{
    #region 关于释放对象
    #region 正式方法
    public async ValueTask DisposeAsync()
    {
        if (IsAvailable)
        {
            IsAvailable = false;
            GC.SuppressFinalize(this);
            await DisposeAsyncRealize();
            IsFreed = true;
        }
    }
    #endregion
    #region 模板方法
    /// <summary>
    /// 释放资源的实际操作在这个函数中
    /// </summary>
    protected abstract ValueTask DisposeAsyncRealize();
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

    /*说明文档：
      有关为什么需要本属性，以及本属性的用处，
      请参考Release类型中的同名API*/
    #endregion
}
