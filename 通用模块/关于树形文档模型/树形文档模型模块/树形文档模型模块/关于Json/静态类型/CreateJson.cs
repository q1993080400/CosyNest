using System.Collections;
using System.Maths;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace System.TreeObject.Json;

/// <summary>
/// 这个静态类被用来创建有关Json的对象
/// </summary>
public static class CreateJson
{
    #region 创建集合转换器
    /// <summary>
    /// 创建一个<see cref="SerializationBase{Output}"/>，
    /// 它会使用一切可用的转换器转换<see cref="ICollection{T}"/>，
    /// 前提是该类型具有无参数构造函数，或者它是一个数组
    /// </summary>
    public static SerializationBase<IEnumerable> JsonCollections { get; } = new SerializationCollections();
    #endregion
    #region 创建计量单位转换器
    /// <summary>
    /// 创建一个可以转换<see cref="IUnit{Template}"/>的对象，
    /// 它将<see cref="IUnit{Template}"/>转换为表示公制单位数量的数字，
    /// 然后再执行转换
    /// </summary>
    /// <typeparam name="UT">计量单位的单位类型</typeparam>
    /// <returns></returns>
    public static SerializationBase<IUnit<UT>> JsonUnit<UT>()
        where UT : IUT
        => JsonMap<decimal?, IUnit<UT>>
        (x => x?.ValueMetric ?? 0, x => CreateBaseMath.UnitMetric<UT>(x ?? 0));
    #endregion
    #region 创建Num的转换器
    /// <summary>
    /// 返回一个可以转换<see cref="Num"/>的对象
    /// </summary>
    public static SerializationBase<Num> JsonNum { get; }
    = JsonMap<decimal, Num>(x => x.Value, x => x);
    #endregion
    #region 创建映射类型转换器
    /// <summary>
    /// 创建一个转换器，它可以通过映射类型来转换对象
    /// </summary>
    /// <typeparam name="Map">映射类型的类型</typeparam>
    /// <typeparam name="Output">可序列化的类型</typeparam>
    /// <param name="toMap">将实际类型转换为映射类型的委托</param>
    /// <param name="fromMap">从映射类型转换为实际类型的委托</param>
    /// <returns></returns>
    public static SerializationBase<Output> JsonMap<Map, Output>
        (Func<Output?, Map?> toMap, Func<Map?, Output?> fromMap)
        => new MapSerialization<Map, Output>(toMap, fromMap);
    #endregion
    #region 创建字典转换器
    /// <summary>
    /// 返回一个转换器，它会使用一切可用的转换器来转换字典
    /// </summary>
    public static SerializationBase<IEnumerable> JsonDictionary { get; }
    = new SerializationDictionary();
    #endregion
    #region 创建元组转换器
    /// <summary>
    /// 创建一个转换器，它可以用来转换元组
    /// </summary>
    public static SerializationBase<ITuple> JsonTuple { get; }
        = new SerializationValueTuple();
    #endregion
    #region 常用Json序列化器
    /// <summary>
    /// 返回常用的Json序列化器，按照规范，
    /// 所有应用程序都应该添加这些序列化支持，
    /// 如果需要添加或删除本集合的元素，请在本集合被使用前执行这个操作
    /// </summary>
    public static IList<JsonConverter> SerializationCommon { get; }
        = new List<JsonConverter>()
        {
            JsonNum,
            JsonDictionary,
            JsonCollections,
            JsonTuple
        };
    #endregion
    #region 包含常用Json序列化器的选项
    /// <summary>
    /// 返回一个<see cref="JsonSerializerOptions"/>，
    /// 它包含所有在<see cref="SerializationCommon"/>中的转换器
    /// </summary>
    public static JsonSerializerOptions SerializationCommonOptions
    {
        get
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(SerializationCommon);
            return options;
        }
    }
    #endregion
}
