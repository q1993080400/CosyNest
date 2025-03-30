namespace System;

/// <summary>
/// 这个记录封装了所有尝试获取值的方法的返回值
/// </summary>
/// <typeparam name="Obj">值的类型</typeparam>
/// <param name="Success">这个值是否存在</param>
/// <param name="Value">获取到的值</param>
public sealed record TryGetObject<Obj>(bool Success, Obj? Value) : IHasResultValue<Obj>
{
    #region 构造函数
    #region 无参数构造函数，指定操作不成功
    /// <summary>
    /// 无参数构造函数，
    /// 它指定这个操作不成功，值为默认值
    /// </summary>
    public TryGetObject()
        : this(false, default)
    {

    }
    #endregion
    #region 指定操作成功的值
    /// <summary>
    /// 使用一个不为<see langword="null"/>的值初始化对象，
    /// 它默认这个操作已经成功
    /// </summary>
    /// <param name="successValue">操作的值，它不可为<see langword="null"/></param>
    public TryGetObject(Obj successValue)
        : this(true, successValue)
    {
    }
    #endregion 
    #endregion
}