using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace System.TreeObject.Json
{
    /// <summary>
    /// 这个类型是实现<see cref="ISerialization"/>的可选基类，
    /// 它还可以同时兼容于<see cref="JsonConverter{T}"/>
    /// </summary>
    /// <typeparam name="Output">反序列化的合法输出类型</typeparam>
    public abstract class SerializationBase<Output> : JsonConverter<Output>, ISerialization
    {
        #region 说明文档
        /*实现本抽象类请遵循以下规范：
          #将序列化和反序列化的逻辑放在JsonConverter的抽象方法中，
          不要重写Serialization和Deserialize方法，
          这是因为通过JsonConverter来适配ISerialization很容易，但是反过来很难
          
          #在执行序列化和反序列化时，
          应该尽可能地使用options参数传递进来的，任何可用的序列化器，
          举例说明：假设某一个JsonConverter可以序列化数组，
          那么它在碰到不能直接序列化的数组类型时，应该根据数组元素的类型，
          向options参数请求可用的序列化器，如果能够请求到，
          则使用它进行序列化，而不是直接抛出异常
        
          这样的好处是使序列化器在尽可能多的情况下能够正常工作，
          同时能够避免为具有不同泛型参数的类型单独编写序列化器，这个操作非常麻烦和枯燥
        
          #为规范反序列化的行为，提出以下约定：
          在对对象，属性值，数组元素递归进行反序列化时，
          每次递归调用方法，应该在方法开头调用Utf8JsonReader.Read()，
          然后方法返回时，Utf8JsonReader的位置应该停留在刚刚完成反序列化的部分的末尾，
          举例说明：反序列化复杂对象结束后，Utf8JsonWriter的令牌应该停留在JsonTokenType.EndObject，
          反序列化集合后，令牌应停留在JsonTokenType.EndArray，
          反序列化基本类型后，令牌应停留在True，False，Null，Number，String
        
          请务必严格遵守本规范，否则将造成反序列化的次序混乱，
          下一个递归执行的方法很可能找不到需要的值*/
        #endregion
        #region 对序列化的判断
        #region 指定是否由本转换器序列化null
        public override bool HandleNull => true;
        #endregion
        #region 确定是否可转换类型
        public override bool CanConvert(Type typeToConvert)
            => typeof(Output).IsAssignableFrom(typeToConvert);

        /*问：在ISerialization接口中，
          可序列化和可反序列化的类型是分开的，
          这意味着这个API无法准确描述可以被ISerialization转换的类型，
          那么，实现和重写这个API应该遵循什么原则？
          答：遵循的原则是：如果某一类型既可以被序列化，
          也可以被反序列化，即它为这两者的交集，则返回true，否则返回flase*/
        #endregion
        #endregion 
        #region 返回协议名称
        string ISerialization.Agreement
            => SerializationAgreement.Json;
        #endregion
        #region 返回不包含自身对象的JsonSerializerOptions副本
        /// <summary>
        /// 返回一个<see cref="JsonSerializerOptions"/>对象的近似副本，
        /// 它的自定义转换器列表中不包含和本对象相同类型的转换器，
        /// 通过这个方法可以避免在序列化和反序列化的方法中，
        /// 由于调用自身所引发的无限递归
        /// </summary>
        /// <param name="options">要创建副本的<see cref="JsonSerializerOptions"/></param>
        /// <returns></returns>
        private JsonSerializerOptions GetOptions(JsonSerializerOptions options)
        {
            var type = GetType();
            var newOptions = new JsonSerializerOptions(options);
            newOptions.Converters.RemoveWhere(x => x.GetType() == type);
            return newOptions;
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
        #region 序列化null的方法
        /// <summary>
        /// 该方法专门用于序列化<see langword="null"/>值，
        /// 它可以提供<see langword="null"/>值的自定义序列化，
        /// 同时避免<see cref="WriteTemplate(Utf8JsonWriter, Output, JsonSerializerOptions)"/>方法中重复的判断空指针
        /// </summary>
        /// <param name="writer">要写入到的编写器</param>
        protected virtual void WriteNull(Utf8JsonWriter writer)
            => writer.WriteNullValue();

        /*问：为什么需要一个专门序列化null的方法，
          但是反序列化不需要？
          答：因为该方法的首要目的不是提供null值的自定义序列化，
          这个功能可以在WriteTemplate和ReadTemplate方法中自行实现，
          而是由于Write方法的value参数可能为null，
          所以需要通过这个机制来避免在WriteTemplate方法中反复地判断空指针，
          而Read方法的参数全部不为null，它不需要这个功能*/
        #endregion
        #region ISerialization版本
        ReadOnlySpan<byte> ISerialization.Serialization(object? obj, Type? inputType, IOptionsBase? options, Encoding? encoding)
        {
            if (options is { } o)
                ISerialization.CheckAgreement(SerializationAgreement.Json, o.Agreement);
            var text = JsonSerializer.SerializeToUtf8Bytes(obj, inputType ?? obj?.GetType() ?? typeof(Output), options.Fit());
            return encoding is null or UTF8Encoding ?
                text :
                Encoding.Convert(Encoding.UTF8, encoding, text);
        }
        #endregion
        #region JsonConverter版本
        public sealed override void Write(Utf8JsonWriter writer, Output? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                WriteNull(writer);
                return;
            }
            var type = value.GetType();
            ISerialization.CheckSerialization(this, value, type);
            if (options.CanConverter(type, GetType()))               //尽量使用原生方法
                JsonSerializer.Serialize(writer, value, type, GetOptions(options));
            else WriteTemplate(writer, value, options);
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
        #region ISerialization版本
        object? ISerialization.Deserialize(ReadOnlySpan<byte> text, Type? outputType, IOptionsBase? options, Encoding? encoding)
        {
            if (options is { } o)
                ISerialization.CheckAgreement(SerializationAgreement.Json, o.Agreement);
            text = encoding is null or UTF8Encoding ?
                text :
                Encoding.Convert(encoding, Encoding.UTF8, text.ToArray());
            return JsonSerializer.Deserialize(text, outputType ?? typeof(Output), options.Fit());
        }
        #endregion
        #region JsonConverter版本
        public sealed override Output? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            ISerialization.CheckDeserialize(this, typeToConvert);           //尽量使用原生方法
            return options.CanConverter(typeToConvert, GetType()) ?
                (Output?)JsonSerializer.Deserialize(ref reader, typeToConvert, GetOptions(options)) :
                ReadTemplate(ref reader, typeToConvert, options);
        }
        #endregion
        #region 模板方法
        /// <inheritdoc cref="JsonConverter{T}.Read(ref Utf8JsonReader, Type, JsonSerializerOptions)"/>
        protected abstract Output? ReadTemplate(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options);
        #endregion
        #endregion
        #endregion
    }
}
