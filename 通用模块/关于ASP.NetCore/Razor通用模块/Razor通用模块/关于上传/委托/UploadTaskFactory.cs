using System.Design;

namespace Microsoft.AspNetCore.Components.Forms;

/// <summary>
/// 返回一个上传任务，
/// 它可以将文件上传到指定位置，并支持进行后续处理
/// </summary>
/// <param name="info">用于上传的参数</param>
/// <returns></returns>
public delegate LongTask<Progress> UploadTaskFactory<Progress>(UploadInfo info);
