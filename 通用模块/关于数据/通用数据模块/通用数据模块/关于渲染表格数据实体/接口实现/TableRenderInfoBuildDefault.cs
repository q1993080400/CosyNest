using System.Reflection;

namespace System.DataFrancis;

/// <summary>
/// 这个类型是<see cref="ITableRenderInfoBuild{Model}"/>的默认实现，
/// 它可以为表格组件提供渲染参数
/// </summary>
/// <inheritdoc cref="ITableRenderInfoBuild{Model}"/>
sealed class TableRenderInfoBuildDefault<Model> : ITableRenderInfoBuild<Model>
    where Model : class
{
    #region 静态成员：获取唯一对象
    /// <summary>
    /// 获取这个泛型类型的唯一一个对象
    /// </summary>
    public static ITableRenderInfoBuild<Model> Single { get; }
        = new TableRenderInfoBuildDefault<Model>();
    #endregion
    #region 接口实现
    #region 获取表头渲染参数
    public IReadOnlyCollection<RenderTableHeaderColumnsInfoBase> GetRenderHeaderColumnsInfo()
        => RenderHeaderColumnsInfoCache;
    #endregion
    #region 获取表身渲染参数
    public IReadOnlyCollection<RenderBodyColumnsInfoBase<Model>> GetRenderBodyColumnsInfo(Model model, int rowIndex)
        => CacheRenderInfo.Select(x => x.Attribute.GetRenderBodyColumnsInfo(x.MemberInfo, model, rowIndex)).ToArray();
    #endregion
    #endregion
    #region 内部成员
    #region 表头渲染参数缓存
    /// <summary>
    /// 获取表头渲染参数的缓存
    /// </summary>
    private IReadOnlyCollection<RenderTableHeaderColumnsInfoBase> RenderHeaderColumnsInfoCache { get; }
    #endregion
    #region 缓存所有特性和属性成员
    /// <summary>
    /// 这个集合缓存所有应该渲染的属性成员和渲染特性
    /// </summary>
    private IEnumerable<(MemberInfo MemberInfo, RenderTableBaseAttribute Attribute)> CacheRenderInfo { get; }
    #endregion
    #endregion
    #region 构造函数
    public TableRenderInfoBuildDefault()
    {
        var type = typeof(Model);
        CacheRenderInfo = [.. type.GetProperties().Where(static x => !x.IsStatic()).
            Cast<MemberInfo>().Append(type).
            Select(static x =>  (x,x.GetCustomAttribute<RenderTableBaseAttribute>()!)).
            Where(static x => x.Item2 is { }).ToArray().
            OrderBy(static x => x.Item2.Order)];
        RenderHeaderColumnsInfoCache = CacheRenderInfo.
            Select(static x => x.Attribute.GetTableHeaderColumnsInfo(x.MemberInfo)).ToArray();
    }
    #endregion
}
