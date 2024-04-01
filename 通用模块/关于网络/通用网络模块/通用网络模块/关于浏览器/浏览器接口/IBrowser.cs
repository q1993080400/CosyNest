using System.Design;
using System.Underlying.PC;

namespace System.NetFrancis.Browser;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个浏览器
/// </summary>
public interface IBrowser : IInstruct, IDisposable
{
    #region 有关选项卡
    #region 创建新选项卡
    /// <summary>
    /// 创建一个新的选项卡，并返回
    /// </summary>
    /// <param name="uri">新选项卡的Uri</param>
    /// <returns>新创建的选项卡，在返回时，已经加载完毕，
    /// 浏览器会聚焦到这个选项卡</returns>
    ITab CreateTab(string uri);
    #endregion
    #region 所有选项卡
    /// <summary>
    /// 获取所有选项卡
    /// </summary>
    IReadOnlyList<ITab> Tabs { get; }
    #endregion
    #region 当前选项卡
    /// <summary>
    /// 获取当前选项卡
    /// </summary>
    ITab CurrentTab { get; }
    #endregion
    #endregion
    #region 有关浏览器内容
    #region 说明文档
    /*注意：
      以下操作的目标都是当前选项卡*/
    #endregion
    #region 根节点元素
    /// <summary>
    /// 获取根节点元素
    /// </summary>
    IElementBrowser Body { get; }
    #endregion
    #region 键盘模拟器
    /// <summary>
    /// 获取一个键盘模拟器，
    /// 通过它可以向选项卡发送按键
    /// </summary>
    IKeyBoard KeyboardEmulation { get; }
    #endregion
    #region 关于执行脚本
    #region 无返回值
    /// <summary>
    /// 执行JS脚本，且不返回值
    /// </summary>
    /// <param name="script">要执行的脚本</param>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    ValueTask InvokingScriptVoid(string script, CancellationToken cancellationToken = default);
    #endregion
    #region 有返回值
    /// <summary>
    /// 执行JS脚本，
    /// 并返回JS脚本的返回值
    /// </summary>
    /// <returns>JS脚本的返回值</returns>
    /// <typeparam name="Obj">JS脚本的返回值类型</typeparam>
    /// <inheritdoc cref="InvokingScriptVoid(string, CancellationToken)"/>
    ValueTask<Obj> InvokingScript<Obj>(string script, CancellationToken cancellationToken = default);
    #endregion
    #endregion 
    #region 当前Uri
    /// <summary>
    /// 获取或设置当前选项卡Uri，
    /// 当设置这个属性时，会将当前选项卡跳转到指定位置
    /// </summary>
    string Uri { get; set; }
    #endregion
    #endregion 
}
