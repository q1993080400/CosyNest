namespace System.AI;

/// <summary>
/// 这个类型是<see cref="IFaceRecognitionInfo"/>的实现，
/// 可以视为一个人脸检测的结果
/// </summary>
sealed record FaceRecognitionInfo : IFaceRecognitionInfo
{
    #region 人脸检测的数量
    public required int FaceCount { get; init; }
    #endregion 
}
