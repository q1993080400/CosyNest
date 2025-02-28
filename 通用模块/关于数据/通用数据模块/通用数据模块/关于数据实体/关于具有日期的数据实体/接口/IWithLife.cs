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

/// <summary>
/// 这个静态类是<see cref="IWithLife{Entity}"/>的辅助类
/// </summary>
public static class IWithLife
{
    #region IWithLife的模板实现
    #region 指定天数
    /// <summary>
    /// 实现<see cref="IWithLife{Entity}.GetFilterExpression{DerivativeEntity}(bool)"/>的模板方法，
    /// 它指定一个以天为单位的期限，作为过期的依据
    /// </summary>
    /// <param name="days">实体的寿命，以天为单位</param>
    /// <returns></returns>
    /// <inheritdoc cref="IWithLife{Entity}.GetFilterExpression{DerivativeEntity}(bool)"/>
    public static Expression<Func<DerivativeEntity, bool>> GetFilterExpression<Entity, DerivativeEntity>(bool filterExpire, double days)
        where Entity : class, IWithLife<Entity>, IWithDate
        where DerivativeEntity : Entity
    {
        if (days <= 0)
            throw new ArgumentOutOfRangeException($"{nameof(days)}的值是{days}，它必须大于0");
        var now = DateTimeOffset.Now;
        return filterExpire ?
            derivativeEntity => derivativeEntity.Date.AddDays(days) < now :
            derivativeEntity => derivativeEntity.Date.AddDays(days) >= now;
    }
    #endregion
    #region 指定时间间隔
    /// <summary>
    /// 实现<see cref="IWithLife{Entity}.GetFilterExpression{DerivativeEntity}(bool)"/>的模板方法，
    /// 它指定一个时间间隔作为期限，作为过期的依据
    /// </summary>
    /// <param name="lifeTime">实体的寿命</param>
    /// <returns></returns>
    /// <inheritdoc cref="IWithLife{Entity}.GetFilterExpression{DerivativeEntity}(bool)"/>
    public static Expression<Func<DerivativeEntity, bool>> GetFilterExpression<Entity, DerivativeEntity>(bool filterExpire, TimeSpan lifeTime)
        where Entity : class, IWithLife<Entity>, IWithDate
        where DerivativeEntity : Entity
    {
        var days = lifeTime.TotalDays;
        return GetFilterExpression<Entity, DerivativeEntity>(filterExpire, days);
    }
    #endregion
    #endregion
}