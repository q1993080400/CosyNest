namespace System.Office.Word;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都允许对Word执行查找和替换
/// </summary>
public interface IWordFindReplace
{
    #region 查找
    /// <summary>
    /// 执行查找，并返回结果
    /// </summary>
    /// <param name="condition">要查找的条件</param>
    /// <returns></returns>
    IReadOnlyCollection<IWordRange> Find(string condition);
    #endregion
}
