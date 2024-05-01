using PaddleOCRSharp;

namespace System.AI;

/// <summary>
/// 这个静态类可以用来创建使用PaddleOCRSharp实现的OCR对象
/// </summary>
public static class CreateOCRPaddle
{
    #region 创建OCR对象
    /// <summary>
    /// 创建一个底层使用PaddleOCRSharp实现的<see cref="IOCR"/>对象
    /// </summary>
    /// <param name="option">配置选项，
    /// 如果为<see langword="null"/>，则使用默认对象</param>
    /// <returns></returns>
    public static IOCR OCR(OCRPaddleOption? option = null)
    {
        OCRModelConfig? modelConfig = null;
        if (option is { ModelOption: { } mo })
        {
            modelConfig = new()
            {
                det_infer = mo.TextDetectionModel,
                rec_infer = mo.TextRecognitionModel,
                cls_infer = mo.TextAngleDetectionModel,
                keys = mo.Key
            };
        }
        var engine = new PaddleOCREngine(modelConfig);
        return new OCRPaddleOCRSharp(engine);
    }
    #endregion
}
