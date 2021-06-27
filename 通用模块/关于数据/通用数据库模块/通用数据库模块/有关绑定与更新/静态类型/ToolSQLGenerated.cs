using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
using System.Text;

namespace System.DataFrancis.DB
{
    /// <summary>
    /// 这个静态类为生成SQL脚本提供帮助
    /// </summary>
    public static class ToolSQLGenerated
    {
        #region 说明文档
        /*问：为什么要将本类型公开？
          答：因为ISQLGenerated.SpecialExplain属性允许自定义某些方法的解析方式，
          公开本类型可以使这些自定义作者能够复用解释表达式的API*/
        #endregion
        #region 将对象转换为文本
        /// <summary>
        /// 将对象转换为符合SQL文本规范的形式
        /// </summary>
        /// <param name="Obj">待转换的对象</param>
        /// <returns>转换后的文本</returns>
        public static string ToSQLText(object? Obj)
            => Obj switch
            {
                null => "null",
                string t => "'" + t + "'",
                var o => o.ToString()
            };
        #endregion
        #region 解析任何表达式
        /// <summary>
        /// 将表达式转换为对数据表的查询条件
        /// </summary>
        /// <param name="Expression">用来描述查询条件的表达式</param>
        /// <returns>和表达式等价的，描述查询条件的SQL脚本</returns>
        public static string ToWhere(Expression Expression)
            => Expression switch
            {
                LambdaExpression e => ToWhere(e.Body),
                MethodCallExpression e => ISQLGenerated.SpecialMethod.TryGetValue(e.Method, out var value) ?
                value(e) : ToSQLText(e.CalValue()),
                MemberExpression e => ISQLGenerated.SpecialMethod.TryGetValue(e.Member, out var value) ?
                value(e) : ToWhere(e),
                BinaryExpression e => ToWhere(e),
                UnaryExpression e => ToWhere(e),
                var e => ToSQLText(e.CalValue()),
            };
        #endregion
        #region 解析访问属性字段表达式
        /// <summary>
        /// 将访问属性或字段的表达式解析为查询条件
        /// </summary>
        /// <param name="e">待解析的访问属性字段表达式</param>
        /// <returns>和表达式等价的，描述查询条件的SQL脚本</returns>
        public static string ToWhere(MemberExpression e)
            => e.Member is PropertyInfo p &&
            ISQLGenerated.SpecialMethod.TryGetValue(p.GetMethod, out var value) ?
            value(Expression.Call(e.Expression, p.GetMethod)) :
                ToSQLText(e.CalValue());
        #endregion
        #region 解析二元运算符表达式
        #region 辅助方法：判断表达式是否要加上括号
        #region 枚举高优先级运算符
        /// <summary>
        /// 枚举具有高优先级的运算符，
        /// 当它们和低优先级运算符一起出现时，会被优先计算
        /// </summary>
        private static ISet<ExpressionType> PriorityHigh { get; }
        = new HashSet<ExpressionType>()
        {
            ExpressionType.Multiply,ExpressionType.Divide,
            ExpressionType.Modulo,ExpressionType.AndAlso
        };
        #endregion
        #region 枚举优先级不变的运算符
        /// <summary>
        /// 枚举优先级不变的运算符，
        /// 无论和高优先或低优先级运算符出现在一起，
        /// 它的执行顺序不受影响
        /// </summary>
        private static ISet<ExpressionType> PriorityUnchanged { get; }
        = new HashSet<ExpressionType>()
        {
            ExpressionType.Equal,ExpressionType.NotEqual,
            ExpressionType.GreaterThan,ExpressionType.GreaterThanOrEqual,
            ExpressionType.LessThan,ExpressionType.LessThanOrEqual
        };
        #endregion
        #region 方法本体
        /// <summary>
        /// 辅助方法，判断二元运算符表达式生成的SQL脚本是否要加上括号
        /// </summary>
        /// <param name="e">待判断的二元运算符表达式</param>
        /// <returns>一个元组，分别指示左操作数和右操作数是否需要加上括号</returns>
        private static (bool LeftNeed, bool NeedRight) NeedBrackets(BinaryExpression e)
        {
            var fatherNode = e.NodeType;
            #region 本地函数
            bool Get(Expression e)
            {
                return e switch
                {
                    BinaryExpression b => Judge(b),
                    UnaryExpression { NodeType: ExpressionType.Convert, Operand: BinaryExpression b } => Judge(b),
                    _ => false
                };
                #region 已知节点为二元运算符表达式，判断是否需要加上括号
                bool Judge(BinaryExpression nesting)
                {
                    var otherNode = nesting.NodeType;
                    if (PriorityUnchanged.Contains(fatherNode) || PriorityUnchanged.Contains(otherNode))
                        return false;
                    return PriorityHigh.Contains(fatherNode) && !PriorityHigh.Contains(otherNode);
                }
                #endregion
            }
            #endregion
            return (Get(e.Left), Get(e.Right));
        }

        /*说明文档：
          判断是否加上括号的标准为：
          1.如果二元运算符表达式的左右操作数没有一个是二元运算符，返回假
          2.如果父表达式是高优先，子表达式是低优先，返回真
          3.其他情况返回假
          
          问：为什么提取子表达式的时候，如果子表达式是Convert，也将它视为二元运算符表达式？
          答：因为C#支持隐式转换，因此在声明二元运算符表达式的时候，它的左右操作数经常发生不易察觉的转换，
          例如PlaceholderValue的+运算符签名是(PlaceholderValue, object)，
          如果没有这个优化，在右操作数传入任何需要装箱的值类型时都会报错*/
        #endregion 
        #endregion
        #region 正式方法
        /// <summary>
        /// 将二元运算符表达式解析为查询条件
        /// </summary>
        /// <param name="Expression">待解析的二元运算符表达式</param>
        /// <returns>和表达式等价的，描述查询条件的SQL脚本</returns>
        public static string ToWhere(BinaryExpression Expression)
        {
            var (CanPerformLeft, ValueLeft, CanPerformRight, ValueRight, CanPerform, Value) = Expression.CalValueSafety();
            if (CanPerform)
                return ToSQLText(Value);
            #region 用于生成文本的本地函数
            static string Get(Expression e, bool CanPerform, object? Value, bool NeedBrackets)
            {
                if (CanPerform)
                    return ToSQLText(Value);
                var t = ToWhere(e);
                return NeedBrackets ? $"({t})" : t;
            }
            #endregion
            var (LeftNeed, RightNeed) = NeedBrackets(Expression);
            var l = Get(Expression.Left, CanPerformLeft, ValueLeft, LeftNeed);
            var r = Get(Expression.Right, CanPerformRight, ValueRight, RightNeed);
            return l + (Expression.NodeType switch
            {
                ExpressionType.Equal => "=",
                ExpressionType.NotEqual => "<>",
                ExpressionType.GreaterThan => ">",
                ExpressionType.GreaterThanOrEqual => ">=",
                ExpressionType.LessThan => "<",
                ExpressionType.LessThanOrEqual => "<=",
                ExpressionType.AndAlso => " AND ",
                ExpressionType.OrElse => " OR ",
                ExpressionType.Add => "+",
                ExpressionType.Subtract => "-",
                ExpressionType.Multiply => "*",
                ExpressionType.Divide => "/",
                ExpressionType.Modulo => "%",
                var t => throw new NotSupportedException($"不支持解析{t}类型的表达式")
            }) + r;
        }
        #endregion 
        #endregion
        #region 解析一元运算符表达式
        /// <summary>
        /// 将一元运算符表达式解析为查询条件
        /// </summary>
        /// <param name="Expression">待解析的一元运算符表达式</param>
        /// <returns>和表达式等价的，描述查询条件的SQL脚本</returns>
        public static string ToWhere(UnaryExpression Expression)
        {
            var (CanPerform, Results) = Expression.CalValueSafety();
            if (CanPerform)
                return ToSQLText(Results);
            var Operation = ToWhere(Expression.Operand);
            return Expression.NodeType switch
            {
                ExpressionType.Convert => Operation,
                ExpressionType.Negate => "-" + Operation,
                var t => throw new NotSupportedException($"不支持解析{t}类型的表达式")
            };
        }
        #endregion
    }
}
