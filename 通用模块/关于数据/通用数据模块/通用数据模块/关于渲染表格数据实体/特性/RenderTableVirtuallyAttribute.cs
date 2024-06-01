using System.Reflection;

namespace System.DataFrancis;

/// <summary>
/// 这个特性表示某个模型存在一个虚拟的表格列，
/// 它不映射到具体的某个属性
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class RenderTableVirtuallyAttribute : RenderTableBaseAttribute
{
    #region 抽象成员实现
    #region 获取表头渲染参数
    public override RenderTableHeaderColumnsInfoBase GetTableHeaderColumnsInfo(MemberInfo attachment)
    {
        CheckMember(attachment);
        return new RenderTableHeaderColumnsInfoCustom()
        {
            Name = HeaderName,
        };
    }
    #endregion
    #region 获取表身渲染参数
    public override RenderBodyColumnsInfoBase<Model> GetRenderBodyColumnsInfo<Model>(MemberInfo attachment, Model model, int rowIndex)
        where Model : class
    {
        CheckMember(attachment);
        return new RenderTableBodyColumnsInfoCustom<Model>()
        {
            ColumnsName = HeaderName,
            RowIndex = rowIndex,
            TableModel = model,
            IsLong = IsLong
        };
    }
    #endregion
    #endregion
    #region 内部成员
    #region 检查成员是否为类型
    /// <summary>
    /// 检查一个成员是否为类型，
    /// 如果不是，则引发异常
    /// </summary>
    /// <param name="attachment">待检查的成员</param>
    private static void CheckMember(MemberInfo attachment)
    {
        if (attachment is not Type)
            throw new NotSupportedException($"{attachment}不是一个类型对象，无法将其转换为渲染参数");
    }
    #endregion
    #endregion
}
