namespace System.Text.Json.Serialization;

/// <summary>
/// 这个类型执行对集合的Json转换逻辑
/// </summary>
/// <typeparam name="CollectionType">要转换的集合的类型</typeparam>
/// <typeparam name="ElementType">要转换的集合的元素的类型</typeparam>
sealed class JsonConvertCollection<CollectionType, ElementType> : JsonConverter<CollectionType>
    where CollectionType : ICollection<ElementType?>
{
    #region 反序列化
    public override CollectionType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var cache = new LinkedList<ElementType?>();
        var elementType = typeof(ElementType);
        while (true)
        {
            reader.Read();
            if (reader.TokenType is JsonTokenType.EndArray)
                break;
            var element = JsonSerializer.Deserialize<ElementType>(ref reader, options);
            cache.Add(element);
        }
        var collectionType = typeof(CollectionType);
        if (collectionType.IsArray)
        {
            var array = Array.CreateInstance(elementType, cache.Count);
            Array.Copy(cache.ToArray(), array, cache.Count);
            return array.To<CollectionType>();
        }
        var collection = collectionType.GetTypeData().ConstructorCreate<CollectionType>();
        collection.Add(cache);
        return collection;
    }
    #endregion
    #region 序列化
    public override void Write(Utf8JsonWriter writer, CollectionType value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var item in value)
        {
            JsonSerializer.Serialize(writer, item, typeof(ElementType), options);
        }
        writer.WriteEndArray();
    }
    #endregion 
}
