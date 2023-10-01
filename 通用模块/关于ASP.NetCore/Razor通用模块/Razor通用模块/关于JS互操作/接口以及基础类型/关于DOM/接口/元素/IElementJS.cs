using System.MathFrancis.Plane;
using System.NetFrancis.Browser;

namespace Microsoft.JSInterop;

/// <summary>
/// 凡是实现本接口的类型，
/// 都可以用来表示一个用于JS互操作的DOM元素
/// </summary>
public interface IElementJS : IElementBase
{
    #region 关于元素的位置和大小
    #region 获取元素内部的高度
    /// <summary>
    /// 获取元素内部的高度，不包括滚动条
    /// </summary>
    ValueTask<double> ClientHeight { get; }
    #endregion
    #region 获取元素的完全高度
    /// <summary>
    /// 获取元素的完全高度，也就是如果不显示滚动条，
    /// 完全呈现它所需要的高度
    /// </summary>
    ValueTask<double> ScrollHeight { get; }
    #endregion
    #region 获取元素的绝对位置和大小
    /// <summary>
    /// 获取元素相对于视口的绝对位置和大小，
    /// 注意：返回的<see cref="ISizePos"/>遵循数学标准而不是UI标准，
    /// 它的Y坐标越小，代表越靠近屏幕底部
    /// </summary>
    /// <returns></returns>
    ValueTask<ISizePos> GetBoundingClientRect();
    #endregion
    #endregion
    #region 关于滚动
    #region 获取已滚动的像素
    /// <summary>
    /// 获取已经滚动的像素
    /// </summary>
    ValueTask<double> ScrollTop { get; }
    #endregion
    #region 获取滚动百分比
    /// <summary>
    /// 获取已经滚动的百分比
    /// </summary>
    /// <param name="notScrollingTreatedAs0">如果这个值为<see langword="true"/>，
    /// 则在计算滚动的时候，会加上ClientHeight，它会导致滚动进度永远不为0，
    /// 否则不会加上，它会导致滚动进度永远达不到1</param>
    /// <param name="treat0HeightAsScrolling1">当元素的高度为0的时候，
    /// 如果这个值为<see langword="true"/>，视为滚动100%，否则视为没有滚动</param>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    async ValueTask<double> ScrollPercentage(bool notScrollingTreatedAs0, bool treat0HeightAsScrolling1, CancellationToken cancellationToken = default)
    {
        var height = await ScrollHeight;
        if (height is 0)
            return treat0HeightAsScrolling1 ? 1 : 0;
        var scrollTop = await ScrollTop;
        var clientHeight = await ClientHeight;
        var top = scrollTop + (notScrollingTreatedAs0 ? 0 : clientHeight);
        return top / height;
    }

    /*注意：
      #出于未知原因，这个滚动百分比不是特别精确，
      它有可能已经全部滚动完了，但是返回值不是1，而是一个很接近的数值
    
      问：为什么要有notScrollingTreatedAs0这个参数？它似乎很费解
      答：这是因为关于滚动进度，存在以下歧义：

      #如果一个元素不需要滚动，那么它的滚动进度是0还是1？
      也就是，它是没有滚动，还是滚动到底了？

      #如果一个元素的ScrollTop是100，ClientHeight是900，ScrollHeight是1000，
      那么它的滚动进度是0.1还是1？
      认为滚动了0.1的人看来，它距离视口顶端有100的距离
      认为滚动了1的人看来，这个元素已经滚动到视口底部*/
    #endregion
    #region 滚动到指定位置
    #region 指定滚动坐标
    /// <summary>
    /// 滚动到指定的位置
    /// </summary>
    /// <param name="x">要滚动的X坐标</param>
    /// <param name="y">要滚动的Y坐标</param>
    /// <param name="isbehavior">如果这个值为<see langword="true"/>，
    /// 则为平滑滚动，否则为直接滚动</param>
    /// <param name="isAbs">设要滚动的坐标为p，如果这个值为<see langword="true"/>，
    /// 则滚动到父容器的p位置，否则向下或向右滚动p个坐标</param>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    ValueTask Scroll(double x, double y, bool isbehavior = false, bool isAbs = true, CancellationToken cancellationToken = default);
    #endregion
    #region 指定滚动百分比
    /// <summary>
    /// 按照百分比滚动到指定位置
    /// </summary>
    /// <param name="xPercentage">要滚动的X坐标百分比</param>
    /// <param name="yPercentage">要滚动的Y坐标值百分比</param>
    /// <returns></returns>
    /// <inheritdoc cref="Scroll(double, double, bool,bool, CancellationToken)"/>
    async ValueTask ScrollFromPercentage(double xPercentage, double yPercentage, bool isbehavior = false, bool isAbs = true, CancellationToken cancellationToken = default)
    {
        if (xPercentage is not 0)
            throw new NotSupportedException($"暂不支持横向滚动，{nameof(xPercentage)}必须为0");
        await Scroll(0, await ScrollHeight * yPercentage, isbehavior, isAbs, cancellationToken);
    }
    #endregion
    #endregion
    #endregion
    #region 获取焦点
    /// <summary>
    /// 使该元素获取焦点
    /// </summary>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    /// <returns></returns>
    ValueTask Focus(CancellationToken cancellationToken = default);
    #endregion
    #region 以元素为基础执行脚本
    #region 无返回值
    /// <summary>
    /// 以这个元素为基础执行JS脚本，且不返回值
    /// </summary>
    /// <param name="jsCode">要执行的JS代码，它相对于这个元素</param>
    /// <param name="isAsynchronous">如果这个值为<see langword="true"/>，
    /// 表示该JS代码是异步代码，且需要等待，会对脚本进行一些特殊处理和转换</param>
    /// <param name="cancellation">用于取消异步任务的令牌</param>
    /// <returns></returns>
    ValueTask InvokeCodeVoidAsync(string jsCode, bool isAsynchronous = false, CancellationToken cancellation = default);
    #endregion
    #region 有返回值
    /// <summary>
    /// 以这个元素为基础执行JS脚本，并返回脚本的返回值
    /// </summary>
    /// <typeparam name="Ret">脚本的返回值类型</typeparam>
    /// <inheritdoc cref="InvokeCodeVoidAsync(string, bool, CancellationToken)"/>
    ValueTask<Ret> InvokeCodeAsync<Ret>(string jsCode, bool isAsynchronous = false, CancellationToken cancellation = default);
    #endregion
    #endregion
}
