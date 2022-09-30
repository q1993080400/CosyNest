namespace System.NetFrancis.Browser;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个浏览器自动测试中的Web元素
/// </summary>
public interface IElementBrowser : IElementBase
{
    #region 获取浏览器标签
    /// <summary>
    /// 获取元素所在的浏览器标签
    /// </summary>
    ITab Tab { get; }
    #endregion
    #region 输入字符串
    /// <summary>
    /// 向元素中输入字符串，只对部分元素有效
    /// </summary>
    /// <param name="input">输入的字符串</param>
    void Input(string input);
    #endregion
    #region 提交表单
    /// <summary>
    /// 向元素提交表单，
    /// 只对部分元素有效
    /// </summary>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    ValueTask Submit(CancellationToken cancellationToken = default);
    #endregion
}
