namespace System.Reflection;

/// <summary>
/// 这个类型表示方法和构造函数的签名
/// </summary>
abstract class Signature : ISignature
{
    #region 参数列表
    public IReadOnlyList<Type> Parameters { get; }
    #endregion
    #region 重写ToString
    public override string ToString()
        => Parameters.Any() ?
            $"参数:{Parameters.Join(x => x.Name, "，")}" : "无参数";
    #endregion
    #region 判断方法签名是否兼容
    #region 正式方法
    public abstract bool IsSame(ISignature other);
    #endregion
    #region 辅助方法
    /// <summary>
    /// 比较两个签名的参数，支持逆变
    /// </summary>
    /// <param name="other">要比较的另一个方法签名</param>
    /// <returns></returns>
    protected bool ComparisonParameters(ISignature other)
    {
        var op = other.Parameters;
        return op.Count == Parameters.Count &&
            Parameters.Zip(op).All(x =>
              {
                  var (self, other) = x;
                  return other.IsAssignableFrom(self);
              });
    }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 用指定的参数列表初始化对象
    /// </summary>
    /// <param name="parameters">方法的参数列表，如果它的元素不是<see cref="Type"/>，
    /// 则调用<see cref="object.GetType"/>将其转换为<see cref="Type"/></param>
    public Signature(params object[] parameters)
    {
        Parameters = parameters.Select(x => x.GetTypeObj()).ToArray();
    }
    #endregion
}
