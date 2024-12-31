namespace System.DataFrancis;

/// <summary>
/// 这个静态类可以用来帮助创建一些关于数据的对象
/// </summary>
public static partial class CreateDataObj
{
    #region 创建表达式解析器
    /// <summary>
    /// 返回一个<see cref="IDataFilterAnalysis"/>的默认实现，
    /// 它可以将<see cref="DataFilterDescription"/>解析为表达式树
    /// </summary>
    public static IDataFilterAnalysis DataFilterAnalysis { get; }
        = new DataFilterAnalysisDefault();
    #endregion
    #region 创建错误日志
    /// <summary>
    /// 创建一个错误日志
    /// </summary>
    /// <typeparam name="Log">错误日志的类型</typeparam>
    /// <param name="exception">错误日志所记录的错误</param>
    /// <param name="state">传入的状态对象</param>
    /// <returns></returns>
    public static Log LogException<Log>(Exception? exception, object? state)
        where Log : LogException, new()
    {
        var ex = exception is { InnerException: { } e } ? e : exception;
        var method = ex?.TargetSite;
        var log = new Log()
        {
            Message = ex?.Message ?? "",
            AdditionalMessage = state?.ToString() ?? "",
            Stack = ex?.StackTrace ?? "",
            Method = method is null ? "" : $"{method.DeclaringType}.{method.Name}",
            Exception = ex?.GetType().Name ?? "",
        };
        return log;
    }
    #endregion
    #region 创建ITableViewerRenderInfoBuild
    #region 默认实现
    /// <summary>
    /// 返回一个默认实现的<see cref="ITableRenderInfoBuild{Model}"/>对象，
    /// 它可以用于为表格组件提供渲染参数
    /// </summary>
    /// <inheritdoc cref="TableRenderInfoBuildDefault{Model}"/>
    /// <returns></returns>
    public static ITableRenderInfoBuild<Model> TableViewerRenderInfoBuild<Model>()
        where Model : class
        => TableRenderInfoBuildDefault<Model>.Single;
    #endregion
    #region 可穿透对象
    /// <summary>
    /// 创建一个<see cref="ITableRenderInfoBuild{Model}"/>,
    /// 它可以穿透模型，实际渲染模型中的某一个属性，
    /// 它通常用于渲染被外层封装过的类型
    /// </summary>
    /// <inheritdoc cref="TableRenderInfoBuildPenetration{OuterModel, TrueModel}(ITableRenderInfoBuild{TrueModel}, Func{OuterModel, TrueModel})"/>
    /// <inheritdoc cref="TableRenderInfoBuildPenetration{OuterModel, TrueModel}.TableRenderInfoBuildPenetration(ITableRenderInfoBuild{TrueModel}, Func{OuterModel, TrueModel})"/>
    public static ITableRenderInfoBuild<OuterModel> TableViewerRenderInfoBuildPenetration<OuterModel, TrueModel>
        (ITableRenderInfoBuild<TrueModel> build, Func<OuterModel, TrueModel> penetration)
        where OuterModel : class
        where TrueModel : class
        => new TableRenderInfoBuildPenetration<OuterModel, TrueModel>(build, penetration);
    #endregion
    #endregion
    #region 创建带标题数据
    /// <summary>
    /// 创建一个携带标题的数据，
    /// 它直接封装标题，数据的类型，和数据的值
    /// </summary>
    /// <param name="name">属性的标题</param>
    /// <param name="valueType">属性的类型</param>
    /// <param name="value">属性的值</param>
    /// <returns></returns>
    public static ITitleData TitleData(string name, Type valueType, object? value)
        => new TitleData(name, valueType, value);
    #endregion
}
