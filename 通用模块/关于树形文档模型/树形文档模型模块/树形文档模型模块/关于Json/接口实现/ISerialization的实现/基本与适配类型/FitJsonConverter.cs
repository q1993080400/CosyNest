using System.Text.Json;
using System.Text.Json.Serialization;

namespace System.TreeObject.Json;

/// <summary>
/// 该类型可以用来适配<see cref="JsonConverter{T}"/>
/// </summary>
/// <typeparam name="Output">反序列化的合法输出类型</typeparam>
sealed class FitJsonConverter<Output> : SerializationBase<Output>
{
    #region 封装的Json转换器
    /// <summary>
    /// 获取封装的Json转换器对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private JsonConverter<Output> Converter { get; }
    #endregion
    #region 是否可转换对象
    public override bool CanConvert(Type typeToConvert)
        => Converter.CanConvert(typeToConvert);
    #endregion
    #region 序列化对象
    protected override void WriteTemplate(Utf8JsonWriter writer, Output value, JsonSerializerOptions options)
        => Converter.Write(writer, value!, options);
    #endregion
    #region 反序列化对象
    protected override Output ReadTemplate(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => Converter.Read(ref reader, typeToConvert, options)!;
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的Json转换器初始化对象
    /// </summary>
    /// <param name="converter">Json转换器对象，
    /// 本对象的功能就是通过它实现的</param>
    public FitJsonConverter(JsonConverter<Output> converter)
    {
        this.Converter = converter;
    }
    #endregion
}
