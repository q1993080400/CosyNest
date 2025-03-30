using BootstrapBlazor.Components;

using Microsoft.AspNetCore.Components;

namespace System;

public static partial class ExtendBootstrapBlazor
{
    //这个静态类专门用来声明有关上传的扩展方法

    #region 返回通过剪切板进行上传的委托
    /// <summary>
    /// 这个高阶函数返回一个函数，
    /// 它可以通过剪切板进行上传，
    /// 并在上传失败时给予一个提示
    /// </summary>
    /// <param name="renderFileUploadInfo">用来上传的参数</param>
    /// <param name="messageService">用来弹出提示的对象</param>
    /// <returns></returns>
    public static Func<Task> GetOnUploadFromClipboard(this RenderFileUploadInfo renderFileUploadInfo, MessageService messageService)
        => async () =>
        {
            var message = await renderFileUploadInfo.OnUploadFromClipboard();
            if (message is { })
                await messageService.Show(message);
        };
    #endregion
}
