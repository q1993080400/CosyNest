namespace System.Office.Word.Realize;

/// <summary>
/// 这个类型封装Word文本位置，
/// 实际位置与底层位置，为实现相关API提供方便
/// </summary>
public sealed record WordPos
{
    #region 文本位置
    /// <summary>
    /// 获取文本位置的开始与结束
    /// </summary>
    public (int Begin, int End) IndexText { get; }
    #endregion
    #region 实际位置
    /// <summary>
    /// 获取实际位置的开始与结束
    /// </summary>
    public (int Begin, int End) IndexActual { get; }
    #endregion
    #region 底层位置
    /// <summary>
    /// 获取底层位置的开始和结束
    /// </summary>
    public (int Begin, int End) IndexUnderlying { get; }
    #endregion
    #region 重写的ToString方法
    public override string ToString()
        => $"文本位置：{IndexText}   实际位置：{IndexActual}  底层位置：{IndexUnderlying}";
    #endregion
    #region 解构对象
    /// <summary>
    /// 将对象解构为文本位置，实际位置和底层实现位置
    /// </summary>
    /// <param name="indexText">用来接收文本位置的对象</param>
    /// <param name="indexActual">用来接收实际位置的对象</param>
    /// <param name="indexUnderlying">用来接收底层实现位置的对象</param>
    public void Deconstruct(out (int Begin, int End) indexText, out (int Begin, int End) indexActual, out (int Begin, int End) indexUnderlying)
    {
        indexText = IndexText;
        indexActual = IndexActual;
        indexUnderlying = IndexUnderlying;
    }
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="iindexText">文本位置的开始与结束</param>
    /// <param name="indexActual">实际位置开始与结束</param>
    /// <param name="indexUnderlying">底层位置的开始和结束</param>
    public WordPos((int Begin, int End) iindexText, (int Begin, int End) indexActual, (int Begin, int End) indexUnderlying)
    {
        IndexActual = indexActual;
        IndexText = iindexText;
        IndexUnderlying = indexUnderlying;
    }
    #endregion
}
