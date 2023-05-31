using System.Design.Direct;

namespace System.DataFrancis;

/// <summary>
/// 这个类型是<see cref="IData"/>的实现，
/// 可以被当作一个数据进行传递
/// </summary>
sealed record DataRealize : DataBase, IData
{
    #region 关于字典和架构
    #region 架构约束
    private ISchema? SchemaField;

    public override ISchema? Schema
    {
        get => SchemaField;
        set => SchemaField = IDirect.CheckSchemaSet(this, value);
    }
    #endregion
    #region 复制数据
    protected override IData CreateSelf()
        => new DataRealize(Array.Empty<KeyValuePair<string, object?>>(), false);
    #endregion
    #region 数据的ID
    public override Guid ID
    {
        get => default;
        set => throw new NotSupportedException("不允许写入这个属性");
    }
    #endregion
    #region 数据ID
    public override string? IDColumnName => null;
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 用一个键是列名的键值对集合（通常是字典）初始化数据
    /// </summary>
    /// <param name="dictionary">一个键值对集合，它的元素的键</param>
    /// <param name="copyValue">如果这个值为<see langword="true"/>，则会复制键值对的值，否则不复制</param>
    public DataRealize(IEnumerable<KeyValuePair<string, object?>> dictionary, bool copyValue = false)
    {
        dictionary.ForEach(x => this[x.Key] = copyValue ? x.Value : null);
    }
    #endregion
}
