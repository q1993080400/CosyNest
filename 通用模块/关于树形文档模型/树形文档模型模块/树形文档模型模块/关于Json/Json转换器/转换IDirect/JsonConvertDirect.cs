using System.Design;
using System.Design.Direct;

namespace System.Text.Json.Serialization;

/// <summary>
/// 这个类型执行对<see cref="IDirect"/>的Json转换逻辑
/// </summary>
sealed class JsonConvertDirect : JsonConverter<IDirect>
{
    #region 是否可转换
    public override bool CanConvert(Type typeToConvert)
        => typeToConvert == typeof(IDirect);
    #endregion
    #region 反序列化
    #region 适用于数组
    /// <summary>
    /// 将Json字符串的一部分反序列化为数组，
    /// 仅在执行弱类型序列化时，这个方法才会被调用
    /// </summary>
    /// <param name="reader">用来读取Json字符串的反序列化器</param>
    /// <param name="options">用于配置反序列化的选项</param>
    /// <returns></returns>
    private static object?[] ReadArray(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var link = new LinkedList<object?>();
        reader.Read();
        while (true)
        {
            if (reader.TokenType is JsonTokenType.EndArray)
                return link.ToArray();
            link.AddLast(ReadObject(ref reader, options));
            reader.Read();
        }
    }
    #endregion
    #region 适用于任何对象
    /// <summary>
    /// 对Json字符串的一部分执行反序列化，
    /// 它可能返回任何对象
    /// </summary>
    /// <param name="reader">用来读取Json字符串的反序列化器</param>
    /// <param name="options">用于配置反序列化的选项</param>
    /// <returns></returns>
    private static object? ReadObject(ref Utf8JsonReader reader, JsonSerializerOptions options)
        => reader.TokenType switch
        {
            JsonTokenType.StartObject => ReadDirect(ref reader, options),
            JsonTokenType.Comment or JsonTokenType.None => (reader.Read(), ReadObject(ref reader, options)).Item2,
            JsonTokenType.StartArray => ReadArray(ref reader, options),
            JsonTokenType.Null or JsonTokenType.EndArray => null,
            var token => reader.TryGetBasicType() switch
            {
                (true, var v) => v,
                _ => throw new NotSupportedException($"未能识别{token}类型的令牌")
            }
        };
    #endregion
    #region 适用于IDirect
    /// <summary>
    /// 将Json字符串的一部分反序列化为<see cref="IDirect"/>
    /// </summary>
    /// <param name="reader">用来读取Json字符串的反序列化器</param>
    /// <param name="options">用于配置反序列化的选项</param>
    /// <returns></returns>
    private static IDirect? ReadDirect(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.Null)
            return null;
        var list = new LinkedList<(string, object?)>();
        var data = CreateDesign.DirectEmpty();
        #region 本地函数
        void Fun(ref Utf8JsonReader reader)
        {
            do
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.StartObject or JsonTokenType.Comment or JsonTokenType.None:
                        break;
                    case JsonTokenType.PropertyName:
                        var proName = reader.GetString()!;
                        reader.Read();
                        var proValue = ReadObject(ref reader, options);
                        list.AddLast((proName, proValue));
                        break;
                    case JsonTokenType.EndObject:
                        return;
                    case var t:
                        throw new NotSupportedException($"未能识别{t}类型的令牌");
                }
            } while (reader.Read());
        }
        #endregion
        Fun(ref reader);
        foreach (var (k, v) in list)
            data[k] = v;
        return data;
    }
    #endregion
    #region 正式方法
    public override IDirect? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => ReadDirect(ref reader, options);
    #endregion
    #endregion
    #region 序列化
    public override void Write(Utf8JsonWriter writer, IDirect value, JsonSerializerOptions options)
    {
        var dictionary = value.ToDictionary(true);
        JsonSerializer.Serialize(writer, dictionary, options);
    }
    #endregion 
}
