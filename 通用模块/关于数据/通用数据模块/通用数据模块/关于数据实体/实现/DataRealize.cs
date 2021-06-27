using System.Collections.Generic;
using System.Design.Direct;
using System.Linq;

namespace System.DataFrancis
{
    /// <summary>
    /// 这个类型是<see cref="IData"/>的实现，
    /// 可以被当作一个数据进行传递
    /// </summary>
    record DataRealize : DataBase, IData
    {
        #region 关于字典和架构
        #region 储存数据的字典
        protected override IRestrictedDictionary<string, object?> Data { get; }
        #endregion
        #region 架构约束
        private ISchema? SchemaField;

        public override ISchema? Schema
        {
            get => SchemaField;
            set
            {
                IDirect.CheckSchemaSet(this, value);
                SchemaField = value;
            }
        }
        #endregion
        #region 复制数据
        protected override IDirect CreateSelf()
            => new DataRealize(Array.Empty<KeyValuePair<string, object?>>(), false);
        #endregion
        #endregion 
        #region 构造函数
        /// <summary>
        /// 用一个键是列名的键值对集合（通常是字典）初始化数据
        /// </summary>
        /// <param name="dictionary">一个键值对集合，它的元素的键</param>
        /// <param name="copyValue">如果这个值为真，则会复制键值对的值，否则不复制</param>
        public DataRealize(IEnumerable<KeyValuePair<string, object?>> dictionary, bool copyValue = false)
        {
            Data = dictionary.ToDictionary(x => (x.Key, copyValue ? x.Value : null), true);
        }
        #endregion
    }
}
