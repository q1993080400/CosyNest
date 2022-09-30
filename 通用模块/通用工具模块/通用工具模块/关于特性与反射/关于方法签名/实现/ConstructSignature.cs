namespace System.Reflection;

/// <summary>
/// 这个类型表示构造函数的签名
/// </summary>
sealed class ConstructSignature : Signature, IConstructSignature
{
    #region 重写的方法
    #region 重写GetHashCode
    public override int GetHashCode()
        => ToolEqual.CreateHash(Parameters.Cast<object>().ToArray());
    #endregion
    #region 重写Equals
    public override bool Equals(object? obj)
        => obj is IConstructSignature c &&
        Parameters.SequenceEqual(c.Parameters);
    #endregion
    #region 重写ToString
    public override string ToString()
        => base.ToString() + "   这个方法是构造函数，无返回值";
    #endregion
    #endregion
    #region 判断方法签名是否兼容
    public override bool IsSame(ISignature other)
        => other is IConstructSignature c &&
        ComparisonParameters(c);
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数列表初始化对象
    /// </summary>
    /// <param name="parameters">构造函数的参数列表，如果它的元素不是<see cref="Type"/>，
    /// 则调用<see cref="object.GetType"/>将其转换为<see cref="Type"/></param>
    public ConstructSignature(params object[] parameters)
        : base(parameters)
    {

    }
    #endregion
}
