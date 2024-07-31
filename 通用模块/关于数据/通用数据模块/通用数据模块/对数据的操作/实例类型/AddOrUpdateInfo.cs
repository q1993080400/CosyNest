namespace System.DataFrancis;

/// <summary>
/// 这个记录是添加和修改数据的配置选项
/// </summary>
/// <typeparam name="Data">数据的类型</typeparam>
public sealed record AddOrUpdateInfo<Data>
{
    #region 筛选要添加的数据
    /// <summary>
    /// 这个委托传入数据，返回一个布尔值，
    /// 如果为<see langword="true"/>，表示添加数据，否则表示更新数据，
    /// 如果不指定，则自动判断，在显式指定主键的情况下，必须显式指定这个委托，否则会出现异常
    /// </summary>
    public Func<Data, bool>? IsAddData { get; init; }
    #endregion
    #region 是否更新业务属性
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示应该更新业务属性，否则表示跳过更新
    /// </summary>
    public bool UpdateBusinessProperty { get; init; } = true;
    #endregion
}
