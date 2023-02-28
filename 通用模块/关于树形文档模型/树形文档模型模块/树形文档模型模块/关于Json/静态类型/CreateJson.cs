using System.Text.Json.Serialization;

namespace System.TreeObject.Json;

/// <summary>
/// 这个静态类被用来创建有关Json的对象
/// </summary>
public static class CreateJson
{
    #region 创建集合转换器
    /// <summary>
    /// 创建一个转换器，
    /// 它会使用一切可以使用的转换器去转换集合
    /// </summary>
    public static JsonConverter JsonCollection { get; } = new JsonConvertFactoryCollection();
    #endregion
    #region 创建元组转换器
    /// <summary>
    /// 创建一个转换器，
    /// 它可以用来转换元组
    /// </summary>
    public static JsonConverter JsonTuple { get; } = new JsonConvertFactoryTuple();
    #endregion
    #region 创建投影转换器
    /// <summary>
    /// 创建一个投影转换器，它可以将将复杂的类型转换为一个简单的类型，
    /// 再将其转换为Json，或执行此操作的反向操作
    /// </summary>
    /// <inheritdoc cref="JsonConvertMap{ConvertTo, Map}"/>
    /// <inheritdoc cref="JsonConvertMap{ConvertTo, Map}.JsonConvertMap(Func{ConvertTo, Map}, Func{Map, ConvertTo})"/>
    public static JsonConverter<ConvertTo> JsonMap<ConvertTo, Map>(Func<ConvertTo, Map> toMap, Func<Map, ConvertTo> fromMap)
        => new JsonConvertMap<ConvertTo, Map>(toMap, fromMap);
    #endregion
}
