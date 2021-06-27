using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.DataFrancis.DB;
using System.DataFrancis;
using System.Data.Common;
using System.Data;

namespace System
{
    /// <summary>
    /// 关于数据库的静态方法全部放在这里
    /// </summary>
    public static partial class @ExtenDB
    {
        #region 关于IDataReader
        #region 读取IDataReader中的全部内容
        /// <summary>
        /// 读取<see cref="IDataReader"/>中的全部内容
        /// </summary>
        /// <param name="Reader">待读取的<see cref="IDataReader"/></param>
        /// <returns>枚举流中内容的枚举器</returns>
        public static IEnumerable<IData> ToDatas(this IDataReader Reader)
        {
            var Schema = Reader.GetSchemaTable().Rows.OfType<DataRow>().ToArray();
            var Titles = Schema.Select(x => x["ColumnName"].ToString()).ToArray();
            var Length = Titles.Length;
            while (Reader.Read())
            {
                var Values = new object?[Length];
                Reader.GetValues(Values);
                yield return CreateDataObj.Data(Titles.Zip(Values.Select(x => x is DBNull ? null : x)).ToArray());
            }
            if (Reader.NextResult())
                throw new NotSupportedException("为保证数据架构的一致性，同一个读取器不能返回两个表");
        }
        #endregion
        #endregion
    }
}
