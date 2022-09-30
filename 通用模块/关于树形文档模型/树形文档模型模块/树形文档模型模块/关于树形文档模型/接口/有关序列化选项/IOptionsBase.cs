namespace System.TreeObject;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为序列化或反序列化树形文档对象的选项
/// </summary>
public interface IOptionsBase
{
    #region 获取序列化器
    /// <summary>
    /// 尝试获取所有可用的序列化器，如果找不到，
    /// 则返回一个空数组
    /// </summary>
    /// <param name="serializationType">序列化的目标类型</param>
    /// <returns></returns>
    ISerializationText[] GetSerialization(Type serializationType);
    #endregion
    #region 获取反序列化器
    /// <summary>
    /// 尝试获取所有可用的反序列化器，如果找不到，
    /// 则返回一个空数组
    /// </summary>
    /// <param name="deserializeType">用来接受反序列化的结果的类型</param>
    /// <returns></returns>
    ISerializationText[] GetDeserialize(Type deserializeType);
    #endregion
}
