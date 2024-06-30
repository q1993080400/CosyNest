namespace Microsoft.AspNetCore;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来获取筛选条件默认值
/// </summary>
public interface IHasFilterDefaultValue
{
    #region 获取默认值
    /// <summary>
    /// 获取默认值
    /// </summary>
    /// <typeparam name="Obj">默认值的类型</typeparam>
    /// <returns></returns>
    Obj? GetDefaultValue<Obj>();
    #endregion
}
