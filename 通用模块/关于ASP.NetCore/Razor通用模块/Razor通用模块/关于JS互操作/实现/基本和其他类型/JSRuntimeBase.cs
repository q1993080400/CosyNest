namespace Microsoft.JSInterop;

/// <summary>
/// 这个抽象类是所有JS对象封装的基类
/// </summary>
abstract class JSRuntimeBase
{
    #region 封装的JS运行时
    /// <summary>
    /// 获取封装的JS运行时对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    protected IJSRuntime JSRuntime { get; }
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的JS运行时初始化对象
    /// </summary>
    /// <param name="jsRuntime">封装的JS运行时对象，本对象的功能就是通过它实现的</param>
    public JSRuntimeBase(IJSRuntime jsRuntime)
    {
        this.JSRuntime = jsRuntime;
    }
    #endregion
}
