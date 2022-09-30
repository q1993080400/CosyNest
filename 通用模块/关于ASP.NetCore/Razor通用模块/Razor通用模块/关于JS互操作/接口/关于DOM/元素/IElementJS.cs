using System.Maths.Plane;
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
    /// <param name="calculateCurrentScreen">如果这个值为<see langword="true"/>，
    /// 则在计算滚动百分比时，会把当前视口屏幕计算进去，
    /// 否则不会计算，它会导致滚动进度永远不会达到1</param>
    /// <param name="cancellationToken">一个用于取消异步操作的令牌</param>
    async ValueTask<double> ScrollPercentage(bool calculateCurrentScreen, CancellationToken cancellationToken = default)
    {
        var top = await ScrollTop + (calculateCurrentScreen ? await ClientHeight : 0);
        var height = await ScrollHeight;
        return top / height;
    }

    /*注意：
      出于未知原因，这个滚动百分比不是特别精确，
      它有可能已经全部滚动完了，但是返回值不是1，而是一个很接近的数值*/
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
