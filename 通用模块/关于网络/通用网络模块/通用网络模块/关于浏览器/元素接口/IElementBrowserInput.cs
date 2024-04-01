namespace System.NetFrancis.Browser;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个浏览器中的可输入元素
/// </summary>
public interface IElementBrowserInput : IElementBrowser
{
    #region 输入字符串
    /// <summary>
    /// 向元素中输入字符串，只对部分元素有效
    /// </summary>
    /// <param name="input">输入的字符串</param>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    ValueTask Input(string input, CancellationToken cancellationToken = default);
    #endregion
}
