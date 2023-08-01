namespace System.AI;

/// <summary>
/// 这个静态类可以用来创建使用PaddleOCRSharp实现的OCR对象
/// </summary>
public static class CreateOCRPaddle
{
    #region 返回公用的OCR对象
    /// <summary>
    /// 返回公用的OCR对象，
    /// 它可以进行文字识别
    /// </summary>
    public static IOCR OCR { get; } = new OCRPaddleOCRSharp();
    #endregion
}
