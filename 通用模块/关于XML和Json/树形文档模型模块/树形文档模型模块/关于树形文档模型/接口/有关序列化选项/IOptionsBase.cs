namespace System.TreeObject
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为序列化或反序列化树形文档对象的选项
    /// </summary>
    public interface IOptionsBase
    {
        #region 返回支持的协议类型
        /// <summary>
        /// 返回这个选项支持的协议类型，
        /// 例如Xml，Json等
        /// </summary>
        string Agreement { get; }
        #endregion
        #region 获取序列化器
        /// <summary>
        /// 尝试获取所有可用的序列化器，如果找不到，
        /// 则返回一个空数组
        /// </summary>
        /// <param name="serializationType">序列化的目标类型</param>
        /// <returns></returns>
        ISerialization[] GetSerialization(Type serializationType);
        #endregion
        #region 获取反序列化器
        /// <summary>
        /// 尝试获取所有可用的反序列化器，如果找不到，
        /// 则返回一个空数组
        /// </summary>
        /// <param name="deserializeType">用来接受反序列化的结果的类型</param>
        /// <returns></returns>
        ISerialization[] GetDeserialize(Type deserializeType);
        #endregion
    }
}
