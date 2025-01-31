namespace System.DataFrancis;

/// <summary>
/// 这个记录是添加和修改数据的配置选项
/// </summary>
/// <typeparam name="Data">数据的类型</typeparam>
public sealed record AddOrUpdateInfo<Data>
{
    #region 筛选要添加的数据
    #region 正式属性
    /// <summary>
    /// 这个委托传入数据，返回一个布尔值，
    /// 如果为<see langword="true"/>，表示添加数据，否则表示更新数据，
    /// 如果不指定，则自动判断，在显式指定主键的情况下，必须显式指定这个委托，否则会出现异常
    /// </summary>
    public Func<Data, bool>? IsAddData { get; init; }
    #endregion
    #region 预设值：添加所有数据
    /// <summary>
    /// 这个属性是<see cref="IsAddData"/>的预设值，
    /// 它指示应该添加所有数据
    /// </summary>
    public static Func<Data, bool> AddAllData { get; }
    = static _ => true;
    #endregion
    #endregion
    #region 是否更新业务属性
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示应该更新业务属性，否则表示跳过更新
    /// </summary>
    public bool UpdateBusinessProperty { get; init; } = true;
    #endregion
}
