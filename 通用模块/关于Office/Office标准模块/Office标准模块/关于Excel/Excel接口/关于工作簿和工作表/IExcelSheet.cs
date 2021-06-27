using System.Collections.Generic;
using System.DrawingFrancis;
using System.DrawingFrancis.Graphics;
using System.Maths;
using System.Office.Chart;

namespace System.Office.Excel
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以作为一个Excel工作表
    /// </summary>
    public interface IExcelSheet
    {
        #region 关于工作簿与工作表
        #region 返回工作表所在的工作簿
        /// <summary>
        /// 返回本工作表所在的工作簿
        /// </summary>
        IExcelBook Book { get; }
        #endregion
        #region 工作表的名称
        /// <summary>
        /// 获取或设置工作表的名称
        /// </summary>
        string Name { get; set; }
        #endregion
        #region 对工作表的操作
        #region 复制工作表
        /// <summary>
        /// 将这个工作表复制到新工作簿，
        /// 并放置在工作表集合的末尾
        /// </summary>
        /// <param name="collection">目标工作簿的工作表容器</param>
        /// <param name="renamed">一个用于修改工作表名，使其不重名的委托，
        /// 它的第一个参数是旧名称，第二个参数是尝试失败的次数，从2开始，返回值就是新的名称，
        /// 如果为<see langword="null"/>，则使用一个默认方法</param>
        /// <returns>复制后的新工作表</returns>
        IExcelSheet Copy(IExcelSheetCollection collection, Func<string, int, string>? renamed = null);
        #endregion
        #region 剪切工作表
        /// <summary>
        /// 将这个工作表剪切到其他工作簿
        /// </summary>
        /// <param name="collection">目标工作簿的工作表容器</param>
        /// <param name="renamed">一个用于修改工作表名，使其不重名的委托，
        /// 它的第一个参数是旧名称，第二个参数是尝试失败的次数，从2开始，返回值就是新的名称，
        /// 如果为<see langword="null"/>，则使用一个默认方法</param>
        /// <returns>剪切后的新工作表</returns>
        IExcelSheet Cut(IExcelSheetCollection collection, Func<string, int, string>? renamed = null)
        {
            var newSheet = Copy(collection, renamed);
            Delete();
            return newSheet;
        }
        #endregion
        #region 删除工作表
        /// <summary>
        /// 将这个工作表从工作簿中删除
        /// </summary>
        void Delete();

        /*实现本API请遵循以下规范：
          如果删除了工作簿中的唯一工作表，
          则不会引发异常，而是将工作簿文件删除*/
        #endregion
        #endregion
        #region 返回页面对象
        /// <summary>
        /// 返回页面对象，
        /// 它可以管理这个工作表的页面设置和打印
        /// </summary>
        IPageSheet Page { get; }
        #endregion
        #endregion
        #region 关于单元格
        #region 索引器
        #region 根据绝对位置
        /// <summary>
        /// 根据起始行列号和结束行列号，
        /// 返回一个或多个单元格
        /// </summary>
        /// <param name="beginRow">开始单元格的行号</param>
        /// <param name="beginColumn">开始单元格的列号</param>
        /// <param name="endRow">结束单元格的行号，如果小于0，代表和起始单元格相同</param>
        /// <param name="endColumn">结束单元格的列号，如果小于0，代表和起始单元格相同</param>
        /// <returns></returns>
        IExcelCells this[int beginRow, int beginColumn, int endRow = -1, int endColumn = -1]
            => RangUser[beginRow, beginColumn, endRow, endColumn];
        #endregion
        #region 根据相对位置
        /// <summary>
        /// 根据起始行列号和单元格大小，返回一个或多个单元格
        /// </summary>
        /// <param name="beginRow">起始单元格的行号</param>
        /// <param name="beginColumn">起始单元格的列号</param>
        /// <param name="size">这个元组指示单元格的行数和列数</param>
        /// <returns></returns>
        IExcelCells this[int beginRow, int beginColumn, (int RowCount, int ColumnCount) size]
            => RangUser[beginRow, beginColumn, size];
        #endregion
        #region 根据平面
        /// <summary>
        /// 根据一个平面，返回一个单元格
        /// </summary>
        /// <param name="rectangle">这个平面被用来描述单元格的大小和位置，
        /// 如果它的坐标有负数，那么会取其绝对值</param>
        /// <returns></returns>
        IExcelCells this[ISizePosPixel rectangle]
            => RangUser[rectangle];
        #endregion
        #endregion
        #region 返回用户范围
        /// <summary>
        /// 返回工作表的用户范围，也就是存在非空单元格的范围
        /// </summary>
        IExcelCells RangUser { get; }

        /*实现本API请遵循以下规范：
          #无论第一个非空单元格在哪里，
          用户范围都应该从工作表的最左上角开始，举例说明：
          假如整个工作表只有R3C3一个单元格有值，
          那么用户范围应该是R1C1:R3C3，而不是R3C3，
          这样做的目的是与日常使用Excel的习惯相符合*/
        #endregion
        #region 返回行或者列
        /// <summary>
        /// 从这个工作表返回行或者列
        /// </summary>
        /// <param name="begin">开始行列号</param>
        /// <param name="end">结束行列号，如果这个值为<see langword="null"/>，
        /// 则默认为与开始行列数相等</param>
        /// <param name="isRow">如果这个值为<see langword="true"/>，
        /// 代表返回行，否则返回列</param>
        /// <returns></returns>
        IExcelRC GetRC(int begin, int? end, bool isRow);
        #endregion
        #endregion
        #region 关于Office对象
        #region 关于图表
        #region 获取图表创建器
        /// <summary>
        /// 获取一个图表创建器，
        /// 它可以用来帮助创建Excel图表
        /// </summary>
        ICreateExcelChart CreateChart { get; }

        /*实现本API请遵循以下规范：
          #在创建图表后，自动将其添加到工作表中*/
        #endregion
        #region 枚举工作表中的所有图表
        /// <summary>
        /// 枚举该工作表中的所有图表
        /// </summary>
        IEnumerable<IExcelObj<IOfficeChart>> Charts { get; }
        #endregion
        #endregion
        #region 关于图像
        #region 枚举工作表中的所有图像
        /// <summary>
        /// 枚举工作表中的所有图像
        /// </summary>
        IEnumerable<IExcelObj<IImage>> Images { get; }
        #endregion
        #region 向工作表中添加图像
        /// <summary>
        /// 向工作表中添加图像，
        /// 并返回新添加的图像
        /// </summary>
        /// <param name="image">待添加的图像</param>
        /// <returns></returns>
        IExcelObj<IImage> CreateImage(IImage image);
        #endregion
        #endregion
        #region 返回画布
        /// <summary>
        /// 返回一个画布，
        /// 它可以直接在Excel工作表上绘制形状
        /// </summary>
        ICanvas Canvas { get; }
        #endregion
        #endregion
    }
}