namespace System.DataFrancis.DB;

/// <summary>
/// 这个枚举指示映射模型继承的方式
/// </summary>
public enum EntityInheritMapping
{
    /// <summary>
    /// 每个层次结构一张表和鉴别器配置，
    /// 它适用于大多数情况
    /// </summary>
    TPH,
    /// <summary>
    /// 每个具体类型一张表配置，
    /// 它适用于每个具体类型存在复杂外键关系的情况，
    /// 但是代价是不能用该实体的抽象类型，
    /// 作为其他实体的外键或导航属性
    /// </summary>
    TPC
}
