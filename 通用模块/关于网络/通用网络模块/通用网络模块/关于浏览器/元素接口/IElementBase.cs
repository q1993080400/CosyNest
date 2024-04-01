using System.Linq.Expressions;

namespace System.NetFrancis.Browser;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为浏览器中的一个元素
/// </summary>
public interface IElementBase
{
    #region 关于元素的属性
    #region 读写属性
    /// <summary>
    /// 返回一个异步索引器，
    /// 它的参数是属性的名称，返回值就是要读取的属性
    /// </summary>
    /// <returns></returns>
    IAsyncIndex<string, string> Index { get; }
    #endregion
    #region 获取ID
    /// <summary>
    /// 获取这个元素的ID（如果有）
    /// </summary>
    string? ID { get; }
    #endregion
    #region 元素的类型
    /// <summary>
    /// 获取元素标签的类型
    /// </summary>
    string Type { get; }
    #endregion
    #region Css类名
    /// <summary>
    /// 获取元素的Css类名
    /// </summary>
    string CssClass { get; }
    #endregion
    #region 元素的显示文本
    /// <summary>
    /// 获取元素的显示文本
    /// </summary>
    string Text { get; }
    #endregion
    #endregion
    #region 触发鼠标点击事件
    /// <summary>
    /// 触发这个元素的鼠标点击事件
    /// </summary>
    /// <returns></returns>
    /// <param name="cancellationToken">用于取消异步操作的令牌</param>
    ValueTask Click(CancellationToken cancellationToken = default);
    #endregion
    #region 查找后代元素
    #region 根据谓词
    /// <summary>
    /// 查找这个元素的后代元素
    /// </summary>
    /// <param name="where">用来查找元素的谓词</param>
    /// <param name="ignoreException">如果这个值为<see langword="true"/>，
    /// 则在超时的时候，不引发异常，而是返回一个空数组</param>
    /// <typeparam name="Element">浏览器元素的具体类型</typeparam>
    /// <returns></returns>
    IEnumerable<Element> Find<Element>(Expression<Func<Element, bool>> where, bool ignoreException = false)
        where Element : IElementBase;
    #endregion
    #region 根据CSS选择器
    #region 泛型方法
    /// <param name="cssSelect">用来匹配后代元素的CSS选择器文本</param>
    /// <returns></returns>
    /// <inheritdoc cref="Find{Element}(Expression{Func{Element, bool}},bool)"/>
    IEnumerable<Element> FindFromCss<Element>(string cssSelect, bool ignoreException = false)
        where Element : IElementBase;
    #endregion
    #region 非泛型方法
    /// <inheritdoc cref="FindFromCss{Element}(string, bool)"/>
    IEnumerable<IElementBrowser> FindFromCss(string cssSelect, bool ignoreException = false)
        => FindFromCss<IElementBrowser>(cssSelect, ignoreException);
    #endregion
    #endregion
    #endregion
}
