namespace System.AI.FaceRecognition;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个人脸检测的结果
/// </summary>
public interface IFaceRecognitionInfo
{
    #region 返回人脸数量
    /// <summary>
    /// 返回人脸的数量
    /// </summary>
    int FaceCount { get; }
    #endregion
}
