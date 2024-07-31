namespace System.Office.Word;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个Word文档的位置
/// </summary>
public interface IWordPos
{
    #region 转换为精确位置
    /// <summary>
    /// 转换为精确位置
    /// </summary>
    /// <param name="document">位置所在的Word文档</param>
    /// <returns></returns>
    IWordRange PrecisePos(IWordDocument document);
    #endregion
}
