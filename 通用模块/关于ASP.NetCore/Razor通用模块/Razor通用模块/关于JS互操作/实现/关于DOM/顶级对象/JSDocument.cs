﻿using Microsoft.AspNetCore;

namespace Microsoft.JSInterop;

/// <summary>
/// 这个类型是<see cref="IJSDocument"/>的实现，
/// 可以视为一个JS中的Document对象
/// </summary>
/// <param name="jsRuntime">封装的JS运行时对象，本对象的功能就是通过它实现的</param>
sealed class JSDocument(IJSRuntime jsRuntime) : IJSDocument
{
    #region 返回索引Cookie的字典
    public ICookie Cookie { get; }
        = new JSCookie(jsRuntime);
    #endregion
    #region 获取或设置标题
    public IAsyncProperty<string> Title { get; }
        = jsRuntime.PackProperty<string>("document.title");
    #endregion
    #region 返回页面的可见状态
    public Task<VisibilityState> VisibilityState
    {
        get
        {
            #region 本地函数
            async Task<VisibilityState> Fun()
            {
                var visibilityState = await jsRuntime.InvokeCodeAsync<string>("document.visibilityState");
                return visibilityState switch
                {
                    "visible" => JSInterop.VisibilityState.Visible,
                    "hidden" => JSInterop.VisibilityState.Hidden,
                    "prerender" => JSInterop.VisibilityState.Prerender,
                    var state => throw new NotSupportedException($"{state}是无法识别的页面可见性状态")
                };
            }
            #endregion
            return Fun();
        }
    }
    #endregion
}
