namespace System.NetFrancis.Amazon;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个亚马逊KeyValueStore的键值对
/// </summary>
public interface IAmazonCloudFrontKeyValuePair : IAmazonCanModifiableObject
{
    #region 键对象
    /// <summary>
    /// 获取键对象
    /// </summary>
    string Key { get; }
    #endregion
    #region 值对象
    /// <summary>
    /// 获取或设置值对象
    /// </summary>
    string Value { get; set; }
    #endregion
}
