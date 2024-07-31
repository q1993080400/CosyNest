namespace System.NetFrancis.Http;

/// <summary>
/// 这个类型是<see cref="IObjectHeaderValue"/>的实现，
/// 可以视为一个封装了序列化后数据的Http标头
/// </summary>
/// <param name="Json">Json格式的数据</param>
sealed record ObjectHeaderValue(string Json) : IObjectHeaderValue
{
    #region 数据的Json形式
    public string Json { get; } = Json;
    #endregion 
}
