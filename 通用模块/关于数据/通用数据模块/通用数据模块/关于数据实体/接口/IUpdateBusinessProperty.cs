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
      在保存它之前，会自动调用这个方法更新这些属性
    
      问：如果实体存在关联关系，那么如何更新它们的业务属性？
      答：为了避免很多奇怪的问题，对这个问题实行一刀切设计，
      调用UpdateBusinessProperty时，只更新自身，不更新其他任何实体，
      实现UpdateBusinessProperty的时候，也只更新自身，不能递归更新*/
    #endregion
    #region 更新业务属性
    /// <summary>
    /// 更新自身的业务属性
    /// </summary>
    void UpdateBusinessProperty();
    #endregion
}
