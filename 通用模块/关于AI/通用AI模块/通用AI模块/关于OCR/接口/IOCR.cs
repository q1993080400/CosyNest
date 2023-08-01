namespace System.AI;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以进行光学字符识别
/// </summary>
public interface IOCR
{
    #region 识别文件
    /// <summary>
    /// 识别图片文件，并返回识别结果
    /// </summary>
    /// <param name="imagePath">图片文件的路径</param>
    /// <returns>识别结果，如果为<see langword="null"/>，表示识别失败</returns>
    Task<string?> Identify(string imagePath);
    #endregion
}
