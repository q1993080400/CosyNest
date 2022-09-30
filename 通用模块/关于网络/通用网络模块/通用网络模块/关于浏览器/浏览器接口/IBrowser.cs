using System.Design;

namespace System.NetFrancis.Browser;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个浏览器
/// </summary>
public interface IBrowser : IInstruct
{
    #region 创建新选项卡
    /// <summary>
    /// 创建一个新的选项卡，并返回
    /// </summary>
    /// <param name="uri">新选项卡的Uri</param>
    /// <returns>新创建的选项卡，在返回时，已经加载完毕</returns>
    ITab CreateTab(string uri);
    #endregion
}
