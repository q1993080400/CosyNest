using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Design;
using System.Design.Direct;
using System.Diagnostics.CodeAnalysis;
using System.Performance;
using System.Reflection;
using System.Text.Json;

namespace System.DataFrancis;

/// <summary>
/// 这个类型是所有强类型实体类的可选基类
/// </summary>
public abstract class Entity : IData
{
    #region 公开成员
    #region 数据ID
    public Guid ID { get; set; }
    #endregion
    #region 数据的元数据
    [NotMapped]
    object? IData.Metadata { get; set; }
    #endregion
    #region 获取数据列名
    [NotMapped]
    string? IData.IDColumnName
        => nameof(ID);
    #endregion
    #region 数据架构
    #region 缓存属性
    /// <summary>
    /// 根据实体类的类型，索引它的属性，
    /// 仅有可读，可写，公开，非静态，
    /// 且没有<see cref="NotMappedAttribute"/>特性的属性才会被索引
    /// </summary>
    private static ICache<Type, ISchema> SchemaCache { get; }
    = CreatePerformance.CacheThreshold(type =>
    {
        var propertyTypes = type.GetTypeData().AlmightyPropertys.
        Where(x => x.GetCustomAttribute<NotMappedAttribute>() is null).
        Select(x => (x.Name, x.PropertyType)).ToArray();
        return CreateDesign.Schema(propertyTypes);
    }, 100, SchemaCache);
    #endregion
    #region 正式属性
    [NotMapped]
    ISchema? IDirect.Schema
    {
        get => SchemaCache[GetType()];
        set => throw new NotSupportedException("本类型是强类型实体类，不支持显式写入架构约束");
    }
    #endregion
    #endregion
    #region 有关键值对集合
    #region 获取列数
    int IReadOnlyCollection<KeyValuePair<string, object?>>.Count
       => CacheProperty[GetType()].Count;
    #endregion
    #region 枚举数据
    IEnumerator<KeyValuePair<string, object?>> IEnumerable<KeyValuePair<string, object?>>.GetEnumerator()
    {
        var property = CacheProperty[GetType()];
        foreach (var (key, value) in property)
        {
            yield return new(key, value.GetValue(this));
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
        => this.To<IEnumerable<KeyValuePair<string, object?>>>().GetEnumerator();
    #endregion
    #region 检查键是否存在
    bool IReadOnlyDictionary<string, object?>.ContainsKey(string key)
       => CacheProperty[GetType()].ContainsKey(key);
    #endregion
    #region 键集合
    IEnumerable<string> IReadOnlyDictionary<string, object?>.Keys
       => this.Select(x => x.Key);
    #endregion
    #region 值集合
    IEnumerable<object?> IReadOnlyDictionary<string, object?>.Values
       => this.Select(x => x.Value);
    #endregion
    #region 读写数据
    #region 缓存属性
    /// <summary>
    /// 根据实体类的类型，索引实体类的属性，
    /// 仅索引映射到数据库的属性
    /// </summary>
    private static ICache<Type, IReadOnlyDictionary<string, PropertyInfo>> CacheProperty { get; }
    = CreatePerformance.CacheThreshold(type =>
    {
        var propertys = type.GetTypeData().AlmightyPropertys.
        Where(x => x.GetCustomAttribute<NotMappedAttribute>() is null);
        return propertys.ToDictionary(x => (x.Name, x), true);
    }, 100, CacheProperty);
    #endregion
    #region 正式方法
    public object? this[string key]
    {
        get => CacheProperty[GetType()][key].GetValue(this);
        set => CacheProperty[GetType()][key].SetValue(this, value);
    }
    #endregion
    #endregion
    #region 尝试读写属性
    public bool TryGetValue(string key, [MaybeNullWhen(false)] out object? value)
    {
        if (CacheProperty[GetType()].TryGetValue(key, out var pro))
        {
            value = pro.GetValue(this);
            return true;
        }
        value = default;
        return false;
    }
    #endregion
    #endregion
    #region 本对象的Json形式
    string IDirect.Json
         => JsonSerializer.Serialize(this, CreateDataObj.JsonDirect.ToOptions());
    #endregion
    #region 返回数据的副本
    IDirect IDirect.Copy(bool copyValue, Type? type)
    {
        if (type is { } && !typeof(IDirect).IsAssignableFrom(type))
            throw new ArgumentException($"{type}不实现{nameof(IDirect)}，无法复制");
        var copy = (type ?? GetType()).GetTypeData().ConstructorCreate<IDirect>();
        if (copyValue)
        {
            foreach (var (key, _) in this)
            {
                copy[key] = this[key];
            }
        }
        return copy;
    }
    #endregion
    #endregion
}
