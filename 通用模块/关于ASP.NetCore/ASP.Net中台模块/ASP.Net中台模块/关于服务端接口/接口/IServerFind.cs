using System.NetFrancis.Http;

namespace Microsoft.AspNetCore;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以在服务端通过ID寻找一个对象
/// </summary>
/// <typeparam name="Obj">要寻找的对象的类型</typeparam>
/// <typeparam name="Parameter">用来搜索对象的类型，
/// 它应该包含实体类的ID，或本身就是ID</typeparam>
public interface IServerFind<Parameter, Obj>
    where Obj : class
{
    #region 寻找对象
    /// <summary>
    /// 根据ID，寻找一个对象，
    /// 如果找不到，则为<see langword="null"/>
    /// </summary>
    /// <param name="withID">这个对象包含对象的ID，或本身就是ID</param>
    /// <returns></returns>
    [HttpMethodPost]
    Task<Obj?> Find(Parameter withID);
    #endregion
}
