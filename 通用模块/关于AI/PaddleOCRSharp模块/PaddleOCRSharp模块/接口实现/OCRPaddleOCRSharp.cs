using PaddleOCRSharp;

namespace System.AI;

/// <summary>
/// 这个类型是底层使用PaddleOCRSharp实现的<see cref="IOCR"/>，
/// 可以用来进行文字识别
/// </summary> 
sealed class OCRPaddleOCRSharp : IOCR
{
    #region 公开成员
    #region 识别图片
    public Task<string?> Identify(string imagePath)
    {
        var result = Engine.DetectText(imagePath);
        return Task.FromResult(result.Text)!;
    }
    #endregion
    #endregion
    #region 内部成员
    #region 公用的OCR引擎
    /// <summary>
    /// 获取公用的OCR引擎，
    /// 它负责执行文字识别操作
    /// </summary>
    private static PaddleOCREngine Engine { get; } = new(null);
    #endregion
    #endregion
}
