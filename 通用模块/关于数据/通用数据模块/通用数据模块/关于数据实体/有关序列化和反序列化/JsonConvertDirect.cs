using System.DataFrancis;
using System.Design.Direct;

namespace System.Text.Json.Serialization;

/// <summary>
/// 这个类型执行对<see cref="IDirect"/>的Json转换逻辑
/// </summary>
/// <typeparam name="Obj"></typeparam>
sealed class JsonConvertDirect<Obj> : JsonConverter<Obj>
    where Obj : IDirect
{
    #region 反序列化
    #region 说明文档
    /*本类型根据以下原则执行反序列化：
      #如果目标类型是继承自Entity的强类型实体类，
      而且它具有无参数构造函数，则：
      通过反射创建它的实例，并写入每个属性，
      在这种情况下，函数能够知道每个属性应该被反序列化为什么类型，
      它会尽量尝试使用一切可用的反序列化器，这个操作是递归的

      #如果目标类型是IDirect，IData，DataRealize，则：
      通过CreateDataObj.Data方法创建IData，
      但是函数不知道每个属性的具体类型，所以只能执行弱类型反序列化，
      在这种情况下，数字和文本令牌会被反序列化为string，bool，Num，Guid，DateTime和DateTimeOffset，
      数组令牌会被反序列化为object[]，其他任何复杂的对象会被反序列化为IData*/
    #endregion
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
            link.AddLast(ReadObject(ref reader, null, options));
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
    /// <param name="typeToConvert">反序列化的目标类型，
    /// 如果为<see langword="null"/>，表示执行弱类型反序列化</param>
    /// <param name="options">用于配置反序列化的选项</param>
    /// <returns></returns>
    private static object? ReadObject(ref Utf8JsonReader reader, Type? typeToConvert, JsonSerializerOptions options)
        => typeToConvert switch
        {
            { } t when options.CanConverter(t) => JsonSerializer.Deserialize(ref reader, t, options),
            { } t => throw new JsonException($"找不到支持{t}的序列化器"),
            null => reader.TokenType switch
            {
                JsonTokenType.StartObject => ReadDirect(ref reader, typeToConvert, options),
                JsonTokenType.Comment or JsonTokenType.None => (reader.Read(), ReadObject(ref reader, typeToConvert, options)).Item2,
                JsonTokenType.StartArray => ReadArray(ref reader, options),
                JsonTokenType.Null or JsonTokenType.EndArray => null,
                var token => reader.TryGetBasicType() switch
                {
                    (true, var v) => v,
                    _ => throw new NotSupportedException($"未能识别{token}类型的令牌")
                }
            }
        };
    #endregion
    #region 适用于IDirect
    /// <summary>
    /// 将Json字符串的一部分反序列化为<see cref="IDirect"/>
    /// </summary>
    /// <param name="reader">用来读取Json字符串的反序列化器</param>
    /// <param name="typeToConvert">反序列化的目标类型，
    /// 如果为<see langword="null"/>，表示执行弱类型反序列化</param>
    /// <param name="options">用于配置反序列化的选项</param>
    /// <returns></returns>
    private static IDirect? ReadDirect(ref Utf8JsonReader reader, Type? typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.Null)
            return null;
        typeToConvert ??= typeof(IDirect);
        var list = new LinkedList<(string, object?)>();
        var data = typeToConvert == typeof(IDirect) || typeToConvert == typeof(IData) || typeToConvert == typeof(DataRealize) ?
            CreateDataObj.DataEmpty() :
            typeToConvert.GetTypeData().ConstructorCreate<IDirect>();         //允许反序列化具体类型，例如强类型实体类
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
                        var proType = data.Schema?.Schema.TryGetValue(proName).Value;
                        reader.Read();
                        var proValue = ReadObject(ref reader, proType, options);
                        if ((proName, data) is (nameof(IData.Metadata), IData d))       //为元数据做特殊处理，因为它不是数据的一部分
                            d.Metadata = proValue;
                        else list.AddLast((proName, proValue));
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
    public override Obj? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => ReadDirect(ref reader, typeToConvert, options).To<Obj?>();
    #endregion
    #endregion
    #region 序列化
    public override void Write(Utf8JsonWriter writer, Obj value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize<IReadOnlyDictionary<string, object?>>(writer, value, options);
    }
    #endregion 
}
