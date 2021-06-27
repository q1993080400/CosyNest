namespace System.Collections.Generic
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以被视为一个只能添加和读取键值对，不能删除键值对的字典
    /// </summary>
    /// <typeparam name="Key">字典的键类型</typeparam>
    /// <typeparam name="Value">字典的值类型</typeparam>
    public interface IAddOnlyDictionary<Key, Value> : IReadOnlyDictionary<Key, Value>
        where Key : notnull
    {
        #region 指示是否可修改元素
        /// <summary>
        /// 如果这个值为<see langword="true"/>，
        /// 代表可以修改已经被添加到字典的值，否则代表禁止修改
        /// </summary>
        bool CanModify { get; }
        #endregion
        #region 根据键读写值
        /// <summary>
        /// 根据键读取或写入值，
        /// 如果<see cref="CanModify"/>为<see langword="false"/>，
        /// 在写入值的时候会引发异常
        /// </summary>
        /// <param name="key">用来获取值的键</param>
        /// <returns></returns>
        new Value this[Key key] { get; set; }
        #endregion
        #region 添加元素
        #region 传入键和值
        /// <summary>
        /// 将指定的键值对添加到字典中
        /// </summary>
        /// <param name="key">指定的键</param>
        /// <param name="value">指定的值</param>
        void Add(Key key, Value value);
        #endregion
        #region 传入键值对
        /// <summary>
        /// 在字典中添加一个键值对
        /// </summary>
        /// <param name="item">传入的键值对</param>
        void Add(KeyValuePair<Key, Value> item);
        #endregion
        #endregion
    }
}
