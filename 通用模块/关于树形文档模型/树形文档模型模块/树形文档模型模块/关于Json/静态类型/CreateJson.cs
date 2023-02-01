using System.Text.Json.Serialization;

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
}
