using System.DataFrancis;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个类型可以将值绑定到排序条件
/// </summary>
/// <param name="render">用来渲染单个排序条件的对象</param>
public sealed class BindSortCondition(RenderSortCondition render) : IGenerateFilter
{
    #region 公开成员
    #region 排序状态
    /// <summary>
    /// 获取排序状态
    /// </summary>
    public SortStatus SortStatus { get; set; }
    #endregion
    #region 生成排序条件
    public DataCondition? GenerateFilter()
        => SortStatus is SortStatus.None ?
        null :
        new SortCondition()
        {
            SortStatus = SortStatus,
            PropertyAccess = Render.PropertyAccess,
            IsVirtually = Render.IsVirtually
        };
    #endregion
    #endregion
    #region 重写的成员
    #region 重写GetHashCode
    public override int GetHashCode()
        => Render.PropertyAccess.GetHashCode();
    #endregion
    #region 重写Equals
    public override bool Equals(object? obj)
        => obj is BindSortCondition bind &&
        bind.Render.PropertyAccess == Render.PropertyAccess;
    #endregion
    #endregion
    #region 内部成员
    #region 渲染排序条件的对象
    /// <summary>
    /// 用来渲染单个排序条件的对象
    /// </summary>
    private RenderSortCondition Render { get; } = render;
    #endregion
    #endregion
}
