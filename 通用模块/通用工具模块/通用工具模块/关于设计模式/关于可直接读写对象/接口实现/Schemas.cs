namespace System.Design.Direct;

/// <summary>
/// 这个类型是<see cref="ISchema"/>的实现，
/// 可以用来表示数据架构
/// </summary>
/// <remarks>
/// 使用指定的架构初始化对象
/// </remarks>
/// <param name="schema">指定的架构</param>
sealed class Schemas(IReadOnlyDictionary<string, Type> schema) : ISchema
{
    #region 返回数据的架构
    public IReadOnlyDictionary<string, Type> Schema { get; } = schema;

    #endregion
}
