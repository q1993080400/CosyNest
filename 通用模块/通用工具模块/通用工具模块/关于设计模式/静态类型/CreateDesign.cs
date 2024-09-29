using System.Design.Direct;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace System.Design;

/// <summary>
/// 这个静态类可以用来帮助创建和设计模式有关的类型
/// </summary>
public static class CreateDesign
{
    #region 创建事件总线工厂
    /// <summary>
    /// 创建一个事件总线工厂，
    /// 它可以用来创建事件总线上下文
    /// </summary>
    /// <returns></returns>
    public static IEventBusFactory EventBusFactory()
        => new EventBusFactory();
    #endregion
    #region 创建IDirect
    #region 创建简易实现
    /// <summary>
    /// 创建一个简易的<see cref="IDirect"/>实现，
    /// 它直接封装了一个字典
    /// </summary>
    /// <returns></returns>
    public static IDirect DirectEmpty()
        => new DirectSimple();
    #endregion
    #endregion
    #region 创建Block
    /// <summary>
    /// 返回一个<see cref="IBlock"/>，它可以用来阻塞当前线程，
    /// 通过它可以实现令牌桶限流算法
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="TokenBucket(int, TimeSpan)"/>
    public static IBlock BlockTokenBucket(int maxToken, TimeSpan resetInterval)
        => new TokenBucket(maxToken, resetInterval);
    #endregion
    #region 常用Json序列化器
    /// <summary>
    /// 返回常用的Json序列化器，按照规范，
    /// 所有应用程序都应该添加这些序列化支持，
    /// 如果需要添加或删除本集合的元素，请在本集合被使用前执行这个操作
    /// </summary>
    public static IList<JsonConverter> JsonCommon { get; } = [];
    #endregion
    #region 包含常用Json序列化器的选项
    /// <summary>
    /// 返回一个<see cref="JsonSerializerOptions"/>，
    /// 它包含所有在<see cref="JsonCommon"/>中的转换器
    /// </summary>
    /// <param name="jsonSerializerDefaults">指示序列化的选项</param>
    public static JsonSerializerOptions JsonCommonOptions
        (JsonSerializerDefaults jsonSerializerDefaults = JsonSerializerDefaults.General)
    {
        var options = new JsonSerializerOptions(jsonSerializerDefaults);
        options.Converters.Add(JsonCommon);
        return options;
    }
    #endregion
}
