using System.Collections;
using System.Design.Direct;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace System.DataFrancis;

/// <summary>
/// 该类型是实现<see cref="IData"/>的可选基类
/// </summary>
public abstract record DataBase : IData
{
    #region 关于储存数据的字典
    #region 创建字典
    /// <summary>
    /// 调用这个方法以创建一个字典，
    /// 它用来储存本对象的数据
    /// </summary>
    /// <returns></returns>
    protected virtual IRestrictedDictionary<string, object?> CreateData()
        => new DictionaryFit<string, object?>();
    #endregion
    #region 字典属性
    /// <summary>
    /// 这个字典被用来储存数据
    /// </summary>
    protected IRestrictedDictionary<string, object?> Data { get; }
    #endregion
    #endregion
    #region 字典的实现
    #region 键值对的元素数量
    public int Count => Data.Count;
    #endregion
    #region 键集合
    public IEnumerable<string> Keys => Data.Keys;
    #endregion
    #region 值集合
    public IEnumerable<object?> Values => Data.Values;
    #endregion
    #region 是否存在元素
    public bool ContainsKey(string key) => Data.ContainsKey(key);
    #endregion
    #region 尝试获取元素
    public bool TryGetValue(string key, [MaybeNullWhen(false)] out object? value)
        => Data.TryGetValue(key, out value);
    #endregion
    #region 枚举键值对集合
    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
        => Data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    #endregion
    #endregion
    #region IData的实现
    #region 架构约束
    public abstract ISchema? Schema { get; set; }
    #endregion
    #region 数据的元数据
    private object? MetadataField;

    public object? Metadata
    {
        get => MetadataField;
        set => MetadataField = (MetadataField, value) switch
        {
            ({ }, { }) => throw new NotSupportedException($"在元数据不为null的情况下，" +
                $"只能向本属性写入null以取消元数据，而不能写入新的元数据"),
            (_, var v) => v
        };
    }
    #endregion
    #region 读写数据
    public object? this[string columnName]
    {
        get => Data[columnName];
        set => Data[columnName] = Schema is { } s ? value.To(s.Schema[columnName]) : value;      //检查架构约束
    }
    #endregion
    #region 复制数据
    #region 正式方法
    public IDirect Copy(bool copyValue = true, Type? type = null)
    {
        var copy = type switch
        {
            var t when t is null || t == GetType() => CreateSelf(),
            var t when typeof(IDirect).IsAssignableFrom(t) => t switch
            {
                var dt when dt == typeof(IDirect) || dt == typeof(IData) => CreateSelf(),
                var dt when t.CanNew() => t.GetTypeData().ConstructorCreate<IData>(),
                var dt => throw new ArgumentException($"{dt}不具备公开无参数构造函数，无法复制")
            },
            var t => throw new ArgumentException($"{t}不实现{nameof(IDirect)}")
        };
        if (copyValue)
            foreach (var (name, value) in this)
            {
                copy[name] = value;
            }
        return copy;
    }
    #endregion
    #region 模板方法
    /// <summary>
    /// 创建一个与本对象相同类型的<see cref="IData"/>，
    /// 它的属性字典全部为空
    /// </summary>
    /// <returns></returns>
    protected abstract IData CreateSelf();
    #endregion
    #endregion
    #region 数据的ID
    public abstract Guid ID { get; set; }
    #endregion
    #region 数据ID的列名
    public abstract string? IDColumnName { get; }
    #endregion
    #region 本对象的Json形式
    public string Json
          => JsonSerializer.Serialize(this, CreateDataObj.JsonDirect.ToOptions());
    #endregion
    #endregion
    #region 重写的方法
    #region 重写ToString
    public sealed override string? ToString()
        => this.Join(x => $"{x.Key}：{x.Value}", Environment.NewLine);
    #endregion
    #region 重写GetHashCode
    public override int GetHashCode()
        => ToolEqual.CreateHash(this.ToArray());
    #endregion
    #endregion
    #region 关于构造函数
    #region 无参数构造函数
    public DataBase()
    {
        Data = CreateData();
    }
    #endregion
    #region 复制构造函数
#pragma warning disable IDE0060
    public DataBase(DataBase d)
    {
        Data = CreateData();
    }
#pragma warning restore
    #endregion
    #endregion
}
