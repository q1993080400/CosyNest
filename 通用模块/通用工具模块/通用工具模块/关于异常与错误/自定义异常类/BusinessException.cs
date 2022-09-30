using System.Runtime.Serialization;

namespace System;

/// <summary>
/// 这个类型表示一个业务异常
/// </summary>
public class BusinessException : Exception
{
    #region 说明文档
    /*问：什么是业务异常？
      答：业务异常指的是因为非技术原因所引发的异常，
      举个例子，数据验证失败所导致的异常就是业务异常
    
      问：业务异常和普通异常有什么区别？
      答：业务异常不应该使整个程序崩溃掉，它仅代表这个业务失败，
      更上层的代码应该捕获并忽略它们，然后给予用户一个提示
    
      问：根据Net规范，不应该使用异常来处理业务问题，
      因为异常捕获调用堆栈会带来性能影响，那么为什么本框架不遵循这个规范？
      答：因为异常能够停止调用堆栈上所有函数的执行，并且能够让上层代码明确知道业务已经失败，
      还可以触发数据库回滚之类的操作，它可以极大地简化业务逻辑的设计，
      如果使用返回值来表示业务失败，则每一层调用都需要检查并处理该返回值，
      与这相比，这点性能算得了什么？不要去纠结它*/
    #endregion
    #region 构造函数
    /// <inheritdoc cref="Exception(string?)"/>
    public BusinessException(string? message)
        : base(message)
    {
    }
    /// <inheritdoc cref="Exception(SerializationInfo, StreamingContext)"/>
    public BusinessException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
    /// <inheritdoc cref="Exception(string?, Exception?)"/>
    public BusinessException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
    #endregion 
}
