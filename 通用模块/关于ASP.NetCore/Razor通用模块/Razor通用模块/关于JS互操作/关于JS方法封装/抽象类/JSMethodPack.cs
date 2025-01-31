namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个类型是JS方法封装的基类，
/// 它可以用来简化在JS中调用Net方法的过程
/// </summary>
/// <typeparam name="Delegate">要封装的委托的类型</typeparam>
abstract class JSMethodPack<Delegate, NetPack> : IDisposable
    where Delegate : System.Delegate
    where NetPack : JSMethodPack<Delegate, NetPack>
{
    #region 公开成员
    #region 释放对象
    public void Dispose()
        => DotNetObjectReference.Dispose();
    #endregion
    #region 封装的Net对象引用
    /// <summary>
    /// 封装的Net对象引用，
    /// 它引用这个对象自己
    /// </summary>
    public DotNetObjectReference<NetPack> DotNetObjectReference { get; }
    #endregion
    #endregion
    #region 内部成员
    #region 要封装的委托
    /// <summary>
    /// 被封装的委托
    /// </summary>
    protected Delegate PackDelegate { get; }
    #endregion
    #region 获取Net封装引用的方法
    /// <summary>
    /// 创建一个Net引用封装，
    /// 它可以用来进行JS调用
    /// </summary>
    /// <returns></returns>
    protected abstract DotNetObjectReference<NetPack> CreateDotNetObjectReference();
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="delegate">要封装的委托</param>
    public JSMethodPack(Delegate @delegate)
    {
        DotNetObjectReference = CreateDotNetObjectReference();
        PackDelegate = @delegate;
    }
    #endregion
}
