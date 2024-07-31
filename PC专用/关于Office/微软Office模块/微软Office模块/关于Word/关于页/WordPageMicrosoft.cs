namespace System.Office.Word;

/// <summary>
/// 这个类型是<see cref="IWordPage"/>的实现，
/// 可以视为一个Word页面
/// </summary>
/// <param name="index">页面的索引</param>
/// <param name="range">被封装的Word页面的范围，本对象的功能就是通过它实现的</param>
sealed class WordPageMicrosoft(int index, MSWordRange range) : IWordPage
{
    #region 获取页面的范围
    public IWordRange Range
        => new WordRangeMicrosoft(range);
    #endregion
    #region 页面的索引
    public int PageIndex => index;
    #endregion
}
