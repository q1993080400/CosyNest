namespace System.DataFrancis;

/// <summary>
/// 这个记录是用来渲染表格表身中的一列的参数，
/// 它表示这是一个虚拟列，没有属性与之直接映射
/// </summary>
/// <inheritdoc cref="RenderBodyColumnsInfoBase{Model}"/>
public sealed record RenderTableBodyColumnsInfoCustom<Model> : RenderBodyColumnsInfoBase<Model>
    where Model : class
{

}
