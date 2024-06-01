using System.Reflection;

namespace System.DataFrancis;

/// <summary>
/// 这个特性表示某个属性被映射到表格组件中的一列
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class RenderTableAttribute : RenderTableBaseAttribute
{
    #region 抽象成员实现
    #region 获取表头渲染参数
    public override RenderTableHeaderColumnsInfoBase GetTableHeaderColumnsInfo(MemberInfo attachment)
    {
        var property = GetPropertyInfo(attachment);
        return new RenderTableHeaderColumnsInfo()
        {
            Name = HeaderName,
            PropertyInfo = property,
        };
    }
    #endregion
    #region 获取表身渲染参数
    public override RenderBodyColumnsInfoBase<Model> GetRenderBodyColumnsInfo<Model>(MemberInfo attachment, Model model, int rowIndex)
        where Model : class
    {
        var property = GetPropertyInfo(attachment);
        return new RenderTableBodyColumnsInfo<Model>()
        {
            ColumnsName = HeaderName,
            RowIndex = rowIndex,
            TableModel = model,
            PropertyInfo = property,
            IsLong = IsLong
        };
    }
    #endregion
    #endregion
    #region 内部成员
    #region 将成员转换为属性
    /// <summary>
    /// 将成员转换为属性
    /// </summary>
    /// <param name="attachment">待转换的成员</param>
    /// <returns></returns>
    private static PropertyInfo GetPropertyInfo(MemberInfo attachment)
        => attachment is PropertyInfo property ?
        property :
        throw new NotSupportedException($"{attachment}不是属性，不能被转换为渲染参数");
    #endregion
    #endregion
}
