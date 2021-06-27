using System.Collections.Generic;
using System.Design.Direct;
using System.Linq;
using System.Text.Json.Serialization;
using System.TreeObject.Json;

namespace System.DataFrancis
{
    /// <summary>
    /// 这个静态类可以用来帮助创建一些关于数据的对象
    /// </summary>
    public static class CreateDataObj
    {
        #region 创建IData
        #region 用指定的列名和值
        /// <summary>
        /// 用指定的列名和值创建<see cref="IData"/>
        /// </summary>
        /// <param name="parameters">这个数组的元素是一个元组，
        /// 分别指示列的名称和值</param>
        public static IData Data(params (string Column, object? Value)[] parameters)
            => Data(parameters.ToDictionary(true));
        #endregion
        #region 用指定的列名
        /// <summary>
        /// 用指定的数据列名创建<see cref="IData"/>
        /// </summary>
        /// <param name="columnName">指定的列名，一经初始化不可增减</param>
        public static IData Data(params string[] columnName)
            => Data(columnName.Select(x => (x, (object?)null)).ToArray());
        #endregion
        #region 用指定的字典
        /// <summary>
        /// 用一个键是列名的键值对集合（通常是字典）创建<see cref="IData"/>
        /// </summary>
        /// <param name="dictionary">一个键值对集合，它的元素的键</param>
        /// <param name="copyValue">如果这个值为真，则会复制键值对的值，否则不复制</param>
        public static IData Data(IEnumerable<KeyValuePair<string, object?>> dictionary, bool copyValue = true)
            => new DataRealize(dictionary, copyValue);
        #endregion
        #region 将多条数据合并 
        /// <summary>
        /// 将一条数据和另一些数据合并，并返回合并后的新数据
        /// </summary>
        /// <param name="data">待合并的数据</param>
        /// <param name="dataMerge">待合并的另一些数据，
        /// 如果存在列名相同的数据，则以后面的数据为准</param>
        /// <returns></returns>
        public static IData DataMerge(IEnumerable<KeyValuePair<string, object?>> data, params (string Name, object? Value)[] dataMerge)
            => Data(data.Union
                (dataMerge.Select
                (x => x.ToKV())));
        #endregion
        #endregion
        #region 创建序列化和反序列化对象
        #region 适用于IDirect
        /// <summary>
        /// 返回一个支持序列化和反序列化<see cref="IDirect"/>的对象，
        /// 它支持协变反序列化，除<see cref="IDirect"/>以外，
        /// 还可以反序列化<see cref="IData"/>以及实现<see cref="IDirect"/>，
        /// 并具有无参数构造函数的类型
        /// </summary>
        /// <returns></returns>
        public static SerializationBase<IDirect> JsonDirect { get; } = new SerializationIDirect();
        #endregion
        #endregion
    }
}
