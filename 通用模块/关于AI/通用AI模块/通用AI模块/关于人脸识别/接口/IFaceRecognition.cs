namespace System.AI.FaceRecognition;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来进行人脸识别
/// </summary>
public interface IFaceRecognition
{
    #region 执行人脸检测
    /// <summary>
    /// 执行一个基本的人脸检测，
    /// 并且返回结果
    /// </summary>
    /// <param name="imageBase64">以Base64形式编码的人脸图片</param>
    /// <param name="cancellation">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    Task<IFaceRecognitionInfo> Recognition(string imageBase64, CancellationToken cancellation = default);
    #endregion
}
