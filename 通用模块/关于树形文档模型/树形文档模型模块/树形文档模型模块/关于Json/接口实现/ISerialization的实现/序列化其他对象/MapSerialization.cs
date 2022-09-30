using System.Text.Json;

namespace System.TreeObject.Json;

/// <summary>
/// 该类型可以通过映射类型来进行序列化和反序列化
/// </summary>
/// <typeparam name="Map">映射类型的类型</typeparam>
/// <typeparam name="Output">可序列化的类型</typeparam>
sealed class MapSerialization<Map, Output> : SerializationBase<Output>
{
    #region 说明文档
    /*问：什么叫映射类型？
      答：映射类型是真正要序列化的类型的降维，
      它仅包含需要序列化的属性，而且属性名称和类型和真正序列化的类型相同，
      在执行序列化时，先将真实类型转换为映射类型，再进行序列化，
      在执行反序列化时，先将Json字符串反序列化为映射类型，再将映射类型转换为真实类型

      举例说明：该类型定义就是System.Security.Principal.IIdentity接口的映射类型

      struct PseudoIIdentity
      {
          public string? AuthenticationType { get; set; }
          public string? Name { get; set; }
      }

      问：使用映射类型有什么好处？
      答：它可以极大地简化序列化器的适配，
      仅需要映射类型，以及在映射类型和真实类型之间进行转换的委托，
      就可以适配任何复杂的类型，包括不能直接实例化的抽象类，接口，构造函数需要参数的类型等

      问：声明映射类型应遵循什么原则？
      答：请遵循以下原则：
      #仅包含需要序列化的属性，而且属性名称和类型和真正序列化的类型相同，
      这是利用了Json没有类型元数据的特点，凡是属性名称和类型相同的类型，在Json中视为等同

      #类型定义尽可能简单，只包括自动属性，
      而且推荐使用私有内部类，因为它不需要被外部访问到

      #可以直接被JsonSerializer序列化和反序列化，不需要指定JsonSerializerOptions

      #如果不需要考虑序列化null值的情况，则尽量使用值类型，因为它们仅在序列化方法中使用，
      而值类型出栈后会被立即释放，在这个场景下拥有更高的性能*/
    #endregion
    #region 关于转换映射类型
    #region 转换为映射类型
    /// <summary>
    /// 将实际类型转换为映射类型的委托
    /// </summary>
    private Func<Output?, Map?> ToMap { get; }
    #endregion
    #region 从映射类型转换
    /// <summary>
    /// 从映射类型转换为实际类型的委托
    /// </summary>
    private Func<Map?, Output?> FromMap { get; }
    #endregion
    #endregion
    #region 关于序列化与反序列化
    #region 序列化null
    protected override void WriteNull(Utf8JsonWriter writer)
    {
        var map = ToMap(ExtenTool.To<Output>(null));
        JsonSerializer.Serialize(map!);
    }
    #endregion
    #region 序列化
    protected override void WriteTemplate(Utf8JsonWriter writer, Output value, JsonSerializerOptions options)
        => JsonSerializer.Serialize(writer, ToMap(value), options);
    #endregion
    #region 反序列化null
    protected override Output? ReadNull()
        => FromMap(ExtenTool.To<Map>(null));
    #endregion
    #region 反序列化
    protected override Output ReadTemplate(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => FromMap(JsonSerializer.Deserialize<Map>(ref reader, options))!;
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="toMap">将实际类型转换为映射类型的委托</param>
    /// <param name="fromMap">从映射类型转换为实际类型的委托</param>
    public MapSerialization(Func<Output?, Map?> toMap, Func<Map?, Output?> fromMap)
    {
        this.ToMap = toMap;
        this.FromMap = fromMap;
    }
    #endregion
}
