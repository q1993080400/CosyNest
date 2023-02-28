using System.Design;

namespace System.NetFrancis.Browser;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为浏览器中的一个选项卡
/// </summary>
public interface ITab : IInstruct, IDisposable
{
    #region 浏览器对象
    /// <summary>
    /// 获取浏览器对象
    /// </summary>
    IBrowser Browser { get; }
    #endregion
    #region 选择选项卡
    /// <summary>
    /// 选择这个选项卡，
    /// 将它设置为活动选项卡
    /// </summary>
    void Select();
    #endregion
}
