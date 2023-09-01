namespace System.Design;

/// <summary>
/// 该类型是一个最简单的对象池实现，
/// 它实际上不缓存对象，而是每次都创建一个新对象，
/// 在调用对象的<see cref="IDisposable.Dispose"/>方法时，会将对象销毁
/// </summary>
/// <inheritdoc cref="IPool{Obj}"/>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="create">该委托用于创建对象池中的对象</param>
sealed class Pool<Obj>(Func<Obj> create) : AutoRelease, IPool<Obj>
   where Obj : class, IDisposable
{
    #region 创建对象
    /// <summary>
    /// 该委托用于创建对象池中的对象
    /// </summary>
    private Func<Obj> Create { get; } = create;
    #endregion
    #region 获取池化对象
    public IPooledObject<Obj> Get()
        => new PooledObject<Obj>()
        {
            Get = Create()
        };
    #endregion
    #region 释放对象
    protected override void DisposeRealize()
    {

    }

    #endregion
}
