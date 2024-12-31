namespace System.DataFrancis;

/// <summary>
/// 这个静态类声明了一些字符串常量，
/// 它们可以用于Web中Input标签的Accept属性
/// </summary>
public static class WebInputAccept
{
    #region 仅接受图片
    /// <summary>
    /// 返回表示仅接受图片的Accept属性
    /// </summary>
    public const string Image = "image/*";
    #endregion
    #region 仅接受图片和PDF
    /// <summary>
    /// 返回表示仅接受图片和PDF的Accept属性
    /// </summary>
    public const string ImageAndPDF = "image/*,.pdf";
    #endregion
}
