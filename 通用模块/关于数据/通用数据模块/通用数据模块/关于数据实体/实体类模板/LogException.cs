namespace System.DataFrancis;

/// <summary>
/// 这个实体类是一个错误记录的模板
/// </summary>
public class LogException : Entity, IWithDate
{
    #region 发生错误的时间
    public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;
    #endregion
    #region 错误消息
    /// <summary>
    /// 错误消息
    /// </summary>
    public string Message { get; set; } = "";
    #endregion
    #region 额外信息
    /// <summary>
    /// 额外信息，它储存错误信息以外的信息
    /// </summary>
    public string AdditionalMessage { get; set; } = "";
    #endregion
    #region 异常名称
    /// <summary>
    /// 发生的异常的名称
    /// </summary>
    public string Exception { get; set; } = "";
    #endregion
    #region 方法名称
    /// <summary>
    /// 发生错误的方法名称
    /// </summary>
    public string Method { get; set; } = "";
    #endregion
    #region 调用堆栈
    /// <summary>
    /// 错误调用堆栈
    /// </summary>
    public string Stack { get; set; } = "";
    #endregion
}
