using System.Reflection.Metadata;

namespace System.AI;

/// <summary>
/// 这个类型是底层使用PaddleOCRSharp实现的<see cref="IOCR"/>的模型配置选项
/// </summary>
public sealed record OCRPaddleModelOption
{
    #region 文本检测模型路径
    /// <summary>
    /// 获取文本检测模型的路径
    /// </summary>
    public required string TextDetectionModel { get; init; }
    #endregion
    #region 文本识别模型路径
    /// <summary>
    /// 获取文本识别模型的路径
    /// </summary>
    public required string TextRecognitionModel { get; init; }
    #endregion
    #region 文本角度检测模型路径
    /// <summary>
    /// 获取文本角度检测模型的路径
    /// </summary>
    public required string TextAngleDetectionModel { get; init; }
    #endregion
    #region Key路径
    /// <summary>
    /// 获取Key路径
    /// </summary>
    public required string Key { get; init; }
    #endregion
}
