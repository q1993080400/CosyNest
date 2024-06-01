namespace System.Design;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来阻塞当前线程，通过它可以实现限流算法等功能
/// </summary>
public interface IBlock : IDisposable
{
    /// <summary>
    /// 返回一个<see cref="Task"/>，
    /// 它可以阻塞当前线程
    /// </summary>
    /// <returns>一个元组，它的第一个项如果为<see langword="true"/>，
    /// 表示无需阻塞线程，可以立即完成等待，
    /// 第二个项是一个用来阻塞当前线程的<see cref="Task"/></returns>
    (bool Complete, Task Wait) Block();
}