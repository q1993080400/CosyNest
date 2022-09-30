using System.Text;

namespace System.TreeObject;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以通过字符串序列化和反序列化树形文档对象
/// </summary>
public interface ISerializationText : ISerialization
{
    #region 说明文档
    /*问：本接口的功能与BCL原生的类型JsonConverter非常相似，
      是否应该把它们统一起来？
      答：不应该，理由如下：
      1.本接口的API提高了抽象层次，不假设调用者只使用UTF8编码，
      降低了性能但是增强了灵活性，但如果你指定使用UTF8编码，则没有性能代价
      2.本接口的设计更加合理，JsonConverter的类型设计不遵循里氏替换原则，
      事实上，只有JsonConverter<T>具有直接进行转换的API，这给开发带来了很大不便
      3.本接口的基接口ISerialization不仅限于Json，
      它还可以转换XML，二进制等编码方式，这为一切转换提供了统一抽象

      问：在何种情况下应该选择本接口或JsonConverter？
      答：根据规范，如果需要和BCL的JsonApi进行交互的话，
      本接口的实现应该以SerializationBase的形式公开，
      它既实现了本接口，又继承了JsonConverter，可以同时兼容两者，
      如果不需要的话，可直接使用本接口*/
    #endregion
    #region 序列化为UTF16
    /// <summary>
    /// 将受支持的对象序列化为UTF16
    /// </summary>
    /// <param name="obj">待序列化的对象</param>
    /// <param name="inputType">序列化的目标类型，
    /// 如果为<see langword="null"/>，则直接使用<paramref name="obj"/>的具体类型</param>
    /// <param name="options">指定序列化中使用的选项</param>
    /// <returns></returns>
    string SerializationUTF16(object? obj, Type? inputType = null, IOptionsBase? options = null)
    {
        var encoding = Encoding.Unicode;
        return encoding.GetString(Serialization(obj, inputType, options, encoding));
    }
    #endregion
    #region 从UTF16反序列化
    #region 非泛型方法
    /// <summary>
    /// 从UTF16反序列化对象
    /// </summary>
    /// <param name="text">用来描述对象的文本</param>
    /// <param name="outputType">指定反序列化的输出类型，
    /// 如果为<see langword="null"/>，则使用默认类型，该参数可为协变反序列化提供支持</param>
    /// <param name="options">指定反序列化中使用的选项</param>
    /// <returns></returns>
    object? DeserializeUTF16(string text, Type? outputType = null, IOptionsBase? options = null)
    {
        var encoding = Encoding.Unicode;
        return Deserialize(encoding.GetBytes(text), outputType, options, encoding);
    }
    #endregion
    #region 泛型方法
    /// <inheritdoc cref="DeserializeUTF16(string, Type?, IOptionsBase?)"/>
    /// <typeparam name="Obj">反序列化的返回值类型</typeparam>
    Obj? DeserializeUTF16<Obj>(string text, Type? outputType = null, IOptionsBase? options = null)
        => (Obj?)DeserializeUTF16(text, typeof(Obj));
    #endregion
    #endregion
}
