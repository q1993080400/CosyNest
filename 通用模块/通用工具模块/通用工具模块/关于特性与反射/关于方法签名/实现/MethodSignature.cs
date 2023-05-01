namespace System.Reflection;

/// <summary>
/// 这个类型表示一个方法的签名
/// </summary>
sealed class MethodSignature : Signature, IMethodSignature
{
    #region 返回值类型
    public Type Return { get; }
    #endregion
    #region 重写的方法
    #region 重写GetHash
    public override int GetHashCode()
        => ToolEqual.CreateHash(Parameters.Prepend(Return).Cast<object>().ToArray());
    #endregion
    #region 重写Equals
    public override bool Equals(object? obj)
        => obj is IMethodSignature m &&
        Parameters.Prepend(Return).
        SequenceEqual(m.Parameters.Prepend(m.Return));
    #endregion
    #region 重写ToString
    public override string ToString()
        => base.ToString() + $"   返回值：{Return.Name}";
    #endregion
    #endregion
    #region 判断方法签名是否兼容
    public override bool IsSame(ISignature other)
        => other is IMethodSignature m &&
        Return.IsAssignableFrom(m.Return) &&
        ComparisonParameters(m);
    #endregion
    #region 构造函数
    /// <summary>
    /// 用指定的返回值类型和参数列表初始化方法签名
    /// </summary>
    /// <param name="return">返回值类型，如果为<see langword="null"/>，则为<see cref="void"/></param>
    /// <param name="parameters">方法的参数列表，如果它的元素不是<see cref="Type"/>，
    /// 则调用<see cref="object.GetType"/>将其转换为<see cref="Type"/></param>
    public MethodSignature(Type? @return, params object[] parameters)
        : base(parameters)
    {
        Return = @return ?? typeof(void);
    }
    #endregion
}
