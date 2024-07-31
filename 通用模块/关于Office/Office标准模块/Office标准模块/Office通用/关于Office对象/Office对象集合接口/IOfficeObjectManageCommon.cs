namespace System.Office;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个常用的Office对象管理，
/// 它可以添加空白对象
/// </summary>
/// <inheritdoc cref="IOfficeObjectManage{OfficeObject}"/>
public interface IOfficeObjectManageCommon<OfficeObject> : IOfficeObjectManage<OfficeObject>
        where OfficeObject : IOfficeObject
{
    #region 添加Office对象
    /// <summary>
    /// 添加一个空白的Office对象
    /// </summary>
    /// <returns></returns>
    OfficeObject Add();
    #endregion
}
