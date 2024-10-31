using System.Design.Direct;

namespace System.Text.Json.Serialization;

/// <summary>
/// 这个类型执行对<see cref="IDirect"/>的Json转换逻辑
/// </summary>
sealed class JsonConverterDirect : JsonConverter<IDirect>
{
    #region 是否可转换
    public override bool CanConvert(Type typeToConvert)
        => typeToConvert == typeof(IDirect);
    #endregion
    #region 反序列化
    public override IDirect? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDocument = JsonSerializer.Deserialize<JsonDocument>(ref reader, options)!;
        return (IDirect?)jsonDocument.RootElement.DeserializeToObject();
    }
    #endregion
    #region 序列化
    public override void Write(Utf8JsonWriter writer, IDirect value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize<IReadOnlyDictionary<string, object?>>(writer, value, options);
    }
    #endregion 
}
