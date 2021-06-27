namespace System.DataFrancis
{
    /// <summary>
    /// 代表一个数据占位符，它不是真正的数据，
    /// 只是为了方便编写表达式而设计的占位符
    /// </summary>
    public sealed class PlaceholderData
    {
        #region 封装的数据对象
        /// <summary>
        /// 获取封装的数据对象
        /// </summary>
        internal IData Data { get; }
        #endregion
        #region 通过列名获取占位符
        /// <summary>
        /// 通过列名获取数据占位符
        /// </summary>
        /// <param name="columnName">数据占位符的列名</param>
        /// <returns></returns>
        public PlaceholderValue this[string columnName]
             => new(Data[columnName]);
        #endregion
        #region 返回数据的索引
        /// <summary>
        /// 返回数据在数据集中的索引
        /// </summary>
        public int Index { get; }

        /*说明文档：
          本属性可用于编写根据索引进行筛选的表达式，
          举例说明：查询最靠前的10个数据的表达式应该这样写：
          x=>x.Index<10*/
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的数据和索引初始化对象
        /// </summary>
        /// <param name="data">被封装的数据</param>
        /// <param name="index">数据在数据集中的索引</param>
        public PlaceholderData(IData data, int index)
        {
            this.Data = data;
            this.Index = index;
        }
        #endregion
    }
}
