using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace System.TreeObject.Json;

/// <summary>
/// 这个类型是实现<see cref="ISerializationText"/>的可选基类，
/// 它还可以同时兼容于<see cref="JsonConverter{T}"/>
/// </summary>
/// <typeparam name="Output">反序列化的合法输出类型</typeparam>
public abstract class SerializationBase<Output> : JsonConverter<Output>, ISerializationText
{
    #region 说明文档
    /*实现本抽象类请遵循以下规范：
      #将序列化和反序列化的逻辑放在JsonConverter的抽象方法中，
      不要重写Serialization和Deserialize方法，
      这是因为通过JsonConverter来适配ISerializationText很容易，但是反过来很难

      #在执行序列化和反序列化时，
      应该尽可能地使用options参数传递进来的，任何可用的序列化器，
      举例说明：假设某一个JsonConverter可以序列化数组，
      那么它在碰到不能直接序列化的数组类型时，应该根据数组元素的类型，
      向options参数请求可用的序列化器，如果能够请求到，
      则使用它进行序列化，而不是直接抛出异常

      这样的好处是使序列化器在尽可能多的情况下能够正常工作，
      同时能够避免为具有不同泛型参数的类型单独编写序列化器，这个操作非常麻烦和枯燥

      #为规范反序列化的行为，提出以下约定：
      每次调用Read方法转换对象时，函数默认：

      在进入方法时，Utf8JsonReader的位置在需要转换的部分开头，举例说明：
      开始转换复杂对象时，Utf8JsonReader.TokenType停留在JsonTokenType.StartObject，
      开始转换集合时，停留在JsonTokenType.StartArray，
      开始转换基本类型时，停留在True，False，Null，Number，String

      在离开方法时，Utf8JsonReader的位置在完成转换的部分末尾，举例说明：
      转换复杂对象后，Utf8JsonReader.TokenType停留在JsonTokenType.EndObject，
      转换集合后，停留在JsonTokenType.EndArray，
      转换基本类型后，停留在True，False，Null，Number，String

      需要注意的是：按照这个规范，反序列化完成后，读取器的位置停留在对象的末尾，
      它不会自动前进到下一个对象，因此上级方法必须自行调用Utf8JsonReader.Read()

      这个行为和JsonSerializer所提供的默认行为是一致的
      请务必严格遵守本规范，否则这两个API将无法互相配合*/
    #endregion
    #region 判断是否可转换
    #region 指定是否由本转换器序列化null
    public override bool HandleNull => true;
    #endregion
    #region 确定是否可转换类型
    public override bool CanConvert(Type typeToConvert)
        => typeof(Output).IsAssignableFrom(typeToConvert);

    /*问：在ISerializationText接口中，
      可序列化和可反序列化的类型是分开的，
      这意味着这个API无法准确描述可以被ISerializationText转换的类型，
      那么，实现和重写这个API应该遵循什么原则？
      答：遵循的原则是：如果某一类型既可以被序列化，
      也可以被反序列化，即它为这两者的交集，则返回true，否则返回flase*/
    #endregion
    #endregion
    #region 返回协议名称
    string ISerialization.Agreement
        => SerializationAgreement.Json;
    #endregion
    #region 检查并转换IOptionsBase
    /// <summary>
    /// 如果<paramref name="options"/>不是用于Json的选项，则引发异常，
    /// 如果是，则将其转换为一个等价的<see cref="JsonSerializerOptions"/>
    /// </summary>
    /// <param name="options">待检查和转换的选项</param>
    private JsonSerializerOptions CheckOptions(IOptionsBase? options)
    {
        if (options is { } and not IOptionsJson)
            throw new JsonException($"{options}不是用于序列化Json的选项");
        var op = options.FitJson(true) ?? new();
        op.Converters.Add(this);
        return op;
    }
    #endregion
    #region 关于序列化
    #region 检查是否可序列化
    #region 可序列化的额外类型
    /// <summary>
    /// 如果该方法和<see cref="CanConvert(Type)"/>任意一个返回<see langword="true"/>，
    /// 则代表该类型可以序列化
    /// </summary>
    /// <param name="type">检查是否可序列化的类型</param>
    /// <returns></returns>
    protected virtual bool CanSerializationExtra(Type type)
        => true;
    #endregion
    #region 正式方法
    bool ISerialization.CanSerialization(Type type)
       => CanSerializationExtra(type) || CanConvert(type);
    #endregion
    #endregion
    #region 序列化对象
    #region 专门用于序列化null的方法
    /// <summary>
    /// 这个方法专门用来序列化<see langword="null"/>值
    /// </summary>
    /// <param name="writer">写入Json的序列化器</param>
    protected virtual void WriteNull(Utf8JsonWriter writer)
        => writer.WriteNullValue();
    #endregion
    #region ISerializationText版本
    ReadOnlySpan<byte> ISerialization.Serialization(object? obj, Type? inputType, IOptionsBase? options, Encoding? encoding)
    {
        var text = JsonSerializer.SerializeToUtf8Bytes(obj, inputType ?? obj?.GetType() ?? typeof(Output), CheckOptions(options));
        return encoding is null or UTF8Encoding ?
            text :
            Encoding.Convert(Encoding.UTF8, encoding, text);
    }
    #endregion
    #region JsonConverter版本
    public sealed override void Write(Utf8JsonWriter writer, Output? value, JsonSerializerOptions options)
    {
        if (value is { })
        {
            ISerialization.CheckSerialization(this, value, value.GetType());
            WriteTemplate(writer, value, options);
        }
        else WriteNull(writer);
    }
    #endregion
    #region 模板方法
    /// <inheritdoc cref="JsonConverter{T}.Write(Utf8JsonWriter, T, JsonSerializerOptions)"/>
    protected abstract void WriteTemplate(Utf8JsonWriter writer, Output value, JsonSerializerOptions options);
    #endregion
    #endregion
    #endregion
    #region 关于反序列化
    #region 检查是否可反序列化
    #region 可反序列化的额外类型
    /// <summary>
    /// 如果该方法和<see cref="CanConvert(Type)"/>任意一个返回<see langword="true"/>，
    /// 则代表该类型可以反序列化
    /// </summary>
    /// <param name="type">检查是否可反序列化的类型</param>
    /// <returns></returns>
    protected virtual bool CanDeserializationAssignmentExtra(Type type)
        => type.IsAssignableFrom(typeof(Output));
    #endregion
    #region 正式方法
    bool ISerialization.CanDeserializationAssignment(Type type)
       => CanDeserializationAssignmentExtra(type) || CanConvert(type);
    #endregion
    #endregion
    #region 反序列化对象
    #region 专门用来反序列化null的方法
    /// <summary>
    /// 这个方法专门用来反序列化<see langword="null"/>
    /// </summary>
    /// <returns></returns>
    protected virtual Output? ReadNull()
        => typeof(Output).CanNull() ?
        default : throw new JsonException($"{nameof(Output)}不能被反序列化为null值");
    #endregion
    #region ISerializationText版本
    object? ISerialization.Deserialize(ReadOnlySpan<byte> text, Type? outputType, IOptionsBase? options, Encoding? encoding)
    {
        var op = CheckOptions(options);
        text = encoding is null or UTF8Encoding ?
            text :
            Encoding.Convert(encoding, Encoding.UTF8, text.ToArray());
        return JsonSerializer.Deserialize(text, outputType ?? typeof(Output), op);
    }
    #endregion
    #region JsonConverter版本
    public sealed override Output? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        ISerialization.CheckDeserialize(this, typeToConvert);
        return reader.TokenType is JsonTokenType.Null ?
            ReadNull() : ReadTemplate(ref reader, typeToConvert, options);
    }
    #endregion
    #region 模板方法
    /// <inheritdoc cref="JsonConverter{T}.Read(ref Utf8JsonReader, Type, JsonSerializerOptions)"/>
    protected abstract Output ReadTemplate(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options);
    #endregion
    #endregion
    #endregion
}
