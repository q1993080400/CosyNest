using System.Design.Direct;
using System.Text.Json.Serialization;

namespace System.TreeObject.Json;

/// <summary>
/// 这个静态类被用来创建有关Json的对象
/// </summary>
public static class CreateJson
{
    #region 创建IDirect转换器
    /// <summary>
    /// 返回一个支持序列化和反序列化<see cref="IDirect"/>的对象，
    /// 请注意，它只能转换<see cref="IDirect"/>，不能转换它的派生类型
    /// </summary>
    public static JsonConverter JsonDirect { get; } = new JsonConverterDirect();
    #endregion
}
