using System.Linq.Expressions;
using System.Maths;
using System.Maths.Plane;
using System.NetFrancis.Browser;

namespace Microsoft.JSInterop;

/// <summary>
/// 该类型是<see cref="IElementJS"/>的实现，
/// 可以视为一个Html元素
/// </summary>
sealed class Element : JSRuntimeBase, IElementJS
{
    #region 公开成员
    #region 获取ID
    public string? ID { get; }
    #endregion
    #region 触发鼠标点击事件
    public ValueTask Click(CancellationToken cancellationToken)
        => JSRuntime.InvokeCodeVoidAsync($"{Prefix}.click()", cancellation: cancellationToken);
    #endregion
    #region 关于元素的位置和大小
    #region 获取元素的完全高度
    public ValueTask<double> ScrollHeight
          => JSRuntime.InvokeCodeAsync<double>(Prefix + ".scrollHeight");
    #endregion
    #region 获取元素内部的高度
    public ValueTask<double> ClientHeight
        => JSRuntime.InvokeCodeAsync<double>(Prefix + ".clientHeight");
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
                => (await JSRuntime.InvokeCodeAsync<double?>(Prefix + ".scrollTop")) ?? 0;
            #endregion
            return Fun();
        }
    }
    #endregion
    #region 滚动到指定位置
    public ValueTask Scroll(double x, double y, bool isbehavior, bool isAbs = true, CancellationToken cancellationToken = default)
    {
        var script = $".{(isAbs ? "scroll" : "scrollBy")}({{left:{x},top:{y},behavior:'{(isbehavior ? "smooth" : "auto")}'}})";
        return JSRuntime.InvokeCodeVoidAsync(Prefix + script, cancellation: cancellationToken);
    }
    #endregion
    #endregion
    #region 获取焦点
    public ValueTask Focus(CancellationToken cancellationToken = default)
        => JSRuntime.InvokeCodeVoidAsync($"{Prefix}.focus()", cancellation: cancellationToken);
    #endregion
    #region 获取成员
    private IAsyncIndex<string, string>? IndexField;

    public IAsyncIndex<string, string> Index
        => IndexField ??= CreateTasks.AsyncIndex<string, string>(
           async (name, c) => await JSRuntime.GetProperty<string>($"{Prefix}.{name}.toString()", c),
           async (name, value, c) => await JSRuntime.SetProperty($"{Prefix}.{name}", value, c));
    #endregion
    #region 以元素为基础执行脚本
    #region 无返回值
    public ValueTask InvokeCodeVoidAsync(string jsCode, bool isAsynchronous = false, CancellationToken cancellation = default)
        => JSRuntime.InvokeCodeVoidAsync($"{Prefix}.{jsCode}", isAsynchronous, cancellation);
    #endregion
    #region 有返回值
    public ValueTask<Ret> InvokeCodeAsync<Ret>(string jsCode, bool isAsynchronous = false, CancellationToken cancellation = default)
        => JSRuntime.InvokeCodeAsync<Ret>($"{Prefix}.{jsCode}", isAsynchronous, cancellation);
    #endregion
    #endregion
    #endregion
    #region 内部成员
    #region 获取前缀
    /// <summary>
    /// 获取元素的前缀，
    /// 它被用来从DOM中找到元素
    /// </summary>
    private string Prefix { get; }
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

    public IEnumerable<Element1> Find<Element1>(Expression<Func<Element1, bool>> where) where Element1 : IElementBase
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Element1> FindFromCss<Element1>(string cssSelect) where Element1 : IElementBase
    {
        throw new NotImplementedException();
    }
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="id">元素的ID</param>
    /// <inheritdoc cref="JSRuntimeBase(IJSRuntime)"/>
    public Element(IJSRuntime jsRuntime, string id)
        : base(jsRuntime)
    {
        this.ID = id;
        this.Prefix = $"document.getElementById(\"{id}\")";
    }
    #endregion
}
