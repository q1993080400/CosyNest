namespace System.Text.Json.Serialization;

/// <summary>
/// 这个类型是一个投影Json转换器，
/// 它先将复杂的类型转换为一个简单的类型，再将其转换为Json，
/// 或执行此操作的反向操作
/// </summary>
/// <typeparam name="ConvertTo">转换的目标类型</typeparam>
/// <typeparam name="Map">投影类型</typeparam>
/// <remarks>
/// 使用指定的参数初始化对象
/// </remarks>
/// <param name="toMap">本委托将复杂类型转换为投影类型</param>
/// <param name="fromMap">本委托将投影类型转换为复杂类型</param>
sealed class JsonConverterMap<ConvertTo, Map>(Func<ConvertTo, Map> toMap, Func<Map, ConvertTo> fromMap) : JsonConverter<ConvertTo>
{
    #region 公开成员
    #region 反序列化
    public override ConvertTo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var map = JsonSerializer.Deserialize<Map>(ref reader, options);
        return map is null ? default : FromMap(map);
    }
    #endregion
    #region 序列化
    public override void Write(Utf8JsonWriter writer, ConvertTo value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value is null ? default : ToMap(value), options);
    }
    #endregion 
    #endregion
    #region 内部成员
    #region 将复杂类型转换为投影类型
    /// <summary>
    /// 本委托将复杂类型转换为投影类型
    /// </summary>
    private Func<ConvertTo, Map> ToMap { get; } = toMap;
    #endregion
    #region 从投影类型转换为复杂类型
    /// <summary>
    /// 本委托将投影类型转换为复杂类型
    /// </summary>
    private Func<Map, ConvertTo> FromMap { get; } = fromMap;

    #endregion
    #endregion
}
