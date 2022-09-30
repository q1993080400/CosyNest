namespace System.Design;

/// <summary>
/// 本类型是一个最简单的<see cref="IPooledObject{Obj}"/>实现，
/// 它实际不归还对象，而是直接调用池化对象的<see cref="IDisposable.Dispose"/>
/// </summary>
/// <typeparam name="Obj">池化对象的类型</typeparam>
sealed class PooledObject<Obj> : AutoRelease, IPooledObject<Obj>
   where Obj : class, IDisposable
{
#pragma warning disable CS8618

    #region 获取池中的对象
    public Obj Get { get; init; }
    #endregion
    #region 释放对象
    protected override void DisposeRealize()
        => Get.Dispose();
    #endregion 
}
