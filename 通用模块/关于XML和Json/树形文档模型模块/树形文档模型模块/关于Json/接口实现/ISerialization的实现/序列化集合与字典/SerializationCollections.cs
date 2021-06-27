using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace System.TreeObject.Json
{
    /// <summary>
    /// 该类型尝试使用一切所支持的<see cref="JsonConverter"/>序列化和反序列化集合
    /// </summary>
    /// <typeparam name="Collections">可序列化和反序列化的集合类型</typeparam>
    class SerializationCollections<Collections> : SerializationBase<Collections>, ISerialization
        where Collections : IEnumerable
    {
        #region 所需要的对象
        #region 获取集合元素的类型的委托
        /// <summary>
        /// 该委托传入集合的类型，
        /// 返回集合元素的类型
        /// </summary>
        private Func<Type, Type> GetElementType { get; }
        #endregion
        #region 创建集合的委托
        /// <summary>
        /// 该委托的第一个参数是集合元素的类型，
        /// 第二个参数是枚举集合元素的枚举器，
        /// 返回值就是新创建的，包含这些元素的集合
        /// </summary>
        private Func<Type, IEnumerable<object?>, Collections> CreateCollections { get; }
        #endregion
        #endregion
        #region 关于序列化和反序列化
        #region 反序列化
        protected override Collections? ReadTemplate(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            reader.Read();
            if (reader.TokenType is JsonTokenType.Null)
                return default;
            var list = new LinkedList<object?>();
            var elementType = GetElementType(typeToConvert);
            while (reader.TokenType is not JsonTokenType.EndArray)
            {
                list.AddLast(JsonSerializer.Deserialize(ref reader, elementType, options));
                reader.Read();
            }
            return CreateCollections(elementType, list);
        }
        #endregion
        #region 序列化
        protected override void WriteTemplate(Utf8JsonWriter writer, Collections value, JsonSerializerOptions options)
        {
            var elementType = GetElementType(value.GetType());
            writer.WriteStartArray();
            foreach (var item in value)
            {
                JsonSerializer.Serialize(writer, item, elementType, options);
            }
            writer.WriteEndArray();
        }
        #endregion
        #endregion 
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="getElementType">该委托传入集合的类型，返回集合元素的类型</param>
        /// <param name="createCollections">该委托的第一个参数是集合元素的类型，
        /// 第二个参数是枚举集合元素的枚举器，返回值就是新创建的，包含这些元素的集合</param>
        public SerializationCollections(Func<Type, Type> getElementType,
            Func<Type, IEnumerable<object?>, Collections> createCollections)
        {
            this.GetElementType = getElementType;
            this.CreateCollections = createCollections;
        }
        #endregion
    }
}
