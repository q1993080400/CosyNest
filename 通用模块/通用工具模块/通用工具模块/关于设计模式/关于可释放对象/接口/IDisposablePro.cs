namespace System.Design
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以显式释放非托管资源，
    /// 并且可以告诉调用者自己是否仍然可用
    /// </summary>
    public interface IDisposablePro : IDisposable
    {
        #region 指示是否被释放
        /// <summary>
        /// 如果这个值为<see langword="true"/>，代表该对象仍然可以使用，
        /// 否则代表对象不可用，试图访问它可能会引发异常
        /// </summary>
        bool IsAvailable { get; }

        /*说明文档：
          在本接口的早期版本，这个属性名为IsRelease，
          用途是指示该对象是否被释放，但是这样出现了一个新的问题：
          有的类型没有封装非托管对象，它们在调用Dispose方法后仍然可以继续使用，
          完全是因为继承基类或实现其他接口的原因，才不得不实现本接口，
          因此作者决定使用意义更加明确的IsAvailable来代替过时的设计，
          只要IsAvailable返回true，无论有没有调用Dispose方法，都可以安全的访问本对象*/
        #endregion
    }
}
