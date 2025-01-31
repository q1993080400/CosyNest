namespace System.Design;

/// <typeparam name="Parameter">投影所需要的参数</typeparam>
/// <inheritdoc cref="IProgress{T}"/>
public interface IProjection<out Obj, in Parameter>
{
    #region 投影对象
    /// <summary>
    /// 将自身投影为其他对象
    /// </summary>
    /// <param name="parameter">投影所需要的参数</param>
    /// <returns></returns>
    Obj Projection(Parameter parameter);
    #endregion
}
