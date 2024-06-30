namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来更新业务属性
/// </summary>
public interface IUpdateBusinessProperty
{
    #region 说明文档
    /*问：这个接口有什么用处？
      答：有些实体类属性可以通过其他属性计算出来，
      但是业务逻辑复杂，不适合作为数据库计算列，
      所以作者设计了这个接口，如果某个实体类实现了本接口，
      在保存它之前，会自动调用这个方法更新这些属性*/
    #endregion
    #region 更新业务属性
    /// <summary>
    /// 更新自身的业务属性，
    /// 并递归更新关联实体的业务属性
    /// </summary>
    /// <param name="caller">指示这个方法的调用方类型，
    /// 指定它可以防止无限递归，
    /// 如果是外部调用，可以为空集合</param>
    void UpdateBusinessProperty(HashSet<Type> caller);
    #endregion
}
