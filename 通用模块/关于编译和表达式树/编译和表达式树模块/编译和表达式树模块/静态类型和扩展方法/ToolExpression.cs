namespace System.Linq.Expressions
{
    /// <summary>
    /// 有关表达式的工具类
    /// </summary>
    public static class ToolExpression
    {
        #region 根据ExpressionType计算二元运算
        /// <summary>
        /// 根据<see cref="ExpressionType"/>计算二元运算
        /// </summary>
        /// <param name="l">运算的左操作数，它应该是实际传入运算符的对象，不是表达式</param>
        /// <param name="r">运算的右操作数，它应该是实际传入运算符的对象，不是表达式</param>
        /// <param name="type">描述所执行的二元运算的类型的枚举</param>
        /// <returns>二元运算的结果</returns>
        public static object? CalBinaryOperators(object l, object? r, ExpressionType type)
        {
            dynamic left = l;
            dynamic? right = r;
            return type switch
            {
                ExpressionType.Add => left + right,
                ExpressionType.Subtract => left - right,
                ExpressionType.Multiply => left * right,
                ExpressionType.Divide => left / right,
                ExpressionType.Modulo => left % right,
                ExpressionType.GreaterThan => left > right,
                ExpressionType.GreaterThanOrEqual => left >= right,
                ExpressionType.LessThan => left < right,
                ExpressionType.LessThanOrEqual => left <= right,
                ExpressionType.Equal => left == right,
                ExpressionType.NotEqual => left != right,
                ExpressionType.And => left & right,
                ExpressionType.AndAlso => left && right,
                ExpressionType.OrElse => left || right,
                ExpressionType.Or => left | right,
                var t => throw new NotSupportedException($"不支持计算{t}类型的二元运算符表达式")
            };
        }
        #endregion
        #region 获取不是表达式的表达式
        /// <summary>
        /// 获取不是表达式的表达式，
        /// 它一般被用于表达式重构
        /// </summary>
        public static Expression NotExpression { get; }
        = new NotExpression();
        #endregion
    }
}
