namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个委托可以将浏览器文件对象转换为<see cref="IUploadFile"/>
/// </summary>
/// <param name="file">待转换的浏览器文件对象</param>
/// <param name="options">用于上传文件的选项</param>
/// <returns></returns>
public delegate IUploadFile BrowserFileConvert(IBrowserFile file, UploadFileOptions options);
