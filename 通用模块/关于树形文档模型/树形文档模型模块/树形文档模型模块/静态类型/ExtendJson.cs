using System.Design;
using System.Design.Direct;
using System.Text.Json;

namespace System;

/// <summary>
/// 有关JSON的扩展方法全部放在这里
/// </summary>
public static class ExtendJson
{
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
            JsonTokenType.Number => (true, reader.GetDecimal()),
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
    #region 正式方法
    /// <summary>
    /// 将<see cref="JsonElement"/>反序列化，并返回结果，
    /// 注意：它将任何数字转换为<see cref="double"/>，
    /// 将对象转换为<see cref="IDirect"/>
    /// </summary>
    /// <param name="json">待解释的<see cref="JsonElement"/></param>
    /// <returns></returns>
    public static object? DeserializeToObject(this JsonElement json)
        => json.ValueKind switch
        {
            JsonValueKind.Object => DeserializeDirect(json),
            JsonValueKind.Array => json.EnumerateArray().Select(static x => x.DeserializeToObject()).ToArray(),
            JsonValueKind.Null => null,
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.String => json.TryGetDateTimeOffset(out var dateTimeOffset) ? dateTimeOffset :
            json.TryGetGuid(out var guid) ? guid : json.GetString(),
            JsonValueKind.Number => json.GetDouble(),
            var kind => throw new NotSupportedException($"不能读取{kind}类型的令牌")
        };
    #endregion
    #region 反序列化为IDirect
    /// <summary>
    /// 将<see cref="JsonElement"/>反序列化为<see cref="IDirect"/>
    /// </summary>
    /// <param name="json">待反序列化的<see cref="JsonElement"/></param>
    /// <returns></returns>
    private static IDirect DeserializeDirect(JsonElement json)
    {
        var obj = CreateDesign.DirectEmpty();
        foreach (var item in json.EnumerateObject())
        {
            obj[item.Name] = item.Value.DeserializeToObject();
        }
        return obj;
    }
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
            case var o when o.GetType().IsNum():
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
