namespace System.Media.Drawing.PDF;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个PDF中的图片
/// </summary>
public interface IPDFImage
{
    #region 图片格式
    /// <summary>
    /// 获取图片格式，不带点号
    /// </summary>
    string Format { get; }
    #endregion
    #region 保存图片
    /// <summary>
    /// 保存这张图片
    /// </summary>
    /// <param name="path">保存路径，
    /// 如果它不包含正确的扩展名，
    /// 则函数会根据图片格式自动加上扩展名</param>
    /// <returns></returns>
    Task Save(string path);
    #endregion
}
