namespace System.Collections.Generic
{
    /// <summary>
    /// 凡是实现这个接口的类型，都可以视为一个受限字典，
    /// 它可以读取或写入键值对，但是不能添加或删除键值对
    /// </summary>
    /// <typeparam name="Key">字典的键类型</typeparam>
    /// <typeparam name="Value">字典的值类型</typeparam>
    public interface IRestrictedDictionary<Key, Value> : IReadOnlyDictionary<Key, Value>
          where Key : notnull
    {
        #region 读取和写入键值对
        /// <summary>
        /// 通过键，读取或写入键值对
        /// </summary>
        /// <param name="key">用来读取或写入键值对的键</param>
        /// <returns></returns>
        new Value this[Key key] { get; set; }
        #endregion
    }
}
