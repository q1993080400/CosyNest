namespace System.Design;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以将自身投影为其他对象
/// </summary>
public interface IProjection<out Obj>
{
    #region 投影对象
    /// <summary>
    /// 将自身投影成其他对象
    /// </summary>
    /// <param name="serviceProvider">一个用于提供服务的对象，
    /// 如果不需要，可以为<see langword="null"/></param>
    /// <returns></returns>
    Obj Projection(IServiceProvider? serviceProvider);
    #endregion
}
