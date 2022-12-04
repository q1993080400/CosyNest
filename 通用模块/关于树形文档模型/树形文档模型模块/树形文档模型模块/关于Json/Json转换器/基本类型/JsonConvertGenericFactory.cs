namespace System.Text.Json.Serialization;

/// <summary>
/// 这个类型是实现泛型Json转换器的可选基类，
/// 它通过替换泛型类型，创建指定的<see cref="JsonConverter{T}"/>并执行转换
/// </summary>
public abstract class JsonConvertGenericFactory : JsonConverterFactory
{
    #region 泛型转换器类型定义
    /// <summary>
    /// 返回<see cref="JsonConverterFactory.CreateConverter"/>要返回的泛型转换器的类型定义，
    /// 它必须是一个泛型类型，继承自<see cref="JsonConverter{T}"/>，且拥有无参数构造函数
    /// </summary>
    protected abstract Type JsonConvertGenericDefinition();
    #endregion
    #region 返回转换器
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        if (!CanConvert(typeToConvert))
            return null;
        var jsonConvertTypePrototype = JsonConvertGenericDefinition();
        if (!jsonConvertTypePrototype.IsGenericType)
            throw new ArgumentException($"{jsonConvertTypePrototype}不是泛型类型");
        var jsonConvertType = jsonConvertTypePrototype.MakeGenericType(typeToConvert);
        if (!typeof(JsonConverter<>).MakeGenericType(typeToConvert).IsAssignableFrom(jsonConvertType))
            throw new ArgumentException($"{jsonConvertType}不继承自JsonConvert<{typeToConvert}>");
        var jsonConvert = jsonConvertType.GetTypeData().ConstructorCreate<JsonConverter>();
        return jsonConvert;
    }
    #endregion
}
