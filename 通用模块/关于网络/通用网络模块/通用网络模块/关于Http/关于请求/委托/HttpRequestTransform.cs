namespace System.NetFrancis;

/// <summary>
/// 这个委托转换Http请求，
/// 它可以用于配置Http请求的默认值
/// </summary>
/// <param name="old">要转换的旧请求</param>
/// <returns></returns>
public delegate Task<HttpRequestRecording> HttpRequestTransform(HttpRequestRecording old);