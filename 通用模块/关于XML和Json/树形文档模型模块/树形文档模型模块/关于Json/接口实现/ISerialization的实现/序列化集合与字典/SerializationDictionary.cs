using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace System.TreeObject.Json
{
    /// <summary>
    /// 该类型是<see cref="ISerialization"/>的实现，
    /// 它会使用一切可用的转换器来序列化字典
    /// </summary>
    class SerializationDictionary : SerializationBase<IEnumerable>
    {
        #region 所必需的对象

        #endregion
        #region 关于检查是否可转换
        #region 是否优先使用默认转换器
        protected override bool PriorityDefault => true;
        #endregion 
        #region 是否可转换
        public override bool CanConvert(Type typeToConvert)
        {
            var t = typeToConvert;
            var dt = typeof(IDictionary<,>);
            var rdt = typeof(IReadOnlyDictionary<,>);
            return t.IsGenericRealize(dt) ||
                t.IsGenericRealize(rdt) ||
                (t.IsRealizeGeneric(dt) && t.CanNew());
        }
        #endregion
        #endregion 
        #region 序列化对象
        protected override void WriteTemplate(Utf8JsonWriter writer, IEnumerable value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            foreach (dynamic item in value)
            {
                object v = item.Value;
                object k = item.Key;
                writer.WriteString(k.ToString()!, JsonSerializer.Serialize(v, v.GetType(), options));
            }
            writer.WriteEndObject();
        }
        #endregion
        #region 反序列化对象
        protected override IEnumerable? ReadTemplate(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            #region 本地函数
            static LinkedList<(string Key, object? Value)> Fun(ref Utf8JsonReader reader, IDictionary<string, Type> propertys, JsonSerializerOptions options)
            {
                var list = new LinkedList<(string, object?)>();
                while (reader.Read())
                {
                    switch (reader.TokenType)
                    {
                        case JsonTokenType.PropertyName:
                            var key = reader.GetString()!;
                            reader.Read();
                            var value = JsonSerializer.Deserialize(ref reader, propertys[key], options);
                            list.AddLast((key, value));
                            break;
                        case JsonTokenType.EndObject:
                            return list;
                        case var t:
                            throw new JsonException($"未能识别{t}类型的令牌");
                    }
                }
                return list;
            }
            #endregion
            var almightyPropertys = typeToConvert.GetProperties().Where(x => x.IsAlmighty()).ToDictionary(x => (x.Name, x.PropertyType), true);
            var propertys = Fun(ref reader, almightyPropertys, options);
            return default;
        }
        #endregion 
    }
}
