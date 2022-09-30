namespace System.Design;

/// <summary>
/// 凡是实现本接口的类型，
/// 都可以视为一个对象池
/// </summary>
/// <typeparam name="Obj">对象池中的对象类型</typeparam>
public interface IPool<out Obj> : IInstruct, IDisposable
    where Obj : class
{
    #region 获取对象
    /// <summary>
    /// 从对象池中获取对象
    /// </summary>
    /// <returns></returns>
    IPooledObject<Obj> Get();
    #endregion
}
