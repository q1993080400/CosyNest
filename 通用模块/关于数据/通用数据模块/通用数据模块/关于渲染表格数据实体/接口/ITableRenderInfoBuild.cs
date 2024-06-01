namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来生成表格的渲染参数
/// </summary>
/// <typeparam name="Model">表格的模型类型</typeparam>
public interface ITableRenderInfoBuild<Model>
    where Model : class
{
    #region 生成表头渲染参数
    /// <summary>
    /// 返回一个集合，
    /// 它的参数表示表头中的一列的渲染参数
    /// </summary>
    /// <returns></returns>
    IReadOnlyCollection<RenderTableHeaderColumnsInfoBase> GetRenderHeaderColumnsInfo();
    #endregion
    #region 生成表身渲染参数
    /// <summary>
    /// 返回一个集合，
    /// 它的参数表示表身中的一列的渲染参数
    /// </summary>
    /// <param name="model">这个表身的一行所对应的模型</param>
    /// <param name="rowIndex">这个模型所在的行的索引</param>
    /// <returns></returns>
    IReadOnlyCollection<RenderBodyColumnsInfoBase<Model>> GetRenderBodyColumnsInfo(Model model, int rowIndex);
    #endregion
}
