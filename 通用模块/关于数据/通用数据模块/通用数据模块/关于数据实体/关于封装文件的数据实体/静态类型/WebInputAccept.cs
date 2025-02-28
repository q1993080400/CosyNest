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
    #region 接受全部
    /// <summary>
    /// 返回接受全部文件的Accept属性，
    /// 它同时也包含了图片和视频，
    /// 这能够让某些移动浏览器能够正确地了解到，
    /// 应该赋予用户访问相机的权限
    /// </summary>
    public const string All = "image/*,video/*,.*";
    #endregion
}
