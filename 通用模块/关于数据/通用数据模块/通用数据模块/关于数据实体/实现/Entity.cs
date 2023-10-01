namespace System.DataFrancis;

/// <summary>
/// 这个类型是所有强类型实体类的可选基类
/// </summary>
public abstract class Entity : IWithID
{
    #region 公开成员
    #region 数据ID
    public Guid ID { get; set; }
    #endregion
    #endregion
}
