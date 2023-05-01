namespace System.Text.Json.Serialization;

/// <summary>
/// 这个类型会调用一切可用的转换器去转换集合
/// </summary>
sealed class JsonConvertFactoryCollection : JsonConverterFactory
{
    #region 是否可执行转换
    public override bool CanConvert(Type typeToConvert)
        => (typeToConvert.IsGenericRealize(typeof(IEnumerable<>)) ||
        typeToConvert.GetInterfaces().Any(x => x.IsGenericRealize(typeof(ICollection<>)))) &&
        typeToConvert.Namespace is not "System.Collections.Immutable";
    #endregion
    #region 返回转换器
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        if (!CanConvert(typeToConvert))
            return null;
        var elementType = typeToConvert.GetCollectionElementType()!;
        var converterType = typeof(JsonConvertCollection<,>).MakeGenericType(typeToConvert, elementType);
        var converter = converterType.GetTypeData().ConstructorCreate<JsonConverter>();
        return converter;
    }
    #endregion
}
