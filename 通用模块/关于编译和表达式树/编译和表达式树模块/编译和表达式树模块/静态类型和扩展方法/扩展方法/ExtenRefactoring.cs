using System.Linq;
using System.Linq.Expressions;

using static System.Linq.Expressions.Expression;

namespace System
{
    public static partial class ExtenExpressions
    {
        //这个部分类专门储存有关重构表达式的API

        #region 重构表达式
#pragma warning disable CS8604,CS8600,CS8620
        /// <summary>
        /// 将表达式的部分节点替换，并返回重构后的表达式
        /// </summary>
        /// <param name="expression">要重构的表达式</param>
        /// <param name="c">将原表达式节点替换为新节点的委托，
        /// 如果返回<see cref="ToolExpression.NotExpression"/>，则不替换表达式节点，函数会继续遍历下级节点，
        /// 如果返回<see langword="null"/>或其他表达式类型，该节点会被返回值替换，不会继续遍历</param>
        /// <returns>重构后的新表达式，它的部分节点已被替换，如何替换取决于<paramref name="c"/>委托</returns>
        public static Expression? Refactoring(this Expression? expression, Func<Expression?, Expression?> c)
            => c(expression) switch
            {
                NotExpression => expression switch
                {
                    MethodCallExpression e => Call(e.Object.Refactoring(c), e.Method, e.Arguments.Select(x => x.Refactoring(c))),
                    LambdaExpression e => Lambda(e.Type, e.Body.Refactoring(c), e.Parameters.Select(x => (ParameterExpression)x.Refactoring(c)).ToArray()),
                    UnaryExpression e => MakeUnary(e.NodeType, e.Operand.Refactoring(c), e.Type, e.Method),
                    BinaryExpression e => MakeBinary(e.NodeType, e.Left.Refactoring(c), e.Right.Refactoring(c), e.IsLiftedToNull, e.Method),
                    var e => e
                },
                var e => e,
            };
#pragma warning restore

        /*问：重构表达式为什么会输入或返回null值？
          答：这是因为有可能会将实例方法的调用重构为对静态方法的调用（或相反），
          按照表达式树的规范，当调用静态方法时，MethodCallExpression.Object必须为null，
          即便是将其重构为封装null值的ConstantExpression也会引发异常*/
        #endregion
    }
}
