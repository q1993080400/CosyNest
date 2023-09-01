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
sealed class JsonConvertPolymorphic<Obj>(IEnumerable<Assembly> assemblies) : JsonConverter<Obj>
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
                case JsonTokenType.PropertyName:
                    if (reader.GetString() is TypeNameKey)
                    {
                        reader.Read();
                        var typeName = reader.GetString() ??
                            throw new NullReferenceException("未能找到描述类型名称的字段，这个Json字符串的格式可能不正确");
                        reader.Read();
                        reader.Read();
                        var assemblyName = reader.GetString() ??
                            throw new NullReferenceException("未能找到描述程序集名称的字段，这个Json字符串的格式可能不正确");
                        reader.Read();
                        reader.Read();
                        var type = Assemblies.TryGetValue(assemblyName).Value?.GetType(typeName) ??
                            throw new ArgumentException($"在所有枚举的程序集中都未能找到类型{typeName}");
                        var obj = type.GetTypeData().ConstructorCreate<object>();
                        var properties = type.GetTypeData().AlmightyPropertys;
                        foreach (var item in properties)
                        {
                            item.SetValue(obj, ReadObj(ref reader, item.PropertyType));
                        }
                        reader.Read();
                        return obj;
                    }
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
            var propertie = trueType.GetTypeData().AlmightyPropertys;
            writer.WriteStartObject();
            writer.WritePropertyName(TypeNameKey);
            writer.WriteStringValue(trueType.FullName);
            writer.WritePropertyName(AssemblyKey);
            var assembly = trueType.Assembly;
            writer.WriteStringValue(assembly.FullName);
            writer.WritePropertyName(ValueKey);
            writer.WriteStartObject();
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
    private Dictionary<string, Assembly> Assemblies { get; } = assemblies.ToDictionary(x => x.FullName ??
        throw new NullReferenceException($"在创建多态Json转换器的时候，某个程序集的全名为null"), x => x);
    #endregion
    #region 用于标记类型全名的键
    /// <summary>
    /// 这个键被用来作为类型名称的属性名字
    /// </summary>
    private const string TypeNameKey = "25A05450-2150-D6E6-D4BE-28723C1898E3";
    #endregion
    #region 用来标记程序集名称的键
    /// <summary>
    /// 这个键被用来作为程序集名称的属性名字
    /// </summary>
    private const string AssemblyKey = "6718FB50-9359-4791-BE1C-3B967041C105";
    #endregion
    #region 用来标记值的键
    /// <summary>
    /// 这个键被用来作为值的属性名字
    /// </summary>
    private const string ValueKey = "FEE34934-59C9-8F1E-2D9D-ECFF4B05EF24";

    #endregion
    #endregion
}
