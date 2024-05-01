namespace System.AI;

/// <summary>
/// 这个类型是底层使用PaddleOCRSharp实现的<see cref="IOCR"/>的配置选项
/// </summary>
public sealed record OCRPaddleOption
{
    #region 模型配置
    /// <summary>
    /// 获取OCR的模型配置
    /// </summary>
    public OCRPaddleModelOption? ModelOption { get; init; }
    #endregion
}
