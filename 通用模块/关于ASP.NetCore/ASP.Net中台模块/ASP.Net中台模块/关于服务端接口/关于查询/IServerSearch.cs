namespace Microsoft.AspNetCore;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个服务端搜索接口
/// </summary>
/// <typeparam name="Parameter">搜索方法的参数类型</typeparam>
/// <typeparam name="Obj">要返回的对象类型</typeparam>
public interface IServerSearch<in Parameter, Obj>
{
    #region 搜索对象
    /// <summary>
    /// 搜索所有对象
    /// </summary>
    /// <param name="parameter">搜索的参数</param>
    /// <returns></returns>
    Task<Obj> Search(Parameter parameter);
    #endregion
}
