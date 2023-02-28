using System.Linq.Expressions;

namespace System.DataFrancis;

/// <summary>
/// 这个记录表示查询实体的条件，
/// 它可以被转换为Json进行传递，
/// 然后被解析为表达式树传递到数据库中
/// </summary>
/// <typeparam name="Obj">查询实体的类型</typeparam>
public sealed record QueryCriteria<Obj>
{
    #region 公开成员
    #region 将查询条件转换为表达式树
    /// <summary>
    /// 将查询条件转换为表达式树，然后返回
    /// </summary>
    /// <returns></returns>
    public Expression<Func<Obj, bool>> Convert()
    {
        throw new NotImplementedException();
    }
    #endregion
    #endregion
}
