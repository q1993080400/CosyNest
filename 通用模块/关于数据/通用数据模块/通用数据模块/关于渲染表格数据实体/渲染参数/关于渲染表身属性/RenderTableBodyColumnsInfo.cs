using System.Reflection;

namespace System.DataFrancis;

/// <summary>
/// 这个记录是用来渲染表格表身中的一列的参数，
/// 它表示有一个真实的属性映射到这个列
/// </summary>
/// <inheritdoc cref="RenderBodyColumnsInfoBase{Model}"/>
public sealed record RenderTableBodyColumnsInfo<Model> : RenderBodyColumnsInfoBase<Model>, ITitleData
    where Model : class
{
    #region 要渲染的属性
    /// <summary>
    /// 获取要渲染的属性
    /// </summary>
    public required PropertyInfo PropertyInfo { get; init; }
    #endregion
    #region 获取属性的值的类型
    public Type ValueType
         => PropertyInfo.PropertyType;
    #endregion
    #region 获取属性的值
    #region 非泛型
    public object? Value
        => PropertyInfo.GetValue(TableModel);
    #endregion
    #region 泛型方法
    public Obj? GetValue<Obj>()
        => Value.To<Obj>();
    #endregion
    #endregion
}
