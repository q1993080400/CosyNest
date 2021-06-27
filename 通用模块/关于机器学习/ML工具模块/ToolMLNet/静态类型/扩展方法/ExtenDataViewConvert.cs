using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Performance;
using System.Linq;
using System.DataFrancis;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.Design;
using IDataViewML = Microsoft.ML.IDataView;

namespace System
{
    public static partial class @ExtenML
    {
        //这个部分类储存了关于IData和DataView互相转换的方法
        
        #region 从IData加载IDataView
        #region 说明文档
        /*问：从IData转换到IDataView的过程比较复杂，
          需要先把IData保存为文件，然后再加载到IDataView，这是为什么？
          答：这是不得已为之，根源在于微软的设计存在缺陷，
          因为IDataView有一个DataViewSchema类型的成员，
          它的构造函数不公开，导致在外部无法实现IDataView，
          进一步导致无法使用适配器模式，如果你有更好的办法，欢迎联系作者

          补充说明：经过查阅文档，作者发现IDataView实际上是可以被实现的，
          但由于作者对IDataView的结构了解不够清楚，为避免未知错误，决定暂时不重构，
          如果在以后随着更深入的学习，这个问题得到解决，请回来将这个API重构掉
           
          问：为什么不在加载完毕后立即删除缓存， 而是要等到程序退出后才删除？
          答：因为IDataView是延迟加载的，如果立即删除缓存，
          有可能在实际加载时找不到被加载的数据*/
        #endregion
        #region 辅助成员
        #region 通过IDATAView获取Column
        /// <summary>
        /// 读取<see cref="DataFrancis.IDataView"/>的列，并将它转换为<see cref="TextLoader.Column"/>
        /// </summary>
        /// <param name="data">待转换的<see cref="IData"/></param>
        /// <returns></returns>
        private static IEnumerable<TextLoader.Column> GetColumn(IDataViewF data)
        {
            foreach (var ((name, type), index, _) in data.Schema.Schema.PackIndex())
            {
                var dk = Enum.Parse(typeof(DataKind), type.Name);
                yield return new TextLoader.Column(name, (DataKind)dk, index);
            }
        }
        #endregion
        #endregion
        #region 正式方法
        /// <summary>
        /// 通过<see cref="DataFrancis.IDataView"/>来加载机器学习数据
        /// </summary>
        /// <param name="datas">待加载的数据</param>
        /// <returns></returns>
        public static IDataViewML ToIDataView(this IDataViewF datas)
        {
            var dataView = datas.ToArray().ToIDirectView();
            var file = ToolPerfo.CreateTemporaryFile(@"txt").Path;
            CreateDataObj.PipeFromFile(file, "\t", "null").DataAdd(dataView, default);
            var op = new TextLoader.Options()
            {
                HasHeader = true,
                Separators = new[] { '\t' },
                Columns = GetColumn(dataView).ToArray()
            };
            return ToolML.Context.Data.LoadFromTextFile(file, op);
        }
        #endregion
        #endregion
        #region 将IDataView转换为IData
        /// <summary>
        /// 将<see cref="Microsoft.ML.IDataView"/>转换为<see cref="DataFrancis.IDataView"/>
        /// </summary>
        /// <param name="data">待转换的<see cref="IDataView"/></param>
        /// <param name="maxRows">读取的最大列数，如果这个值过大，可能会对性能产生影响，默认为全部读取</param>
        /// <returns></returns>
        public static IDataViewF ToIData(this IDataViewML data, int maxRows = int.MaxValue)
            => CreateDataObj.DataView(data.Preview(maxRows).RowView.Select(x => CreateDataObj.Data(x.Values, true)).ToArray());
        #endregion
    }
}
