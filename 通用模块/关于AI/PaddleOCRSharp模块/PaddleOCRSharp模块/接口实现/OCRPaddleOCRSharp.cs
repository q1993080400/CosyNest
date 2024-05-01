using PaddleOCRSharp;

namespace System.AI;

/// <summary>
/// 这个类型是底层使用PaddleOCRSharp实现的<see cref="IOCR"/>，
/// 可以用来进行文字识别
/// </summary> 
/// <param name="engine">文字识别引擎</param>
sealed class OCRPaddleOCRSharp(PaddleOCREngine engine) : IOCR
{
    #region 公开成员
    #region 识别图片
    public Task<string?> Identify(string imagePath)
    {
        var result = engine.DetectText(imagePath);
        return Task.FromResult(result.Text)!;
    }
    #endregion
    #endregion
}
