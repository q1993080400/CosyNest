using System.Runtime.CompilerServices;
using System.Text.Json;

namespace System.TreeObject.Json;

/// <summary>
/// 该类型可用于序列化与反序列化值元组
/// </summary>
sealed class SerializationValueTuple : SerializationBase<ITuple>
{
    #region 是否可序列化
    public override bool CanConvert(Type typeToConvert)
        => typeof(ITuple).IsAssignableFrom(typeToConvert) &&
        typeToConvert.IsValueType &&
        typeToConvert.Name.StartsWith(nameof(ValueTuple));
    #endregion
    #region 序列化对象
    protected override void WriteTemplate(Utf8JsonWriter writer, ITuple value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        for (int i = 0; i < value.Length; i++)
        {
            writer.WritePropertyName($"Item{i + 1}");
            JsonSerializer.Serialize(writer, value[i], options);
        }
        writer.WriteEndObject();
    }
    #endregion
    #region 反序列化对象
    protected override ITuple ReadTemplate(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var list = new LinkedList<object?>();
        var generic = typeToConvert.GenericTypeArguments;
        #region 本地函数
        void Fun(ref Utf8JsonReader reader)
        {
            var pos = 0;
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.EndObject:
                        return;
                    case JsonTokenType.PropertyName:
                        reader.Read();
                        var type = generic[pos];
                        list.AddLast(JsonSerializer.Deserialize(ref reader, type, options));
                        pos++;
                        break;
                    case JsonTokenType.Comment or JsonTokenType.None:
                        break;
                    case var t:
                        throw new NotSupportedException($"未识别{t}类型的令牌");
                }
            }
        }
        #endregion
        Fun(ref reader);
        var method = typeof(ValueTuple).GetTypeData().Methods.
            First(x => x.Name is nameof(ValueTuple.Create) && x.GetParameters().Length == list.Count).
            MakeGenericMethod(generic);
        return method.Invoke<ITuple>(null, list.ToArray())!;
    }
    #endregion 
}
