namespace System.Design;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个对象池中的对象，
/// 当调用它的<see cref="IDisposable.Dispose"/>方法时，
/// 对象会被归还进池中
/// </summary>
/// <typeparam name="Obj">对象池提供的对象的类型</typeparam>
public interface IPooledObject<out Obj> : IInstruct, IDisposable
     where Obj : class
{
    #region 说明文档
    /*问：为什么不能从对象池中直接获取对象，
      而是要通过本接口把它封装一遍？
      答：这是为了更方便的归还对象，因为本接口实现了IDisposable，
      因此归还对象仅需使用using语句即可*/
    #endregion
    #region 获取对象
    /// <summary>
    /// 获取对象池中的对象
    /// </summary>
    Obj Get { get; }
    #endregion
}
