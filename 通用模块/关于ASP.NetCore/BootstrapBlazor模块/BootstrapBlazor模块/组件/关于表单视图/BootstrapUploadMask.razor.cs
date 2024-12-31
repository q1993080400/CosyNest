using Microsoft.AspNetCore.Components;

namespace BootstrapBlazor.Components;

/// <summary>
/// 当<see cref="BootstrapFormViewer{Model}"/>执行提交逻辑时，
/// 如果其中包含正在执行的上传操作，
/// 可以通过本组件遮罩屏幕，提醒用户上传正在进行
/// </summary>
public sealed partial class BootstrapUploadMask : ComponentBase
{

}
