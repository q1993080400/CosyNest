namespace System.Reflection
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以作为一个方法的签名
    /// </summary>
    public interface IMethodSignature : ISignature
    {
        #region 返回值类型
        /// <summary>
        /// 获取方法的返回值类型
        /// </summary>
        Type Return { get; }
        #endregion
    }
}
