namespace ViewDependencies;

/// <summary>
/// 这个实体类表示一个模块
/// </summary>
public sealed record Module
{
    #region 名字
    /// <summary>
    /// 获取模块的名字
    /// </summary>
    public required string Name { get; init; }
    #endregion
    #region 引用深度
    /// <summary>
    /// 获取模块的引用深度，引用深度越小的模块越底层，
    /// 它等于所有子模块中引用深度最大的模块+1，
    /// 如果没有引用任何模块，则为0
    /// </summary>
    public required int ReferenceDepth { get; init; }
    #endregion
    #region 直接引用的模块
    /// <summary>
    /// 获取这个模块所有直接引用的模块
    /// </summary>
    public required IReadOnlySet<Module> Reference { get; init; }
    #endregion
    #region 递归引用模块
    /// <summary>
    /// 递归获取所有引用模块
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Module> ReferenceRecursion()
    {
        if (Reference.Count is 0)
            return Array.Empty<Module>();
        var equalityComparer = FastRealize.EqualityComparer<Module>((x, y) => Equals(x.Name, y.Name), x => x.Name.GetHashCode());
        var rferenceRecursion = Reference.Select(x => x.ReferenceRecursion()).
            SelectMany(x => x).Concat(Reference).Distinct(equalityComparer).ToArray();
        return rferenceRecursion;
    }
    #endregion
    #region 重写ToString
    public override string ToString()
        => Name;
    #endregion
}
