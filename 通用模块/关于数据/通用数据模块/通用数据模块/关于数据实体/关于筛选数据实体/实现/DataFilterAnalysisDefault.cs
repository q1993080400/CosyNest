using System.Linq.Expressions;
using System.Reflection;

using static System.Linq.Expressions.Expression;

namespace System.DataFrancis;

/// <summary>
/// 这个类型是<see cref="IDataFilterAnalysis"/>的默认实现
/// </summary>
sealed class DataFilterAnalysisDefault : IDataFilterAnalysis
{
    #region 公开成员
    #region 解析为查询表达式
    public IOrderedQueryable<Obj> Analysis<Obj>(IQueryable<Obj> dataSource, DataFilterDescription<Obj> description, Func<IQueryable<Obj>, IOrderedQueryable<Obj>>? sortFunction = null)
    {
        var query = GenerateQueryExpression(description, dataSource);
        var sort = sortFunction?.Invoke(query) ?? query.OrderBy(x => 0);
        return GenerateSortExpression(description, sort);
    }
    #endregion
    #endregion
    #region 内部成员
    #region 生成查询表达式
    #region 正式方法
    /// <summary>
    /// 生成一个用来查询的表达式
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="IDataFilterAnalysis.Analysis{Obj}(IQueryable{Obj}, DataFilterDescription{Obj}, Func{IQueryable{Obj}, IOrderedQueryable{Obj}}?)"/>
    private static IQueryable<Obj> GenerateQueryExpression<Obj>(DataFilterDescription<Obj> description, IQueryable<Obj> dataSource)
    {
        var condition = description.QueryCondition.ToArray();
        if (condition.Any())
        {
            var where = GenerateWhereExpression(condition);
            return dataSource.Where(where);
        }
        return dataSource;
    }
    #endregion
    #region 生成Where表达式树
    /// <summary>
    /// 生成一个可以用于<see cref="Queryable.Where{TSource}(IQueryable{TSource}, Expression{Func{TSource, bool}})"/>的表达式树
    /// </summary>
    /// <typeparam name="Obj">实体类的类型</typeparam>
    /// <param name="condition">查询条件</param>
    /// <returns></returns>
    private static Expression<Func<Obj, bool>> GenerateWhereExpression<Obj>(IEnumerable<QueryCondition<Obj>> condition)
    {
        var parameter = Parameter(typeof(Obj));
        var body = GenerateWhereBodyExpression(condition, parameter);
        return Lambda<Func<Obj, bool>>(body, new[] { parameter });
    }
    #endregion
    #region 生成Where表达式树的Body部分
    /// <summary>
    /// 生成Where表达式树的Body部分
    /// </summary>
    /// <param name="parameter">Where表达式树的参数</param>
    /// <returns></returns>
    /// <inheritdoc cref="GenerateWhereExpression{Obj}(IEnumerable{QueryCondition{Obj}})"/>
    private static Expression GenerateWhereBodyExpression<Obj>(IEnumerable<QueryCondition<Obj>> condition, ParameterExpression parameter)
        => condition.Select(x => GenerateWhereBodySingleExpression(x, parameter)).
        Aggregate((Expression?)null,
            (seed, expression) => seed is null ? expression : AndAlso(seed, expression))!;
    #endregion
    #region 生成单个查询条件的表达式
    /// <summary>
    /// 生成适用于单个查询条件的表达式
    /// </summary>
    /// <param name="condition">要生成表达式树的单个查询条件</param>
    /// <returns></returns>
    /// <inheritdoc cref="GenerateWhereBodyExpression{Obj}(IEnumerable{QueryCondition{Obj}}, ParameterExpression)"/>
    private static Expression GenerateWhereBodySingleExpression<Obj>(QueryCondition<Obj> condition, ParameterExpression parameter)
    {
        var propertyAccess = condition.PropertyAccess.Split('.');
        var single = GenerateWhereBodyPartExpression(parameter, propertyAccess, condition.LogicalOperator, condition.CompareValue);
        return single;
    }
    #endregion
    #region 生成单个查询条件中每个部分的表达式
    /// <summary>
    /// 生成单个查询条件中每个子部分的表达式
    /// </summary>
    /// <param name="expression">上一个递归表达式</param>
    /// <param name="propertyAccess">这个数组中的每一个元素都代表访问一个属性</param>
    /// <param name="logicalOperator">最终用来比较结果的逻辑运算符</param>
    /// <param name="compareValue">用来和实体类属性进行比较的值</param>
    /// <returns></returns>
    private static Expression GenerateWhereBodyPartExpression(Expression expression, string[] propertyAccess,
        LogicalOperator logicalOperator, object? compareValue)
    {
        if (!propertyAccess.Any())
            return GenerateCompareExpression(expression, logicalOperator, compareValue);
        var nextProperty = propertyAccess[1..];
        var propertyName = propertyAccess[0];
        var property = expression.Type.GetProperty(propertyName) ??
            throw new NotSupportedException($"未能在{expression.Type}中找到属性{propertyName}");
        var propertyType = property.PropertyType;
        var propertyAccessExpression = Property(expression, property);
        if (propertyType.IsRealizeGeneric(typeof(ICollection<>)).IsRealize)
        {
            var parameter = Parameter(propertyType.GetGenericArguments()[0]);
            var body = GenerateWhereBodyPartExpression(parameter, nextProperty, logicalOperator, compareValue);
            return Call(MethodAny, propertyAccessExpression, body);
        }
        return GenerateWhereBodyPartExpression(propertyAccessExpression, nextProperty, logicalOperator, compareValue);
    }
    #endregion
    #region 生成最终进行比较的表达式
    /// <summary>
    /// 生成最终进行比较的表达式
    /// </summary>
    /// <param name="left">进行比较的二元运算符的左表达式</param>
    /// <returns></returns>
    /// <inheritdoc cref="GenerateWhereBodyPartExpression(Expression, string[], LogicalOperator, object?)"/>
    private static Expression GenerateCompareExpression(Expression left, LogicalOperator logicalOperator, object? compareValue)
    {
        Expression<Func<decimal?, decimal, bool>> a = (x, y) => x == y;
        var @const = Constant(compareValue);
        #region 将转换左右节点的本地函数
        Expression Fun(Func<Expression, Expression, Expression> fun)
           => left.Type.IsEnum || @const.Type.IsEnum ?
           fun(Convert(left, typeof(int)), Convert(@const, typeof(int))) :
           fun(left, Convert(@const, left.Type));
        #endregion
        return logicalOperator switch
        {
            LogicalOperator.Equal => Fun(Equal),
            LogicalOperator.NotEqual => Fun(NotEqual),
            LogicalOperator.GreaterThan => Fun(GreaterThan),
            LogicalOperator.GreaterThanOrEqual => Fun(GreaterThanOrEqual),
            LogicalOperator.LessThan => Fun(LessThan),
            LogicalOperator.LessThanOrEqual => Fun(LessThanOrEqual),
            LogicalOperator.Contain when @const.Type == typeof(string) => Call(left, MethodContains, @const),
            LogicalOperator.Contain => throw new NotSupportedException($"表达式的逻辑运算符是{LogicalOperator.Contain}，但是它的主体部分不是string"),
            var l => throw new NotSupportedException($"未能识别{l}类型的表达式")
        };
    }
    #endregion
    #region 缓存Any方法的反射信息
    /// <summary>
    /// 缓存<see cref="Enumerable.Any{TSource}(IEnumerable{TSource}, Func{TSource, bool})"/>方法的反射信息，
    /// 它在解析表达式树的时候会被用到
    /// </summary>
    private static MethodInfo MethodAny { get; }
        = typeof(Enumerable).GetTypeData().
        MethodDictionary[nameof(Enumerable.Any)].SingleOrDefault(x => x.GetParameters().Length is 2) ??
        throw new NotSupportedException($"{nameof(Enumerable)}中存在多个名为{nameof(Enumerable.Any)}，且参数数量为2的方法，" +
            $"您可以检查下，是不是升级Net版本后新增了这个方法？如果是，请将这个属性重构");
    #endregion
    #region 缓存Contains方法的反射信息
    /// <summary>
    /// 缓存<see cref="string.Contains(string)"/>方法的反射信息，
    /// 它在解析表达式树的时候会被用到
    /// </summary>
    private static MethodInfo MethodContains { get; }
        = typeof(string).GetTypeData().FindMethod(nameof(string.Contains), CreateReflection.MethodSignature(typeof(bool), typeof(string)));
    #endregion
    #endregion
    #region 生成排序表达式
    #region 正式方法
    /// <summary>
    /// 生成一个用来排序的表达式
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="IDataFilterAnalysis.Analysis{Obj}(IQueryable{Obj}, DataFilterDescription{Obj})"/>
    private static IOrderedQueryable<Obj> GenerateSortExpression<Obj>(DataFilterDescription<Obj> description, IOrderedQueryable<Obj> dataSource)
    {
        var condition = description.SortCondition.ToArray();
        if (condition.Any())
        {
            return condition.Aggregate(dataSource, GenerateSortSingleExpression);
        }
        return dataSource;
    }
    #endregion
    #region 生成处理单个排序条件的表达式
    /// <summary>
    /// 生成处理单个排序条件的表达式
    /// </summary>
    /// <param name="seed">种子对象，也就是上一个被处理的表达式</param>
    /// <param name="condition">执行排序的条件</param>
    /// <returns></returns>
    /// <inheritdoc cref="GenerateSortExpression{Obj}(DataFilterDescription{Obj}, IOrderedQueryable{Obj})"/>
    private static IOrderedQueryable<Obj> GenerateSortSingleExpression<Obj>(IOrderedQueryable<Obj> seed, SortCondition<Obj> condition)
    {
        var propertyAccess = condition.PropertyAccess.Split('.');
        var parameter = Parameter(typeof(Obj));
        var body = propertyAccess.Aggregate((Expression)parameter, Property);
        var lambda = Lambda<Func<Obj, object>>(Convert(body, typeof(object)), parameter);
        return condition.IsAscending ?
            seed.ThenBy(lambda) :
            seed.ThenByDescending(lambda);
    }
    #endregion
    #endregion
    #endregion
}
