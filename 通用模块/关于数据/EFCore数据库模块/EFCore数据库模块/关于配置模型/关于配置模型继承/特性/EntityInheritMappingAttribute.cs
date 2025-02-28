namespace System.DataFrancis.DB;

/// <summary>
/// 这个特性被放置在抽象实体类型上，
/// 它指示映射实体继承关系的方式
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class EntityInheritMappingAttribute : Attribute
{
    #region 实体继承映射方式
    /// <summary>
    /// 指示实体的继承映射方式
    /// </summary>
    public required EntityInheritMapping EntityInheritMapping { get; init; }
    #endregion
}
