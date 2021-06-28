using System.Collections.Generic;
using System.DataFrancis;
using System.Design.Direct;
using System.Linq;
using System.TreeObject.Json;

namespace System.Text.Json.Serialization
{
    /// <summary>
    /// 这个类型可以使用Json序列化和反序列化<see cref="IDirect"/>
    /// </summary>
    sealed class SerializationIDirect : SerializationBase<IDirect>
    {
        #region 重写的CanConvert方法
        public override bool CanConvert(Type typeToConvert)
            => typeToConvert == typeof(IDirect) ||
            typeToConvert == typeof(IData) ||
            typeToConvert == typeof(DataRealize) ||
            (typeof(IDirect).IsAssignableFrom(typeToConvert) && typeToConvert.CanNew());
        #endregion
        #region 关于序列化和反序列化
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
        private object?[] ReadArray(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            var link = new LinkedList<object?>();
            while (true)
            {
                var elements = ReadObject(ref reader, null, options);
                if (reader.TokenType is JsonTokenType.EndArray)
                    return link.ToArray();
                link.AddLast(elements);
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
        private object? ReadObject(ref Utf8JsonReader reader, Type? typeToConvert, JsonSerializerOptions options)
            => reader.Read() switch
            {
                false => throw new JsonException("读取器已到达末尾，但找不到要反序列化的属性值或数组元素，该Json字符串可能不合法"),
                true => typeToConvert switch
                {
                    { } t when options.CanConverter(t) => JsonSerializer.Deserialize(ref reader, t, options),
                    { } t => throw new JsonException($"找不到支持{t}的序列化器"),
                    null => reader.TokenType switch
                    {
                        JsonTokenType.StartObject => ReadDirect(ref reader, typeToConvert, options),
                        JsonTokenType.Comment or JsonTokenType.None => ReadObject(ref reader, typeToConvert, options),
                        JsonTokenType.StartArray => ReadArray(ref reader, options),
                        JsonTokenType.Null or JsonTokenType.EndArray => null,
                        var token => reader.TryGetBasicType() switch
                        {
                            (true, var v) => v,
                            _ => throw new NotSupportedException($"未能识别{token}类型的令牌")
                        }
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
        private IDirect? ReadDirect(ref Utf8JsonReader reader, Type? typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.Null)
                return null;
            typeToConvert ??= typeof(IDirect);
            var list = new LinkedList<(string, object?)>();
            var pro = typeof(Entity).IsAssignableFrom(typeToConvert) ?
                Entity.GetProperty(typeToConvert).ToDictionary(x => (x.Name, x), true) : new();
            #region 本地函数
            void Fun(ref Utf8JsonReader reader)
            {
                while (reader.Read())
                {
                    switch (reader.TokenType)
                    {
                        case JsonTokenType.StartObject or JsonTokenType.Comment or JsonTokenType.None:
                            break;
                        case JsonTokenType.PropertyName:
                            var proName = reader.GetString()!;
                            var proType = pro.TryGetValue(proName).Value?.PropertyType;
                            list.AddLast((proName, ReadObject(ref reader, proType, options)));
                            break;
                        case JsonTokenType.EndObject:
                            return;
                        case var t:
                            throw new NotSupportedException($"未能识别{t}类型的令牌");
                    }
                }
            }
            #endregion
            Fun(ref reader);
            var data = CreateDataObj.Data(list.ToArray());
            if (typeToConvert == typeof(IDirect) || typeToConvert == typeof(IData) || typeToConvert == typeof(DataRealize))
                return data;
            var copy = typeToConvert.GetTypeData().ConstructorCreate<IDirect>();            //以下代码将允许反序列化具体类型，例如强类型实体类
            foreach (var (k, v) in data)
                copy[k] = v;
            return copy;
        }
        #endregion
        #region 正式方法
        protected override IDirect? ReadTemplate(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => ReadDirect(ref reader, typeToConvert, options);
        #endregion
        #endregion
        #region 序列化
        protected override void WriteTemplate(Utf8JsonWriter writer, IDirect? value, JsonSerializerOptions options)
            => JsonSerializer.Serialize<IReadOnlyDictionary<string, object?>>(writer, value!, options);
        #endregion
        #endregion 
    }
}
