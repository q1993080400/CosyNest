using System.Collections.Generic;
using System.Office.Excel;

namespace System.DataFrancis.Excel.Block
{
    /// <summary>
    /// 这个类型是<see cref="IDataBinding"/>的实现，
    /// 可以通过块绑定数据
    /// </summary>
    class BlockBinding : IDataBinding
    {
        #region 底层成员
        #region 数据所在的块
        /// <summary>
        /// 获取数据所在的块
        /// </summary>
        private IExcelCells Block { get; }
        #endregion
        #region 枚举读写属性的方式
        /// <summary>
        /// 这个字典的键是属性的名称，
        /// 值是用来读写属性的方式
        /// </summary>
        private IReadOnlyDictionary<string, IBlockProperty> Property { get; }
        #endregion
        #endregion
        #region 数据通知数据源
        #region 通知修改
        public void NoticeUpdateToSource(string columnName, object? newValue)
        {
            if (Block.Book.IsAvailable)
                Property[columnName].SetValue(Block, newValue);
        }
        #endregion
        #region 通知删除
        public void NoticeDeleteToSource()
        {
            if (Block.Book.IsAvailable)
                Block.Value = null;
        }
        #endregion
        #endregion
        #region 数据源通知数据
        #region 通知修改
        public event Action<string, object?>? NoticeUpdateToData
        {
            add { }
            remove { }
        }
        #endregion
        #region 通知删除
        public event Action? NoticeDeleteToData
        {
            add { }
            remove { }
        }
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的块和读写属性的方式初始化对象
        /// </summary>
        /// <param name="block">数据所在的块</param>
        /// <param name="property">这个字典的键是属性的名称，
        /// 值是用来读写属性的方式</param>
        public BlockBinding(IExcelCells block, IReadOnlyDictionary<string, IBlockProperty> property)
        {
            this.Block = block;
            this.Property = property;
        }
        #endregion
    }
}
