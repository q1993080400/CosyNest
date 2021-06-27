using System.Collections.Generic;

namespace System.Design.Direct
{
    /// <summary>
    /// 这个类型是<see cref="ISchema"/>的实现，
    /// 可以用来表示数据架构
    /// </summary>
    class Schemas : ISchema
    {
        #region 返回数据的架构
        public IReadOnlyDictionary<string, Type> Schema { get; }
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的架构初始化对象
        /// </summary>
        /// <param name="schema">指定的架构</param>
        public Schemas(IReadOnlyDictionary<string, Type> schema)
        {
            this.Schema = schema;
        }
        #endregion
    }
}
