namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个委托可以用来创建一个适用于客户端的可预览文件
/// </summary>
/// <param name="coverUri">封面的Uri</param>
/// <param name="uri">本体的Uri</param>
/// <param name="uploadFile">要上传的文件</param>
/// <param name="id">文件的ID，它一般被用来将文件映射为数据库对象</param>
/// <returns></returns>

public delegate IHasUploadFileClient UploadFileClientFactory(string coverUri, string uri, IUploadFile uploadFile, Guid id = default);