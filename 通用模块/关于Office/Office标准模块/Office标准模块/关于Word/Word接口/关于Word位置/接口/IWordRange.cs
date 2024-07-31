namespace System.Office.Word;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个Word范围，
/// 它拥有固定的开始和结束
/// </summary>
public interface IWordRange : IWordPos
{
    #region 范围开始
    /// <summary>
    /// 获取范围的开始
    /// </summary>
    int Start { get; }
    #endregion
    #region 范围结束
    /// <summary>
    /// 获取范围的结束
    /// </summary>
    int End { get; }
    #endregion
    #region 范围的长度
    /// <summary>
    /// 获取范围的长度
    /// </summary>
    int Length { get; }
    #endregion
    #region 范围的文本
    /// <summary>
    /// 返回范围的文本
    /// </summary>
    string Text { get; }
    #endregion
    #region 获取范围内所有图片
    /// <summary>
    /// 获取范围内所有图片
    /// </summary>
    IReadOnlyCollection<IWordImage> Images { get; }
    #endregion
    #region 获取子范围
    /// <summary>
    /// 获取这个范围的子范围
    /// </summary>
    /// <param name="range">标识子范围的范围</param>
    /// <returns></returns>
    IWordRange this[Range range] { get; }
    #endregion
}
