namespace System.DataFrancis;

/// <summary>
/// 这个类型是<see cref="ITableRenderInfoBuild{Model}"/>的实现，
/// 它可以穿透模型，实际渲染模型中的某一个属性，
/// 它通常用于渲染被外层封装过的类型
/// </summary>
/// <typeparam name="OuterModel">外层封装模型的类型</typeparam>
/// <typeparam name="TrueModel">内层实际被渲染的模型的类型</typeparam>
/// <param name="build">用来生成内层模型渲染参数的对象</param>
/// <param name="penetration">这个委托传入外层模型，返回内层模型</param>
sealed class TableRenderInfoBuildPenetration<OuterModel, TrueModel>
    (ITableRenderInfoBuild<TrueModel> build, Func<OuterModel, TrueModel> penetration)
    : ITableRenderInfoBuild<OuterModel>
    where OuterModel : class
    where TrueModel : class
{
    #region 接口实现
    #region 获取表头渲染参数
    public IReadOnlyCollection<RenderTableHeaderColumnsInfoBase> GetRenderHeaderColumnsInfo()
        => build.GetRenderHeaderColumnsInfo();
    #endregion
    #region 获取表身渲染参数
    public IReadOnlyCollection<RenderBodyColumnsInfoBase<OuterModel>> GetRenderBodyColumnsInfo
        (OuterModel model, int rowIndex)
        => build.GetRenderBodyColumnsInfo(penetration(model), rowIndex).
        Select(x => x switch
        {
            RenderTableBodyColumnsInfo<TrueModel> info =>
            (RenderBodyColumnsInfoBase<OuterModel>)new RenderTableBodyColumnsInfo<OuterModel>()
            {
                ColumnsName = info.ColumnsName,
                RowIndex = info.RowIndex,
                TableModel = model,
                PropertyInfo = info.PropertyInfo,
                IsLong = info.IsLong,
            },
            RenderTableBodyColumnsInfoCustom<TrueModel> info =>
            new RenderTableBodyColumnsInfoCustom<OuterModel>()
            {
                ColumnsName = info.ColumnsName,
                RowIndex = info.RowIndex,
                TableModel = model,
                IsLong = info.IsLong,
            },
            var info => throw new NotSupportedException($"无法识别{info.GetType()}类型的渲染参数")
        }).ToArray();
    #endregion
    #endregion
}
