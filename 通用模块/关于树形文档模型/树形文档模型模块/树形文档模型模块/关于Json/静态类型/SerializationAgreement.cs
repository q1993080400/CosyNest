namespace System.TreeObject;

/// <summary>
/// 这个类型储存了<see cref="ISerializationText"/>所支持的一些协议名称
/// </summary>
public static class SerializationAgreement
{
    #region Json
    /// <summary>
    /// 返回Json的协议名称
    /// </summary>
    public const string Json = "Json";
    #endregion
    #region Xml
    /// <summary>
    /// 返回Xml的协议名称
    /// </summary>
    public const string Xml = "Xml";
    #endregion
}
