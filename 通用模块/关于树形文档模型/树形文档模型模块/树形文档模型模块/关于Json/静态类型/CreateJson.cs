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
    /// 创建一个转换器，
    /// 它会使用一切可以使用的转换器去转换集合
    /// </summary>
    public static JsonConverter JsonCollection { get; } = new JsonConvertFactoryCollection();
    #endregion
    #region 常用Json序列化器
    /// <summary>
    /// 返回常用的Json序列化器，按照规范，
    /// 所有应用程序都应该添加这些序列化支持，
    /// 如果需要添加或删除本集合的元素，请在本集合被使用前执行这个操作
    /// </summary>
    public static ICollection<JsonConverter> JsonCommon { get; }
    #endregion
    #region 包含常用Json序列化器的选项
    /// <summary>
    /// 返回一个<see cref="JsonSerializerOptions"/>，
    /// 它包含所有在<see cref="JsonCommon"/>中的转换器
    /// </summary>
    public static JsonSerializerOptions JsonCommonOptions
    {
        get
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(JsonCommon);
            return options;
        }
    }
    #endregion
    #region 静态构造函数
    static CreateJson()
    {
        JsonCommon = new List<JsonConverter>()
        {
            JsonCollection
        };
    }
    #endregion
}
