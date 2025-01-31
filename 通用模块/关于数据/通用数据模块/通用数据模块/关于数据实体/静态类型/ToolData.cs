using System.Linq.Expressions;

namespace System.DataFrancis;

public static partial class ToolData
{
    //这个部分类专门用来声明有关数据实体的工具方法

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
