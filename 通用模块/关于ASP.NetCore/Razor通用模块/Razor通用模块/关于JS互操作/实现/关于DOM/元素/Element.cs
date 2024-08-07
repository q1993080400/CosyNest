﻿using System.Linq.Expressions;
using System.MathFrancis;
using System.MathFrancis.Plane;
using System.NetFrancis.Browser;

namespace Microsoft.JSInterop;

/// <summary>
/// 该类型是<see cref="IElementJS"/>的实现，
/// 可以视为一个Html元素
/// </summary>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="id">元素的ID</param>
/// <param name="jsRuntime">封装的JS运行时对象，本对象的功能就是通过它实现的</param>
sealed class Element(IJSRuntime jsRuntime, string id) : IElementJS
{
    #region 公开成员
    #region 获取ID
    public string? ID { get; } = id;
    #endregion
    #region 触发鼠标点击事件
    public async ValueTask Click(int interval = 0, CancellationToken cancellationToken = default)
    {
        await jsRuntime.InvokeCodeVoidAsync($"{Prefix}.click()", cancellation: cancellationToken);
        if (interval > 0)
            await Task.Delay(interval, cancellationToken);
    }
    #endregion
    #region 关于元素的位置和大小
    #region 获取元素的完全高度
    public ValueTask<double> ScrollHeight
          => jsRuntime.InvokeCodeAsync<double>(Prefix + ".scrollHeight");
    #endregion
    #region 获取元素内部的高度
    public ValueTask<double> ClientHeight
        => jsRuntime.InvokeCodeAsync<double>(Prefix + ".clientHeight");
    #endregion
    #region 获取元素的绝对位置和大小
    public async ValueTask<ISizePos> GetBoundingClientRect()
    {
        var domRect = await InvokeCodeAsync<DOMRect>("getBoundingClientRect()");
        return CreateMath.SizePos(-domRect.y, domRect.x, domRect.width, domRect.height);
    }
    #endregion
    #endregion
    #region 关于滚动
    #region 获取已滚动的像素
    public ValueTask<double> ScrollTop
    {
        get
        {
            #region 本地函数
            async ValueTask<double> Fun()
                => (await jsRuntime.InvokeCodeAsync<double?>(Prefix + ".scrollTop")) ?? 0;
            #endregion
            return Fun();
        }
    }
    #endregion
    #region 滚动到指定位置
    public ValueTask Scroll(double x, double y, bool isbehavior, bool isAbs = true, CancellationToken cancellationToken = default)
    {
        var script = $".{(isAbs ? "scroll" : "scrollBy")}({{left:{x},top:{y},behavior:'{(isbehavior ? "smooth" : "auto")}'}})";
        return jsRuntime.InvokeCodeVoidAsync(Prefix + script, cancellation: cancellationToken);
    }
    #endregion
    #endregion
    #region 获取焦点
    public ValueTask Focus(CancellationToken cancellationToken = default)
        => jsRuntime.InvokeCodeVoidAsync($"{Prefix}.focus()", cancellation: cancellationToken);
    #endregion
    #region 获取成员
    private IAsyncIndex<string, string>? IndexField;

    public IAsyncIndex<string, string> Index
        => IndexField ??= CreateTasks.AsyncIndex<string, string>(
           async (name, c) => await jsRuntime.GetProperty<string>($"{Prefix}.{name}.toString()", c),
           async (name, value, c) => await jsRuntime.SetProperty($"{Prefix}.{name}", value, c));
    #endregion
    #region 以元素为基础执行脚本
    #region 无返回值
    public ValueTask InvokeCodeVoidAsync(string jsCode, bool isAsynchronous = false, CancellationToken cancellation = default)
        => jsRuntime.InvokeCodeVoidAsync($"{Prefix}.{jsCode}", isAsynchronous, cancellation);
    #endregion
    #region 有返回值
    public ValueTask<Ret> InvokeCodeAsync<Ret>(string jsCode, bool isAsynchronous = false, CancellationToken cancellation = default)
        => jsRuntime.InvokeCodeAsync<Ret>($"{Prefix}.{jsCode}", isAsynchronous, cancellation);
    #endregion
    #endregion
    #endregion
    #region 内部成员
    #region 获取前缀
    /// <summary>
    /// 获取元素的前缀，
    /// 它被用来从DOM中找到元素
    /// </summary>
    private string Prefix { get; } = $"document.getElementById(\"{id}\")";
    #endregion
    #endregion
    #region 私有辅助类
    /// <summary>
    /// 这个类型是对JS中的DOMRect类型的映射
    /// </summary>
    private struct DOMRect
    {
#pragma warning disable IDE1006
        public double x { get; set; }
        public double y { get; set; }
        public double width { get; set; }
        public double height { get; set; }
#pragma warning restore
    }
    #endregion
    #region 未实现的成员
    public string Type => throw new NotImplementedException();

    public string CssClass => throw new NotImplementedException();

    public string Text => throw new NotImplementedException();

    public IReadOnlyList<Element1> Find<Element1>(Expression<Func<Element1, bool>> where, bool ignoreException) where Element1 : IElementBase
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<Element1> FindFromCss<Element1>(string cssSelect, bool ignoreException) where Element1 : IElementBase
    {
        throw new NotImplementedException();
    }

    #endregion
}
