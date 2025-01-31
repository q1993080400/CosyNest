namespace System.Design;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以将自身投影为其他对象
/// </summary>
/// <typeparam name="Obj">投影的目标对象</typeparam>
public interface IProjection<out Obj>
{
    #region 投影对象
    /// <summary>
    /// 将自身投影为其他对象
    /// </summary>
    /// <returns></returns>
    Obj Projection();
    #endregion
}
