namespace System.DataFrancis;

/// <summary>
/// 这个类型是描述实体数据条件的基类
/// </summary>
/// <typeparam name="Obj">描述的实体数据的类型</typeparam>
public abstract record DataCondition<Obj>
{
    #region 属性访问表达式
    /// <summary>
    /// 获取属性访问表达式，
    /// 它决定应该访问实体类的什么属性，支持递归
    /// </summary>
    public required string PropertyAccess { get; init; }
    #endregion
}
