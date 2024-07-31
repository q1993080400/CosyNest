namespace System.Office.Word;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个特殊的Word文档位置
/// </summary>
public interface IWordPosSpecial : IWordPos
{
    #region 位置
    /// <summary>
    /// 获取文档的特殊位置
    /// </summary>
    WordPosSpecialEnum Pos { get; }
    #endregion
}
