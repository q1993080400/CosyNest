using System.Text;
using System.Text.Json;

namespace System.TreeObject
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以序列化和反序列化树形文档对象
    /// </summary>
    public interface ISerialization
    {
        #region 说明文档
        /*问：为什么本类型要把可序列化的类型和可反序列化的类型分开？
          答：因为为了最大程度的泛化接口，可序列化的类型应该支持逆变，
          可反序列化的类型应该支持协变，因此将它们分开是一个更好的设计

          问：为什么不使用一个泛型参数来表示可反序列化的类型？
          答：因为为了最大程度的泛化接口，可反序列化的类型具有比较复杂的规则：

          首先，它应该支持协变，假设一个ISerialization可以反序列化IDirect，
          那么可以用object来接收反序列化的结果

          然后，它应该有限地支持逆变，假设一个ISerialization可以反序列化IDirect，
          但实际上它反序列化的具体返回值是DataRealize，那么在这种情况下，
          无论反序列化的输出类型是IDirect，IData，DataRealize，
          或者任何实现了IDirect，但是具有公开无参数构造函数的类型，
          反序列化功能都应该能够正常工作

          综上所述，使用泛型参数约束不能准确地描述这些规则，它只能在运行中进行动态地检查

          问：本接口的功能与BCL原生的类型JsonConverter非常相似，
          是否应该把它们统一起来？
          答：不应该，理由如下：
          1.设计目的不同，本接口可以支持任何序列化协议，
          包括Json，Xml，二进制等，只要实现者提供了对应实现，
          而JsonConverter只能用来序列化Json
          2.本接口的API提高了抽象层次，不假设调用者只使用UTF8编码，
          降低了性能但是增强了灵活性，但如果你指定使用UTF8编码，则没有性能代价
          3.本接口的设计更加合理，JsonConverter的类型设计不遵循里氏替换原则，
          事实上，只有JsonConverter<T>具有直接进行序列化和反序列化的API，这给开发带来了很大不便
          4.很多Net框架（例如ASP.NetCore）依赖于JsonConverter，
          因此不宜将这个类型完全取代
        
          问：在何种情况下应该选择本接口或JsonConverter？
          答：根据规范，如果需要和BCL的JsonApi进行交互的话，
          本接口的实现应该以SerializationBase的形式公开，
          它既实现了本接口，又继承了JsonConverter，可以同时兼容两者，
          如果不需要的话，可直接使用本接口*/
        #endregion
        #region 静态方法
        #region 如果协议不匹配，则引发异常
        /// <summary>
        /// 如果支持的协议和传入的协议不匹配，则引发异常
        /// </summary>
        /// <param name="support">受支持的协议</param>
        /// <param name="check">传入的协议</param>
        public static void CheckAgreement(string support, string check)
        {
            if (!support.EqualsIgnore(check))
                throw new JsonException($"仅支持协议{support}，不支持协议{check}");
        }
        #endregion
        #region 如果不可序列化，则引发异常
        /// <summary>
        /// 检查要序列化的类型和对象，
        /// 如果类型不可序列化，或者要序列化的类型无法赋值给要序列化的对象，则引发异常
        /// </summary>
        /// <param name="serialization">用来进行序列化的对象</param>
        /// <param name="input">被序列化的对象，注意：
        /// 如果它为<see langword="null"/>，则不会进行下一步检查，
        /// 因为函数默认<see langword="null"/>可以被任何实现序列化</param>
        /// <param name="inputType">序列化的输入类型</param>
        protected static void CheckSerialization(ISerialization serialization, object? input, Type? inputType)
        {
            if (input is null)
                return;
            var ip = input.GetType();
            inputType ??= ip;
            if (!serialization.CanSerialization(inputType))
                throw new JsonException($"类型{inputType}不可序列化");
            if (!inputType.IsAssignableFrom(ip))
                throw new JsonException($"序列化的目标类型{inputType}不能赋值给要序列化的对象");
        }
        #endregion
        #region 如果不可反序列化，则引发异常
        /// <summary>
        /// 检查要反序列化的类型，
        /// 如果不可反序列化，则引发异常
        /// </summary>
        /// <param name="serialization">用来进行反序列化的对象</param>
        /// <param name="outputType">反序列化的输出类型</param>
        protected static void CheckDeserialize(ISerialization serialization, Type outputType)
        {
            if (!serialization.CanDeserializationAssignment(outputType))
                throw new JsonException($"类型{outputType}不可反序列化");
        }
        #endregion
        #endregion
        #region 返回协议名称
        /// <summary>
        /// 返回描述树形文档对象的协议名称，
        /// 例如Xml，Json等
        /// </summary>
        string Agreement { get; }
        #endregion
        #region 关于序列化
        #region 是否可序列化
        /// <summary>
        /// 检查指定的类型是否可序列化，
        /// 注意：它不负责检查是否可反序列化
        /// </summary>
        /// <param name="type">待检查的类型</param>
        /// <returns>如果可以被序列化，则返回<see langword="true"/>，
        /// 否则返回<see langword="false"/></returns>
        bool CanSerialization(Type type);
        #endregion
        #region 序列化为指定编码
        /// <summary>
        /// 将受支持的对象序列化为指定编码
        /// </summary>
        /// <param name="obj">待序列化的对象</param>
        /// <param name="inputType">序列化的目标类型，
        /// 如果为<see langword="null"/>，则直接使用<paramref name="obj"/>的具体类型</param>
        /// <param name="options">指定序列化中使用的选项</param>
        /// <param name="encoding">序列化的目标编码，
        /// 如果为<see langword="null"/>，默认为UTF8</param>
        /// <returns></returns>
        ReadOnlySpan<byte> Serialization(object? obj, Type? inputType = null, IOptionsBase? options = null, Encoding? encoding = null);
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
        #endregion
        #region 关于反序列化
        #region 是否可反序列化
        /// <summary>
        /// 传入一个类型，如果它可以用来接收反序列化的返回值，
        /// 则返回<see langword="true"/>，否则返回<see langword="false"/>
        /// </summary>
        /// <param name="type">指定的类型</param>
        /// <returns></returns>
        bool CanDeserializationAssignment(Type type);

        /*注意：请仔细阅读本API的说明，
          它的含义和CanSerialization有点不一样，
          根据它的定义，如果参数传入typeof(object)，则永远返回true*/
        #endregion
        #region 非泛型方法
        #region 从指定的编码反序列化
        /// <summary>
        /// 从指定的编码反序列化对象
        /// </summary>
        /// <param name="text">用来描述对象的文本</param>
        /// <param name="outputType">指定反序列化的输出类型，
        /// 如果为<see langword="null"/>，则使用默认类型，该参数可为协变反序列化提供支持</param>
        /// <param name="options">指定反序列化中使用的选项</param>
        /// <param name="encoding">文本的编码，
        /// 如果为<see langword="null"/>，默认为UTF8</param>
        /// <returns></returns>
        object? Deserialize(ReadOnlySpan<byte> text, Type? outputType = null, IOptionsBase? options = null, Encoding? encoding = null);
        #endregion
        #region 从UTF16反序列化
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
        #endregion
        #region 泛型方法
        #region 从指定的编码反序列化
        /// <inheritdoc cref="Deserialize(ReadOnlySpan{byte}, Type?, IOptionsBase?, Encoding?)"/>
        /// <typeparam name="Obj">反序列化的返回值类型</typeparam>
        Obj? Deserialize<Obj>(ReadOnlySpan<byte> text, IOptionsBase? options = null, Encoding? encoding = null)
            => (Obj?)Deserialize(text, typeof(Obj), options, encoding);
        #endregion
        #region 从UTF16反序列化
        /// <inheritdoc cref="DeserializeUTF16(string, Type?, IOptionsBase?)"/>
        /// <typeparam name="Obj">反序列化的返回值类型</typeparam>
        Obj? DeserializeUTF16<Obj>(string text, Type? outputType = null, IOptionsBase? options = null)
            => (Obj?)DeserializeUTF16(text, typeof(Obj));
        #endregion
        #endregion
        #endregion
    }
}
