namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以为待渲染的元素编号
/// </summary>
public interface IElementNumber
{
    #region 为元素编号
    /// <summary>
    /// 为元素编号，并返回元素的ID
    /// </summary>
    /// <param name="index">元素的索引</param>
    /// <returns></returns>
    string GetElementID(int index);
    #endregion
    #region 返回跳转到指定元素的委托
    /// <summary>
    /// 返回一个委托，
    /// 调用它可以跳转到指定的元素
    /// </summary>
    /// <param name="index">要跳转到的元素的索引</param>
    /// <param name="smooth">如果这个值为<see langword="true"/>，
    /// 表示应该进行平滑滚动，否则进行立即滚动</param>
    /// <returns></returns>
    Func<Task> JumpToElement(int index, bool smooth = true);
    #endregion
}
