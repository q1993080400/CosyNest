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
    public IOrderedQueryable<Obj> Analysis<Obj>(DataFilterAnalysisInfo<Obj> info)
    {
        var description = info.Description;
        var data = info.DataSource;
        var query = GenerateQueryExpression(info);
        var sort = info.SortFunction?.Invoke(query) ?? query.OrderBy(static x => 0);
        return GenerateSortExpression(description.SortCondition, info.SkipVirtualization, info.GenerateVirtuallySort, sort);
    }
    #endregion
    #endregion
    #region 内部成员
    #region 生成查询表达式
    #region 正式方法
    /// <summary>
    /// 生成一个用来查询的表达式
    /// </summary>
    /// <param name="info">用来生成查询表达式的参数</param>
    /// <returns></returns>
    private static IQueryable<Obj> GenerateQueryExpression<Obj>(DataFilterAnalysisInfo<Obj> info)
    {
        var dataSource = info.DataSource;
        var condition = info.Description.QueryCondition;
        var (isVirtually, isTrue) = condition.Split(static x => x.IsVirtually);
        if (isTrue.Count > 0)
        {
            var where = GenerateWhereExpression<Obj>(isTrue, info.Reconsitution);
            dataSource = dataSource.Where(where);
        }
        if (info.SkipVirtualization)
            return dataSource;
        if (isVirtually.Count > 0)
        {
            var generateVirtuallyQuery = info.GenerateVirtuallyQuery ??
                throw new NotSupportedException("存在虚拟查询条件，但是没有指定转换虚拟查询条件的函数");
            foreach (var queryCondition in isVirtually)
            {
                var virtuallyQuery = generateVirtuallyQuery(queryCondition);
                dataSource = dataSource.Where(virtuallyQuery);
            }
        }
        return dataSource;
    }
    #endregion
    #region 生成Where表达式树
    /// <summary>
    /// 生成一个可以用于<see cref="Queryable.Where{TSource}(IQueryable{TSource}, Expression{Func{TSource, bool}})"/>的表达式树
    /// </summary>
    /// <param name="condition">查询条件</param>
    /// <param name="reconsitution">这个委托允许重构生成的每个查询表达式，
    /// 并返回重构后的表达式，如果为<see langword="null"/>，则不进行重构</param>
    /// <returns></returns>
    private static Expression<Func<Obj, bool>> GenerateWhereExpression<Obj>(IEnumerable<QueryCondition> condition, Func<QueryConditionReconsitutionInfo, Expression>? reconsitution)
    {
        var parameter = Parameter(typeof(Obj));
        var body = GenerateWhereBodyExpression(condition, parameter, reconsitution);
        return Lambda<Func<Obj, bool>>(body, [parameter]);
    }
    #endregion
    #region 生成Where表达式树的Body部分
    /// <summary>
    /// 生成Where表达式树的Body部分
    /// </summary>
    /// <param name="parameter">Where表达式树的参数</param>
    /// <returns></returns>
    /// <inheritdoc cref="GenerateWhereExpression{Obj}(IEnumerable{QueryCondition}, Func{QueryConditionReconsitutionInfo, Expression}?)"/>
    private static Expression GenerateWhereBodyExpression(IEnumerable<QueryCondition> condition, ParameterExpression parameter,
        Func<QueryConditionReconsitutionInfo, Expression>? reconsitution)
        => condition.Select(x => GenerateWhereBodySingleExpression(x, parameter, reconsitution)).
        Aggregate((Expression?)null,
            (seed, expression) => seed is null ? expression : AndAlso(seed, expression))!;
    #endregion
    #region 生成单个查询条件的表达式
    /// <summary>
    /// 生成适用于单个查询条件的表达式
    /// </summary>
    /// <param name="condition">要生成表达式树的单个查询条件</param>
    /// <returns></returns>
    /// <inheritdoc cref="GenerateWhereBodyExpression(IEnumerable{QueryCondition}, ParameterExpression, Func{QueryConditionReconsitutionInfo, Expression}?)"/>
    private static Expression GenerateWhereBodySingleExpression(QueryCondition condition, ParameterExpression parameter,
        Func<QueryConditionReconsitutionInfo, Expression>? reconsitution)
    {
        var propertyAccess = condition.Identification.Split('.');
        var single = GenerateWhereBodyPartExpression(parameter, propertyAccess, condition.LogicalOperator, condition.CompareValue);
        if (reconsitution is null)
            return single;
        var info = new QueryConditionReconsitutionInfo()
        {
            Expression = single,
            QueryCondition = condition,
            Parameter = parameter
        };
        var reconsitutionExpression = reconsitution(info);
        return reconsitutionExpression;
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
        if (propertyAccess.Length == 0)
            return GenerateCompareExpression(expression, logicalOperator, compareValue);
        var propertyName = propertyAccess[0];
        var property = expression.Type.GetProperty(propertyName) ??
            throw new NotSupportedException($"未能在{expression.Type}中找到属性{propertyName}");
        var propertyType = property.PropertyType;
        var propertyAccessExpression = Property(expression, property);
        var nextProperty = propertyAccess[1..];
        if (propertyType.IsRealizeGeneric(typeof(ICollection<>)).IsRealize)
        {
            var elementType = propertyType.GetGenericArguments()[0];
            var parameter = Parameter(elementType);
            var body = GenerateWhereBodyPartExpression(parameter, nextProperty, logicalOperator, compareValue);
            var lambdaType = typeof(Func<,>).MakeGenericType(elementType, typeof(bool));
            var lambda = Lambda(lambdaType, body, parameter);
            return Call(MethodAny.MakeGenericMethod(elementType), propertyAccessExpression, lambda);
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
        var @const = Constant(compareValue);
        var leftType = left.Type;
        var leftTrueType = Nullable.GetUnderlyingType(leftType) ?? leftType;
        var isNullStruct = leftType != leftTrueType;
        #region 将转换左右节点的本地函数
        Expression ConvertNode(Func<Expression, Expression, Expression> fun, bool isComparisonOperator = false)
        {
            if (leftTrueType.IsEnum)
            {
                var @enum = compareValue switch
                {
                    int num => Enum.Parse(leftTrueType, num.ToString()),
                    string text => Enum.Parse(leftTrueType, text),
                    null when isNullStruct => null,
                    var obj => throw new NotSupportedException($"无法将{obj}类型和枚举进行比较")
                };
                var enumExpression = Constant(@enum, leftType);
                if (isComparisonOperator)
                {
                    var comparisonType = isNullStruct ? typeof(int) : typeof(int?);
                    return fun(Convert(left, comparisonType), Convert(enumExpression, comparisonType));
                }
                return fun(left, Convert(enumExpression, leftType));
            }
            return fun(left, Convert(@const, leftType));
        }
        #endregion
        return logicalOperator switch
        {
            LogicalOperator.Equal => ConvertNode(Equal),
            LogicalOperator.NotEqual => ConvertNode(NotEqual),
            LogicalOperator.GreaterThan => ConvertNode(GreaterThan, true),
            LogicalOperator.GreaterThanOrEqual => ConvertNode(GreaterThanOrEqual, true),
            LogicalOperator.LessThan => ConvertNode(LessThan, true),
            LogicalOperator.LessThanOrEqual => ConvertNode(LessThanOrEqual, true),
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
        = typeof(Enumerable).GetMethods(BindingFlags.Public | BindingFlags.Static).
        SingleOrDefaultSecure(static x => x.Name is nameof(Enumerable.Any) && x.GetParameters().Length is 2) ??
        throw new NotSupportedException($"{nameof(Enumerable)}中存在多个名为{nameof(Enumerable.Any)}，且参数数量为2的方法，" +
            $"您可以检查下，是不是升级Net版本后新增了这个方法？如果是，请将这个属性重构");
    #endregion
    #region 缓存Contains方法的反射信息
    /// <summary>
    /// 缓存<see cref="string.Contains(string)"/>方法的反射信息，
    /// 它在解析表达式树的时候会被用到
    /// </summary>
    private static MethodInfo MethodContains { get; }
        = typeof(string).GetMethod(nameof(string.Contains), BindingFlags.Public | BindingFlags.Instance, [typeof(string)])!;
    #endregion
    #endregion
    #region 生成排序表达式
    #region 正式方法
    /// <summary>
    /// 生成一个用来排序的表达式
    /// </summary>
    /// <typeparam name="Obj">表达式的对象类型</typeparam>
    /// <param name="sortConditions">这个集合描述所有排序表达式</param>
    /// <param name="skipVirtualization">如果这个值为<see langword="true"/>，
    /// 则跳过所有虚拟化条件，由用户自行处理它们</param>
    /// <param name="generateVirtuallySort">构造虚拟排序条件的函数，
    /// 它的第一个参数是虚拟排序条件，第二个参数是当前排序好的表达式，返回值是经过虚拟排序的表达式</param>
    /// <param name="dataSource">数据源对象</param>
    /// <returns></returns>
    private static IOrderedQueryable<Obj> GenerateSortExpression<Obj>
        (IReadOnlyCollection<SortCondition> sortConditions, bool skipVirtualization,
        Func<SortCondition, IOrderedQueryable<Obj>, IOrderedQueryable<Obj>>? generateVirtuallySort,
        IOrderedQueryable<Obj> dataSource)
    {
        var (isVirtually, isTrue) = sortConditions.Split(static x => x.IsVirtually);
        dataSource = isTrue.Aggregate(dataSource, GenerateSortSingleExpression);
        if (skipVirtualization)
            return dataSource;
        if (isVirtually.Count > 0)
        {
            if (generateVirtuallySort is null)
                throw new NotSupportedException("存在虚拟排序条件，但是没有指定转换虚拟排序条件的函数");
            foreach (var sortCondition in isVirtually)
            {
                dataSource = generateVirtuallySort(sortCondition, dataSource);
            }
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
    private static IOrderedQueryable<Obj> GenerateSortSingleExpression<Obj>(IOrderedQueryable<Obj> seed, SortCondition condition)
    {
        var propertyAccess = condition.Identification.Split('.');
        var parameter = Parameter(typeof(Obj));
        var body = propertyAccess.Aggregate((Expression)parameter, Property);
        var lambda = Lambda<Func<Obj, object>>(Convert(body, typeof(object)), parameter);
        return condition.SortStatus switch
        {
            SortStatus.None => seed,
            SortStatus.Ascending => seed.ThenBy(lambda),
            SortStatus.Descending => seed.ThenByDescending(lambda),
            var sortStatus => throw new NotSupportedException($"未能识别排序模式{sortStatus}")
        };
    }
    #endregion
    #endregion
    #endregion
}