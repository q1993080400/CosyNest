namespace System.NetFrancis.Browser;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个浏览器自动测试中的Web元素
/// </summary>
public interface IElementBrowser : IElementBase
{
    #region 提交表单
    /// <summary>
    /// 向元素提交表单，
    /// 只对部分元素有效
    /// </summary>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    ValueTask Submit(CancellationToken cancellationToken = default);
    #endregion
    #region 点击并创建一个新选项卡
    /// <summary>
    /// 触发这个元素的点击事件，
    /// 并返回一个<see cref="IDisposable"/>，
    /// 当释放它的时候，关闭浏览器最右边的选项卡，
    /// 它专门用于已知点击时一定会创建一个新选项卡的元素
    /// </summary>
    /// <returns></returns>
    Task<IDisposable> ClickWithCreateTab();
    #endregion
}
