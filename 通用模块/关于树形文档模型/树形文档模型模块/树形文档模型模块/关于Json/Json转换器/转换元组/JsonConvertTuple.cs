using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace System.TreeObject.Json;

/// <summary>
/// 这个转换器可以用来转换元组
/// </summary>
/// <typeparam name="Tuple">待转换的元组的类型，它只支持值元组</typeparam>
sealed class JsonConvertTuple<Tuple> : JsonConverter<Tuple>
    where Tuple : struct, ITuple
{
    #region 反序列化
    public override Tuple Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        object tuple = new Tuple();
        while (true)
        {
            reader.Read();
            if (reader.TokenType is JsonTokenType.EndObject)
                break;
            if (reader.TokenType is JsonTokenType.Comment)
                continue;
            var propertyName = reader.GetString()!;
            var property = typeToConvert.GetField(propertyName) ?? throw new KeyNotFoundException($"未找到名为{propertyName}的字段");
            reader.Read();
            var propertyValue = JsonSerializer.Deserialize(ref reader, property.FieldType, options);
            property.SetValue(tuple, propertyValue);
        }
        return (Tuple)tuple;
    }
    #endregion
    #region 序列化
    public override void Write(Utf8JsonWriter writer, Tuple value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        for (int i = 0; i < value.Length; i++)
        {
            var propertyName = $"Item{i + 1}";
            writer.WritePropertyName(propertyName);
            var propertyType = typeof(Tuple).GetField(propertyName)!.FieldType;
            JsonSerializer.Serialize(writer, value[i], propertyType, options);
        }
        writer.WriteEndObject();
    }
    #endregion 
}
