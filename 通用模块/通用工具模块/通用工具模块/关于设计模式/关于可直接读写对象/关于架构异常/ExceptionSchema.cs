namespace System.Design.Direct;

/// <summary>
/// 表示由于<see cref="ISchema"/>架构不兼容所导致的异常
/// </summary>
public abstract class ExceptionSchema : Exception
{
    #region 发生异常的属性名称
    /// <summary>
    /// 获取发生异常的属性名称
    /// </summary>
    public string PropertyName { get; }
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="propertyName">发生异常的属性名称</param>
    /// <param name="message">对错误的说明，
    /// 如果为<see langword="null"/>，则使用默认说明</param>
    public ExceptionSchema(string propertyName, string? message)
        : base(message)
    {
        PropertyName = propertyName;
    }
    #endregion
}
