namespace Microsoft.AspNetCore;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以将筛选条件绑定到一个范围上
/// </summary>
/// <typeparam name="RangeValue">筛选范围的类型</typeparam>
public interface IBindRange<RangeValue> : IBind<RangeValue>
{
    #region 筛选范围
    /// <summary>
    /// 获取筛选的范围，
    /// 当它的开始和结束被修改的时候，
    /// 属性也会随之改变
    /// </summary>
    BindRange<RangeValue> Range { get; }
    #endregion
}
