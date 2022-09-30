using System.Linq.Expressions;

namespace System;

/// <summary>
/// 与表达式有关的扩展方法全部放在这里，
/// 通常无需专门调用
/// </summary>
public static partial class ExtenExpressions
{
    //这个部分类专门储存有关计算表达式的API

    #region 说明文档
    /*说明文档：
      事实上可以将Lambda表达式编译成委托执行，
      并直接返回结果，但仍需要这个方法的原因在于：

      本类型的通常用途不是执行表达式，而是简化表达式，
      例如，通过计算表达式，将：

      x=>x+1+1
    
      简化为：
      x=>x+2*/
    #endregion
    #region 针对任何表达式
    #region 会引发异常
    /// <summary>
    /// 执行一个表达式，并返回表达式的返回值，
    /// 如果表达式不可执行，则引发异常
    /// </summary>
    /// <param name="expression">要执行的表达式</param>
    /// <returns>表达式的计算结果</returns>
    public static object? CalValue(this Expression? expression)
        => expression?.CanReduce ?? false ? expression.Reduce() : expression switch
        {
            null => null,
            ConstantExpression e => e.Value,
            MemberExpression e => e.Member.To<dynamic>().GetValue(e.Expression.CalValue()),
            MethodCallExpression e => e.CalValue(),
            NewExpression e => e.Constructor!.Invoke(e.Arguments.Select(x => x.CalValue()).ToArray()),
            LambdaExpression e => e.Body.CalValue(),    //仅对没有参数或不使用参数的表达式有效
            BinaryExpression e => e.CalValue(),
            UnaryExpression e => e.CalValue(),
            var e => throw new NotSupportedException($"不支持解析{e.NodeType}类型的表达式"),
        };
    #endregion
    #region 不会引发异常
    /// <summary>
    /// 执行一个表达式，并返回一个元组，
    /// 它的项分别是表达式是否可以被执行，
    /// 以及如果可以执行，执行后的返回值
    /// </summary>
    /// <param name="expression">要执行的表达式</param>
    /// <returns>一个元组，指示表达式是否计算成功，以及计算结果</returns>
    public static (bool CanPerform, object? Results) CalValueSafety(this Expression expression)
        => ToolException.Ignore<object?, NotSupportedException>(expression.CalValue);
    #endregion
    #endregion
    #region 针对一元运算符表达式
    /// <summary>
    /// 计算一个一元运算符表达式的值
    /// </summary>
    /// <param name="unary">待计算的一元运算符表达式</param>
    /// <returns>表达式的计算结果</returns>
    private static object? CalValue(this UnaryExpression unary)
    {
        dynamic? op = unary.Operand.CalValue();
        if (unary.Method is { })
            return unary.Method.Invoke(null, new[] { op });
        return unary.NodeType switch
        {
            ExpressionType.Convert
            => op is IConvertible ? Convert.ChangeType(op, unary.Type) : op,
            ExpressionType.PreDecrementAssign => --op,
            ExpressionType.PostDecrementAssign => op--,
            ExpressionType.PreIncrementAssign => ++op,
            ExpressionType.PostIncrementAssign => op++,
            ExpressionType.Negate => -op,
            ExpressionType.Not => !op,
            var e => throw new NotSupportedException($"不支持解析{e}类型的表达式")
        };
    }
    #endregion
    #region 针对方法表达式
    /// <summary>
    /// 执行一个<see cref="MethodCallExpression"/>封装的方法，
    /// 并返回方法的值
    /// </summary>
    /// <param name="methodCall">需要求值的方法表达式</param>
    /// <returns>表达式的计算结果</returns>
    private static object? CalValue(this MethodCallExpression methodCall)
        => methodCall.Method.Invoke(
            methodCall.Object.CalValue(),
            methodCall.Arguments.Select(x => x.CalValue()).ToArray());
    #endregion
    #region 针对二元运算符表达式
    #region 会引发异常
    /// <summary>
    /// 计算一个二元运算符表达式的值
    /// </summary>
    /// <param name="binary">待计算的二元运算符表达式</param>
    /// <returns>二元运算符表达式的计算结果</returns>
    private static object? CalValue(this BinaryExpression binary)
    {
        var left = binary.Left.CalValue();
        var right = binary.Right.CalValue();
        return binary.Method == null ?
            ToolExpression.CalBinaryOperators(left!, right, binary.NodeType) :
            binary.Method.Invoke(null, new[] { left, right });
    }
    #endregion
    #region 不会引发异常
    /// <summary>
    /// 计算二元运算符表达式的左操作数，右操作数和整个表达式的结果
    /// </summary>
    /// <param name="binary">待计算的二元运算符表达式</param>
    /// <returns>一个元组，它的项分别是左操作数是否计算成功，左操作数，
    /// 右操作数是否计算成功，右操作数，整体是否计算成功，表达式的结果</returns>
    public static (bool CanPerformLeft, object? ValueLeft, bool CanPerformRight, object? ValueRight, bool CanPerform, object? Value) CalValueSafety(this BinaryExpression binary)
    {
        var (canPerformLeft, valueLeft) = binary.Left.CalValueSafety();
        var (canPerformRight, valueRight) = binary.Right.CalValueSafety();
        if (canPerformLeft && canPerformRight)
        {
            var (canPerform, value) = Expression.MakeBinary
                (binary.NodeType, Expression.Constant(valueLeft), Expression.Constant(valueRight),
                binary.IsLiftedToNull, binary.Method, binary.Conversion).
                To<Expression>().CalValueSafety();
            return (canPerformLeft, valueLeft, canPerformRight, valueRight, canPerform, value);
        }
        return (canPerformLeft, valueLeft, canPerformRight, valueRight, false, null);
    }
    #endregion
    #endregion
    #region 执行Lambda表达式
    /// <summary>
    /// 将一个Lambda表达式编译成委托，
    /// 并执行它，返回它的返回值
    /// </summary>
    /// <typeparam name="Obj">返回值类型</typeparam>
    /// <param name="lambda">要执行的Lambda表达式</param>
    /// <param name="parameters">表达式的参数</param>
    /// <returns>表达式的计算结果</returns>
    public static Obj CompileInvoke<Obj>(this LambdaExpression lambda, params object[] parameters)
        => (Obj)lambda.Compile().DynamicInvoke(parameters)!;
    #endregion
}
