using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace System.TreeObject;

/// <summary>
/// 这个转换器允许进行多态序列化与反序列化
/// </summary>
/// <typeparam name="Obj">要转换的对象类型</typeparam>
/// <remarks>
/// 使用指定的对象初始化类型
/// </remarks>
/// <param name="assemblies">转换器会从这些程序集中搜索多态反序列化的类型</param>
sealed class JsonConverterPolymorphic<Obj>(IEnumerable<Assembly> assemblies) : JsonConverter<Obj>
    where Obj : class
{
    #region 公开成员
    #region 是否可转换
    public override bool CanConvert(Type typeToConvert)
        => typeof(Obj).IsAssignableFrom(typeToConvert);
    #endregion
    #region 反序列化对象
    public override Obj? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        #region 读取对象
        object? ReadObj(ref Utf8JsonReader reader, Type typeToConvert)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    return null;
                case JsonTokenType.StartObject:
                    reader.Read();
                    return ReadObj(ref reader, typeToConvert);
                case JsonTokenType.PropertyName when reader.GetString() is TypeIDKey:
                    reader.Read();
                    var typeID = reader.GetGuid();
                    var type = TypeDictionary.TryGetValue(typeID).Value ??
                        throw new ArgumentException($"在所有枚举的程序集中都未能找到具有以下ID的类型{typeID}");
                    var typeData = type.GetTypeData();
                    var obj = typeData.ConstructorCreate<object>();
                    var properties = typeData.AlmightyPropertys;
                    reader.Read();
                    reader.Read();
                    foreach (var item in properties)
                    {
                        item.SetValue(obj, ReadObj(ref reader, item.PropertyType));
                    }
                    reader.Read();
                    return obj;
                case JsonTokenType.PropertyName:
                    reader.Read();
                    var @return = JsonSerializer.Deserialize(ref reader, typeToConvert, options)!;
                    reader.Read();
                    return @return;
                case var token:
                    throw new NotSupportedException($"没有找到{token}类型的令牌");
            }
        }
        #endregion
        return (Obj?)ReadObj(ref reader, typeToConvert);
    }
    #endregion
    #region 序列化对象
    public override void Write(Utf8JsonWriter writer, Obj value, JsonSerializerOptions options)
    {
        #region 写入对象
        void WriteObj(object? setValue)
        {
            if (setValue is null)
            {
                writer.WriteNullValue();
                return;
            }
            var type = setValue.GetType();
            var trueType = type.Assembly.IsDynamic ? type.BaseType! : type;
            if (!CanConvert(trueType))
            {
                JsonSerializer.Serialize(writer, setValue, trueType, options);
                return;
            }
            writer.WriteStartObject();
            writer.WritePropertyName(TypeIDKey);
            writer.WriteStringValue(trueType.GUID);
            writer.WritePropertyName(ObjectKey);
            writer.WriteStartObject();
            var propertie = trueType.GetTypeData().AlmightyPropertys;
            foreach (var item in propertie)
            {
                writer.WritePropertyName(item.Name);
                WriteObj(item.GetValue(setValue));
            }
            writer.WriteEndObject();
            writer.WriteEndObject();
        }
        #endregion
        WriteObj(value);
    }
    #endregion
    #endregion
    #region 内部成员
    #region 要搜索多态类型的程序集
    /// <summary>
    /// 转换器会从这些程序集中搜索多态反序列化的类型，
    /// 它的键就是程序集的全名
    /// </summary>
    private IReadOnlyDictionary<Guid, Type> TypeDictionary { get; }
        = assemblies.Select(x => x.GetTypes()).SelectMany(x => x).
        Where(x => typeof(Obj).IsAssignableFrom(x)).
        ToDictionary(x => x.GUID, x => x);
    #endregion
    #region 用于标记类型ID的键
    /// <summary>
    /// 这个键被用来作为类型ID的属性名称
    /// </summary>
    private const string TypeIDKey = "25A05450-2150-D6E6-D4BE-28723C1898E3";
    #endregion
    #region 用来标记对象的键
    /// <summary>
    /// 用来标记实际被序列化的对象的键
    /// </summary>
    private const string ObjectKey = "2693BC00-F2ED-4434-B386-ED31330700F3";
    #endregion
    #endregion
}
