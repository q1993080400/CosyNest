using System.Design;

namespace Microsoft.AspNetCore.Components.Forms;

/// <summary>
/// 返回一个上传任务，
/// 它可以将文件上传到一个<see cref="WithResource"/>的资源目录中
/// </summary>
/// <param name="uploads">上传的目标目录</param>
/// <param name="files">要上传的所有文件</param>
/// <returns></returns>
/// <inheritdoc cref="LongTask{Progress}"/>

public delegate LongTask<Progress> UploadTaskFactory<Progress>(string uploads, IEnumerable<IBrowserFile> files);
