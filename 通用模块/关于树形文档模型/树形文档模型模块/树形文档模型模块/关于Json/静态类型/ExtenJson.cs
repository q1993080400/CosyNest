using System.Design;
using System.Design.Direct;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.TreeObject;
using System.TreeObject.Json;

namespace System;

/// <summary>
/// 有关JSON的扩展方法全部放在这里
/// </summary>
public static class ExtenJson
{
    #region 关于JsonSerializerOptions
    #region 通过JsonConverter创建JsonSerializerOptions
    /// <summary>
    /// 创建一个<see cref="JsonSerializerOptions"/>，
    /// 并将一个<see cref="JsonConverter"/>添加到它的转换器列表中
    /// </summary>
    /// <param name="converter">待添加进转换器列表的<see cref="JsonConverter"/></param>
    /// <returns></returns>
    public static JsonSerializerOptions ToOptions(this JsonConverter converter)
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(converter);
        return options;
    }
    #endregion
    #region 检查类型是否可转换
    /// <summary>
    /// 如果<see cref="JsonSerializerOptions"/>具有指定类型的转换器，
    /// 则返回<see langword="true"/>，否则返回<see langword="false"/>
    /// </summary>
    /// <param name="options">要检查的<see cref="JsonSerializerOptions"/></param>
    /// <param name="converterType">待转换的类型</param>
    /// <returns></returns>
    public static bool CanConverter(this JsonSerializerOptions options, Type converterType)
        => options.GetConverter(converterType) is { };
    #endregion
    #endregion
    #region 关于适配
    #region 适配ISerializationText和JsonConverter
    #region 说明文档
    /*说明文档
      问：ISerializationText和JsonConverter之间的适配存在相当多的限制，它主要体现在哪里？
      答：体现在以下方面：
      #从JsonConverter适配为ISerializationText不存在任何问题

      #实际上无法从ISerializationText适配为JsonConvertern，
      现在的适配实际上是假适配，它和简单类型转换差不了多少，
      这是因为微软对序列化的限制过于严格，Utf8JsonWriter不能直接写入原始字符串，
      这导致无法将ISerializationText序列化后的字符串写入序列化器

      问：既然如此，如何最大程度的保障序列化器的兼容性？
      答：建议在实现ISerializationText时，从可选基类SerializationBase继承，
      它既继承自JsonConverter，又实现了ISerializationText，
      同时，序列化和反序列化的逻辑应该放在JsonConverter的抽象方法中，
      因为这两个抽象方法易于实现，但是难以适配*/
    #endregion
    #region 从JsonConverter<T>适配
    /// <summary>
    /// 将<see cref="JsonConverter{T}"/>适配为<see cref="SerializationBase{Output}"/>
    /// </summary>
    /// <typeparam name="Output">可序列化的目标类型</typeparam>
    /// <param name="converter">待适配的转换器</param>
    /// <returns></returns>
    public static SerializationBase<Output> Fit<Output>(this JsonConverter<Output> converter)
        => converter as SerializationBase<Output> ?? new FitJsonConverter<Output>(converter);
    #endregion
    #region 从JsonConverter适配
    /// <summary>
    /// 将<see cref="JsonConverter"/>适配为<see cref="ISerializationText"/>
    /// </summary>
    /// <param name="converter">待适配的序列化器</param>
    /// <returns></returns>
    public static ISerializationText Fit(this JsonConverter converter)
        => converter as ISerializationText ??
        throw new ArgumentException($"{nameof(converter)}必须是一个{nameof(ISerializationText)}，否则无法适配");
    #endregion
    #region 从ISerializationText适配
    /// <summary>
    /// 将<see cref="ISerializationText"/>适配为<see cref="JsonConverter"/>，
    /// 但除非它本身就是一个<see cref="JsonConverter"/>，否则无法适配
    /// </summary>
    /// <param name="serialization">待适配的序列化器</param>
    /// <returns></returns>
    public static JsonConverter Fit(this ISerializationText serialization)
        => serialization as JsonConverter ??
        throw new ArgumentException($"{nameof(serialization)}必须是一个{nameof(JsonConverter)}，否则无法适配");
    #endregion
    #endregion
    #region 适配IOptionsBase和JsonSerializerOptions
    #region 从JsonSerializerOptions适配
    /// <summary>
    /// 将一个<see cref="JsonSerializerOptions"/>适配为<see cref="IOptionsBase"/>
    /// </summary>
    /// <param name="options">待适配的对象</param>
    /// <param name="copy">如果这个值为<see langword="true"/>，
    /// 则使用<paramref name="options"/>的副本创建选项，否则使用它的原始引用</param>
    /// <returns></returns>
    [return: NotNullIfNotNull("options")]
    public static IOptionsJson? FitJson(this JsonSerializerOptions? options, bool copy = false)
        => options is null ? null : new OptionsJsonFit(copy ? new(options) : options);
    #endregion
    #region 从IOptionsBase适配
    /// <summary>
    /// 将一个<see cref="IOptionsBase"/>适配为<see cref="JsonSerializerOptions"/>
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="FitJson(JsonSerializerOptions?, bool)"/>
    [return: NotNullIfNotNull("options")]
    public static JsonSerializerOptions? FitJson(this IOptionsBase? options, bool copy = false)
        => options switch
        {
            null => null,
            OptionsJsonFit { Options: { } o } => copy ? new(o) : o,
            not IOptionsJson => throw new JsonException($"{options}不是Json序列化选项，无法适配"),
            _ => throw new ArgumentException($"{options}没有封装{nameof(JsonSerializerOptions)}，无法进行适配")
        };
    #endregion
    #endregion
    #endregion
    #region 关于读取Json
    #region 反序列化为基本类型
    #region 传入Utf8JsonReader
    /// <summary>
    /// 读取<see cref="Utf8JsonReader"/>的令牌，并尝试将其解释为基本类型
    /// </summary>
    /// <param name="reader">待读取令牌的<see cref="Utf8JsonReader"/></param>
    /// <returns>一个元组，它的项分别是是否成功解释为基本类型，以及转换后的基本类型的值，
    /// 本函数可以处理的基本类型包括：<see cref="bool"/>，<see cref="Num"/>（以及可转换为它的数字类型），
    /// <see cref="DateTime"/>，<see cref="DateTimeOffset"/>，<see cref="Guid"/>，
    /// <see cref="string"/>和<see langword="null"/>值</returns>
    public static (bool IsSuccess, object? Value) TryGetBasicType(this in Utf8JsonReader reader)
        => reader.TokenType switch
        {
            JsonTokenType.True or JsonTokenType.False => (true, reader.GetBoolean()),
            JsonTokenType.Null => (true, null),
            JsonTokenType.Number => (true, (Num)reader.GetDecimal()),
            JsonTokenType.String => reader switch
            {
                var r when r.TryGetDateTimeOffset(out var value) => (true, value),
                var r when r.TryGetDateTime(out var value) => (true, value),
                var r when r.TryGetGuid(out var value) => (true, value),
                var r => (true, r.GetString())
            },
            _ => (false, null)
        };
    #endregion
    #region 传入JsonElement
    #region 反序列化对象
    /// <summary>
    /// 将<see cref="JsonElement"/>反序列化为对象
    /// </summary>
    /// <param name="json">待反序列化的<see cref="JsonElement"/></param>
    /// <returns></returns>
    private static IDirect DeserializeObject(JsonElement json)
    {
        var obj = CreateDesign.DirectEmpty();
        foreach (var item in json.EnumerateObject())
        {
            obj[item.Name] = item.Value.Deserialize();
        }
        return obj;
    }
    #endregion
    #region 正式方法
    /// <summary>
    /// 将<see cref="JsonElement"/>解释为与它兼容的基本类型，
    /// 如果它不是基本类型，会产生异常
    /// </summary>
    /// <param name="json">待解释的<see cref="JsonElement"/></param>
    /// <returns></returns>
    public static object? Deserialize(this JsonElement json)
        => json.ValueKind switch
        {
            JsonValueKind.Object => DeserializeObject(json),
            JsonValueKind.Array => json.EnumerateArray().Select(x => x.Deserialize()).ToArray(),
            JsonValueKind.Null => null,
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.String => json.TryGetDateTimeOffset(out var datetime) ? datetime :
            json.TryGetDateTimeOffset(out var dateTimeOffset) ? dateTimeOffset :
            json.TryGetGuid(out var guid) ? guid : json.GetString(),
            JsonValueKind.Number => (Num)json.GetDecimal(),
            var kind => throw new NotSupportedException($"不能读取{kind}类型的令牌")
        };
    #endregion
    #endregion 
    #endregion
    #endregion
    #region 关于写入Json
    #region 序列化基本类型
    /// <summary>
    /// 尝试将基本类型写入<see cref="Utf8JsonWriter"/>
    /// </summary>
    /// <param name="writer">待写入的<see cref="Utf8JsonWriter"/>对象</param>
    /// <param name="value">要写入的值</param>
    /// <returns>如果<paramref name="value"/>是基本类型，
    /// 则将其写入，并返回<see langword="true"/>，否则不执行其他操作，并返回<see langword="false"/>，
    /// 基本类型包括：<see cref="bool"/>，<see cref="Num"/>（以及可转换为它的数字类型），
    /// <see cref="DateTime"/>，<see cref="DateTimeOffset"/>，<see cref="Guid"/>，
    /// <see cref="string"/>，<see cref="Enum"/>和<see langword="null"/>值</returns>
    public static bool TryWriteBasicType(this Utf8JsonWriter writer, object? value)
    {
        switch (value)
        {
            case null:
                writer.WriteNullValue();
                return true;
            case string o:
                writer.WriteStringValue(o);
                return true;
            case bool o:
                writer.WriteBooleanValue(o);
                return true;
            case Enum o:
                writer.WriteStringValue(o.ToString());
                return true;
            case DateTime o:
                writer.WriteStringValue(o);
                return true;
            case DateTimeOffset o:
                writer.WriteStringValue(o);
                return true;
            case Guid o:
                writer.WriteStringValue(o);
                return true;
            case var o when o.GetType().IsNum() || o is Num:
                writer.WriteNumberValue(o.To<decimal>());
                return true;
            default:
                return false;
        }
    }
    #endregion
    #region 尝试写入属性的名称
    /// <summary>
    /// 如果<paramref name="propertyName"/>不为<see langword="null"/>，
    /// 则向<paramref name="writer"/>中写入属性的名称，否则不执行任何操作
    /// </summary>
    /// <param name="writer">待写入属性名称的<see cref="Utf8JsonWriter"/></param>
    /// <param name="propertyName">待写入的属性名称，
    /// 如果为<see langword="null"/>，则不执行任何操作</param>
    public static void TryWritePropertyName(this Utf8JsonWriter writer, string? propertyName)
    {
        if (propertyName is { })
            writer.WritePropertyName(propertyName);
    }
    #endregion
    #endregion
}
