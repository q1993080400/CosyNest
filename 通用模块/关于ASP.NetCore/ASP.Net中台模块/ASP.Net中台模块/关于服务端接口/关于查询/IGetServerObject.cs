namespace Microsoft.AspNetCore;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以从服务器上无条件获取一个对象
/// </summary>
/// <typeparam name="Obj">要获取的对象的类型</typeparam>
public interface IGetServerObject<Obj>
{
    #region 获取对象
    /// <summary>
    /// 获取服务器上指定类型的对象，
    /// 不需要任何参数
    /// </summary>
    /// <returns></returns>
    Task<Obj> GetServerObject();
    #endregion
}
