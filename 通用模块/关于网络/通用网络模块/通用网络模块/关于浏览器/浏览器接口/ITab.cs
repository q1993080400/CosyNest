using System.Design;
using System.Underlying.PC;

namespace System.NetFrancis.Browser;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为浏览器中的一个选项卡
/// </summary>
public interface ITab : IInstruct, IDisposable
{
    #region 根节点元素
    /// <summary>
    /// 获取根节点元素
    /// </summary>
    IElementBrowser Body { get; }
    #endregion
    #region 浏览器对象
    /// <summary>
    /// 获取浏览器对象
    /// </summary>
    IBrowser Browser { get; }
    #endregion
    #region 键盘模拟器
    /// <summary>
    /// 获取一个键盘模拟器，
    /// 通过它可以向选项卡发送按键
    /// </summary>
    IKeyBoard KeyboardEmulation { get; }
    #endregion
    #region 选项卡Uri
    /// <summary>
    /// 获取或设置选项卡Uri，
    /// 当设置这个属性时，会将选项卡跳转到指定位置
    /// </summary>
    string Uri { get; set; }
    #endregion
}
