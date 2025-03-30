namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以对自身进行额外的数据验证，
/// 它适用于比较复杂的，用特性不方便表述的验证方式
/// </summary>
public interface IDataVerify
{
    #region 验证自身
    /// <summary>
    /// 验证自身，并返回验证失败的原因，
    /// 如果验证通过，则返回空集合
    /// </summary>
    /// <returns></returns>
    IEnumerable<string> Verify();
    #endregion
}
