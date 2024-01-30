using System.Reflection;
using System.Text.Json.Serialization;
using System.Design.Direct;

namespace System.TreeObject.Json;

/// <summary>
/// 这个静态类被用来创建有关Json的对象
/// </summary>
public static class CreateJson
{
    #region 创建集合转换器
    /// <summary>
    /// 创建一个转换器，
    /// 它会使用一切可以使用的转换器去转换集合
    /// </summary>
    public static JsonConverter JsonCollection { get; } = new JsonConvertFactoryCollection();
    #endregion
    #region 创建元组转换器
    /// <summary>
    /// 创建一个转换器，
    /// 它可以用来转换元组
    /// </summary>
    public static JsonConverter JsonTuple { get; } = new JsonConvertFactoryTuple();
    #endregion
    #region 创建投影转换器
    /// <summary>
    /// 创建一个投影转换器，它可以将复杂的类型转换为一个简单的类型，
    /// 再将其转换为Json，或执行此操作的反向操作
    /// </summary>
    /// <inheritdoc cref="JsonConvertMap{ConvertTo, Map}"/>
    /// <inheritdoc cref="JsonConvertMap{ConvertTo, Map}.JsonConvertMap(Func{ConvertTo, Map}, Func{Map, ConvertTo})"/>
    public static JsonConverter<ConvertTo> JsonMap<ConvertTo, Map>(Func<ConvertTo, Map> toMap, Func<Map, ConvertTo> fromMap)
        => new JsonConvertMap<ConvertTo, Map>(toMap, fromMap);
    #endregion
    #region 创建多态转换器
    /// <summary>
    /// 创建一个转换器，
    /// 它允许执行多态Json转换
    /// </summary>
    /// <returns></returns>
    /// <param name="assemblies">转换器会从这些程序集中搜索多态反序列化的类型，
    /// 如果为<see langword="null"/>，默认为<typeparamref name="Obj"/>所在的程序集</param>
    /// <inheritdoc cref="JsonConvertPolymorphic{Obj}.JsonConvertPolymorphic(IEnumerable{Assembly})"/>
    /// <inheritdoc cref="JsonConvertPolymorphic{Obj}"/>
    public static JsonConverter<Obj> JsonPolymorphic<Obj>(IEnumerable<Assembly>? assemblies = null)
        where Obj : class
        => new JsonConvertPolymorphic<Obj>(assemblies ?? new[] { typeof(Obj).Assembly });
    #endregion
    #region 创建IDirect转换器
    /// <summary>
    /// 返回一个支持序列化和反序列化<see cref="IDirect"/>的对象，
    /// 请注意，它只能转换<see cref="IDirect"/>，不能转换它的派生类型
    /// </summary>
    public static JsonConverter JsonDirect { get; } = new JsonConvertDirect();
    #endregion
}
