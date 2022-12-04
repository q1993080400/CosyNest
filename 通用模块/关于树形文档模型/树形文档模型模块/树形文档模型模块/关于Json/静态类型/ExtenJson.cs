using System.Design;
using System.Design.Direct;
using System.Text.Json;
using System.Text.Json.Serialization;

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
