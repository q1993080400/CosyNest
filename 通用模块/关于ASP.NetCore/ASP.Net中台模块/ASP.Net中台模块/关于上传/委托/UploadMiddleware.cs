namespace Microsoft.AspNetCore;

#region 上传中间件委托
/// <summary>
/// 这个委托是一个上传中间件，
/// 它可以执行处理上传文件的额外逻辑
/// </summary>
/// <param name="info">上传中间件的参数</param>
/// <returns>该中间件是否可以处理该文件，
/// 如果返回<see cref="UploadReturnValue.NotSupported"/>，后面的中间件不再执行</returns>
public delegate Task<UploadReturnValue> UploadMiddleware(UploadMiddlewareInfo info);
#endregion