using System.DataFrancis;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件是一个表格视图，
/// 它能够正确地渲染表格
/// </summary>
/// <typeparam name="Model">表格模型的类型</typeparam>
public sealed partial class TableViewer<Model> : ComponentBase
    where Model : class
{
    #region 组件参数
    #region 渲染参数生成器
    /// <summary>
    /// 获取渲染参数生成器
    /// </summary>
    [Parameter]
    public ITableRenderInfoBuild<Model> RenderInfoBuild { get; set; }
        = CreateDataObj.TableViewerRenderInfoBuild<Model>();
    #endregion
    #region 用来渲染组件的委托
    /// <summary>
    /// 用来渲染组件的委托
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderTableViewerInfo> RenderComponent { get; set; }
    #endregion
    #region 用来渲染表头的委托
    /// <summary>
    /// 用来渲染表头的委托
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderTableHeaderInfo> RenderHeader { get; set; }
    #endregion
    #region 用来渲染表格中每一行的委托
    /// <summary>
    /// 用来渲染表格中每一行的委托
    /// </summary>
    [Parameter]
    [EditorRequired]
    public RenderFragment<RenderTableRowInfo<Model>> RenderBodyRows { get; set; }
    #endregion
    #region 枚举所有模型
    /// <summary>
    /// 枚举要渲染的所有模型
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IEnumerable<Model> Models { get; set; }
    #endregion
    #region 数据编号对象
    /// <summary>
    /// 获取数据编号对象，
    /// 它能够正确地为元素编号
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IElementNumber ElementNumber { get; set; }
    #endregion
    #endregion
    #region 内部成员
    #region 组件的ID
    /// <summary>
    /// 获取组件的ID
    /// </summary>
    private string ID { get; } = CreateASP.JSObjectName();
    #endregion
    #region 获取渲染参数
    /// <summary>
    /// 获取本组件的渲染参数
    /// </summary>
    /// <returns></returns>
    private RenderTableViewerInfo GetRenderInfo()
    {
        var renderBodyRow = Models.
            Select((x, index) =>
            {
                var info = new RenderTableRowInfo<Model>()
                {
                    RenderBodyColumnsInfo = RenderInfoBuild.GetRenderBodyColumnsInfo(x, index),
                    RowIndex = index,
                    TableModel = x,
                    ID = ElementNumber.GetElementID(index)
                };
                return RenderBodyRows(info);
            }).ToArray();
        var renderHeaderColumnsInfo = RenderInfoBuild.GetRenderHeaderColumnsInfo();
        var renderHeaderInfo = new RenderTableHeaderInfo()
        {
            RenderHeaderColumns = renderHeaderColumnsInfo
        };
        return new()
        {
            ColumnsCount = renderHeaderColumnsInfo.Count,
            RenderRow = renderBodyRow,
            RenderHeader = RenderHeader(renderHeaderInfo),
            ElementNumber = ElementNumber
        };
    }
    #endregion
    #endregion
}
