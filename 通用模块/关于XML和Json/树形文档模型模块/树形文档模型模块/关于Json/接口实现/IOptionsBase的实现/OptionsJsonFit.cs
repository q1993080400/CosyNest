using System.Linq;
using System.Text.Json;

namespace System.TreeObject.Json
{
    /// <summary>
    /// 该类型是<see cref="IOptionsBase"/>的实现，
    /// 可以视为一个用来适配<see cref="JsonSerializerOptions"/>的Json选项
    /// </summary>
    class OptionsJsonFit : IOptionsBase
    {
        #region 封装的Json选项
        /// <summary>
        /// 获取封装的Json选项，
        /// 本类型的功能就是通过它实现的
        /// </summary>
        public JsonSerializerOptions Options { get; }
        #endregion
        #region 返回协议类型
        public string Agreement
            => SerializationAgreement.Json;
        #endregion
        #region 返回序列化器
        public ISerialization[] GetSerialization(Type serializationType)
            => Options.Converters.OfType<ISerialization>().
            Where(x => x.CanSerialization(serializationType)).ToArray();
        #endregion
        #region 返回反序列化器
        public ISerialization[] GetDeserialize(Type deserializeType)
            => Options.Converters.OfType<ISerialization>().
            Where(x => x.CanDeserializationAssignment(deserializeType)).ToArray();
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的选项初始化对象
        /// </summary>
        /// <param name="options">封装的Json选项，本类型的功能就是通过它实现的</param>
        public OptionsJsonFit(JsonSerializerOptions options)
        {
            this.Options = options;
        }
        #endregion
    }
}
