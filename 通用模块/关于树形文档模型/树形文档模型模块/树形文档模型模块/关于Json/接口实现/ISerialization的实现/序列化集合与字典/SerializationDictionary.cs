using System.Collections;
using System.Text.Json;

namespace System.TreeObject.Json;

/// <summary>
/// 该类型是<see cref="ISerializationText"/>的实现，
/// 它会使用一切可用的转换器来序列化字典
/// </summary>
sealed class SerializationDictionary : SerializationBase<IEnumerable>
{
    #region 检查是否可转换
    public override bool CanConvert(Type typeToConvert)
    {
        var t = typeToConvert;
        var dt = typeof(IDictionary<,>);
        var rdt = typeof(IReadOnlyDictionary<,>);
        return t.IsGenericRealize(dt) ||
            t.IsGenericRealize(rdt) ||
            (t.IsRealizeGeneric(dt) && t.CanNew());
    }
    #endregion
    #region 序列化对象
    protected override void WriteTemplate(Utf8JsonWriter writer, IEnumerable value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        foreach (dynamic item in value)
        {
            object? k = item.Key;
            object v = item.Value;
            writer.WritePropertyName(k.ToString()!);
            JsonSerializer.Serialize(writer, v, v?.GetType() ?? typeof(object), options);
        }
        writer.WriteEndObject();
    }
    #endregion
    #region 反序列化对象
    protected override IEnumerable ReadTemplate(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        #region 本地函数
        static LinkedList<(object Key, object? Value)> Fun(ref Utf8JsonReader reader, Type keyType, Type valueType, JsonSerializerOptions options)
        {
            var list = new LinkedList<(object, object?)>();
            do
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.PropertyName:
                        var key = reader.GetString()!;
                        reader.Read();
                        var value = JsonSerializer.Deserialize(ref reader, valueType, options);
                        list.AddLast((key.To(keyType), value));
                        break;
                    case JsonTokenType.EndObject:
                        return list;
                    case var t:
                        throw new JsonException($"未能识别{t}类型的令牌");
                }
            } while (reader.Read());
            return list;
        }
        #endregion
        var arguments = typeToConvert.GetGenericArguments();
        var keyType = arguments[0];
        var valueType = arguments[1];
        var dictionary = (typeToConvert.IsGenericRealize(typeof(IDictionary<,>)) ||
            typeToConvert.IsGenericRealize(typeof(IReadOnlyDictionary<,>)) ?
            typeof(Dictionary<,>).MakeGenericType(arguments) : typeToConvert).
            GetTypeData().ConstructorCreate<object>();
        var pro = dictionary.GetTypeData().Indexings.First();
        reader.Read();      //跳过StartObject令牌
        foreach (var (key, value) in Fun(ref reader, keyType, valueType, options))
        {
            pro.SetValue(dictionary, value, new[] { key });
        }
        return (IEnumerable)dictionary;
    }
    #endregion
}
