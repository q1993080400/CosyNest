using System.Collections;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace System.TreeObject.Json;

/// <summary>
/// 该类型尝试使用一切所支持的<see cref="JsonConverter"/>序列化和反序列化集合
/// </summary>
sealed class SerializationCollections : SerializationBase<IEnumerable>, ISerializationText
{
    #region 是否可转换
    public override bool CanConvert(Type typeToConvert)
        => typeToConvert.IsArray ||
        typeToConvert.IsGenericRealize(typeof(ICollection<>)) ||
        (typeToConvert.CanNew() && typeToConvert.IsRealizeGeneric(typeof(ICollection<>)));
    #endregion
    #region 关于序列化和反序列化
    #region 返回集合的元素类型
    /// <summary>
    /// 返回集合元素的类型
    /// </summary>
    /// <param name="collectionsType">集合的类型</param>
    /// <returns></returns>
    private static Type GetElementType(Type collectionsType)
        => collectionsType.IsArray ?
        collectionsType.GetElementType()! :
        collectionsType.GetGenericArguments()[0];
    #endregion
    #region 反序列化
    #region 返回实际结果
    /// <summary>
    /// 该方法传入反序列化的临时结果缓存，
    /// 并返回反序列化的真正结果
    /// </summary>
    /// <param name="collections">临时结果缓存</param>
    /// <param name="elementType">集合的元素类型</param>
    /// <param name="typeToConvert">目标结果集合类型</param>
    /// <returns></returns>
    private static IEnumerable GetResult(IEnumerable<object?> collections, Type elementType, Type typeToConvert)
    {
        if (typeToConvert.IsArray)
        {
            var len = collections.Count();
            var array = Array.CreateInstance(elementType, len);
            Array.Copy(collections.ToArray(), array, len);
            return array;
        }
        var newCollectionsType = (typeToConvert.IsInterface ? typeof(List<>).MakeGenericType(elementType) : typeToConvert).GetTypeData();
        var newCollections = newCollectionsType.ConstructorCreate<IEnumerable>();
        var add = newCollectionsType.FindMethod(nameof(ICollection<int>.Add), CreateReflection.MethodSignature(null, elementType));
        foreach (var item in collections)
        {
            add.Invoke(newCollections, new[] { item });
        }
        return newCollections;
    }
    #endregion
    #region 正式方法
    protected override IEnumerable ReadTemplate(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var elementType = GetElementType(typeToConvert);
        var list = new LinkedList<object?>();
        reader.Read();
        while (reader.TokenType is not JsonTokenType.EndArray)
        {
            list.AddLast(JsonSerializer.Deserialize(ref reader, elementType, options));
            reader.Read();
        }
        return GetResult(list, elementType, typeToConvert);
    }
    #endregion 
    #endregion
    #region 序列化
    protected override void WriteTemplate(Utf8JsonWriter writer, IEnumerable value, JsonSerializerOptions options)
    {
        var elementType = GetElementType(value.GetType());
        writer.WriteStartArray();
        foreach (var item in value)
        {
            JsonSerializer.Serialize(writer, item, elementType, options);
        }
        writer.WriteEndArray();
    }
    #endregion
    #endregion
}
