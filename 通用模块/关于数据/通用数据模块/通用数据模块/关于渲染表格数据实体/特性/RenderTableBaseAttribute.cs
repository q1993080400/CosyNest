using System.Reflection;

namespace System.DataFrancis;

/// <summary>
/// 这个特性是表格渲染特性的基类
/// </summary>
public abstract class RenderTableBaseAttribute : Attribute
{
    #region 顺序
    /// <summary>
    /// 显式指定要显示的顺序
    /// </summary>
    public int Order { get; init; }
    #endregion
    #region 表头名称
    /// <summary>
    /// 获取这一列对应的表头的名称
    /// </summary>
    public required string HeaderName { get; init; }
    #endregion
    #region 渲染出的结果是否很长
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示最终渲染出的表身数据可能会很长，
    /// 它会影响到一些布局
    /// </summary>
    public bool IsLong { get; init; }
    #endregion
    #region 抽象成员
    #region 获取表头渲染参数
    /// <summary>
    /// 通过这个特性，获取表头渲染参数
    /// </summary>
    /// <param name="attachment">这个特性所依附的类型成员对象</param>
    /// <returns></returns>
    public abstract RenderTableHeaderColumnsInfoBase GetTableHeaderColumnsInfo(MemberInfo attachment);
    #endregion
    #region 获取表身渲染参数
    /// <summary>
    /// 通过这个特性，获取表身渲染参数
    /// </summary>
    /// <typeparam name="Model">表格模型的类型</typeparam>
    /// <param name="attachment">这个特性所依附的类型成员对象</param>
    /// <param name="model">表格模型的实例</param>
    /// <param name="rowIndex">表身所在的行的索引</param>
    /// <returns></returns>
    public abstract RenderBodyColumnsInfoBase<Model> GetRenderBodyColumnsInfo<Model>(MemberInfo attachment, Model model, int rowIndex)
        where Model : class;
    #endregion
    #endregion
}
