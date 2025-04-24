namespace System.NetFrancis.Amazon;

/// <summary>
/// 这个枚举指示亚马逊CDN函数的类型
/// </summary>
public enum FunctionEventType
{
    /// <summary>
    /// 源请求函数，它在CDN向源发送请求的时候执行
    /// </summary>
    OriginRequest,
    /// <summary>
    /// 源响应函数，它在CDN接受从源的响应的时候执行
    /// </summary>
    OriginResponse,
    /// <summary>
    /// 查看器请求函数，它在CDN接受用户的请求的时候执行
    /// </summary>
    ViewerRequest,
    /// <summary>
    /// 查看器响应函数，它在CDN向用户发送响应的时候执行
    /// </summary>
    ViewerResponse
}
