namespace System.DataFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个携带标题的数据
/// </summary>
public interface ITitleData : IWithName
{
    #region 获取属性的值的类型
    /// <summary>
    /// 获取属性的值的类型
    /// </summary>
    Type ValueType { get; }
    #endregion
    #region 属性的值
    /// <summary>
    /// 获取属性的值
    /// </summary>
    object? Value { get; }
    #endregion
    #region 获取属性的值
    /// <summary>
    /// 将属性转换为指定的值，
    /// 然后返回
    /// </summary>
    /// <typeparam name="Obj">属性的值的类型</typeparam>
    /// <returns></returns>
    Obj? GetValue<Obj>()
        => Value.To<Obj>();
    #endregion
}
