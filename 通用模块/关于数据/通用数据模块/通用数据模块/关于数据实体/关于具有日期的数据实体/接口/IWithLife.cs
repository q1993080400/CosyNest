using System.Linq.Expressions;

namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个具有寿命的数据实体，
/// 它可以指示实体是否已经过期
/// </summary>
/// <typeparam name="Entity">实体类的类型</typeparam>
public interface IWithLife<Entity>
    where Entity : class, IWithLife<Entity>
{
    #region 获取筛选过期或未过期的实体的表达式
    /// <summary>
    /// 获取一个表达式，它可以用来筛选过期或未过期的实体
    /// </summary>
    /// <typeparam name="DerivativeEntity">实体的派生实体，
    /// 它可以用于为派生类生成表达式</typeparam>
    /// <param name="filterExpire">如果这个值为<see langword="true"/>，
    /// 则筛选已经过期的实体，否则筛选未过期的实体</param>
    /// <returns></returns>
    static abstract Expression<Func<DerivativeEntity, bool>> GetFilterExpression<DerivativeEntity>(bool filterExpire)
        where DerivativeEntity : Entity;
    #endregion
}
