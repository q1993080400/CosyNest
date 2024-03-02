using System.IOFrancis.FileSystem;
using System.Media.Drawing;
using System.Media.Drawing.Graphics;
using System.Office.Chart;

namespace System.Office.Excel;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以作为一个Excel工作表
/// </summary>
public interface IExcelSheet : IExcelCellsCommunity
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
    /// <param name="collection">目标工作簿的工作表容器，
    /// 如果为<see langword="null"/>，则复制到本工作簿</param>
    /// <param name="renamed">一个用于修改工作表名，使其不重名的委托，
    /// 它的第一个参数是旧名称，第二个参数是尝试失败的次数，从2开始，返回值就是新的名称，
    /// 如果为<see langword="null"/>，则使用一个默认方法</param>
    /// <returns>复制后的新工作表</returns>
    IExcelSheet Copy(IExcelSheetCollection? collection = null, Func<string, int, string>? renamed = null);
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
    #region 返回所有单元格
    /// <summary>
    /// 返回工作表的所有单元格
    /// </summary>
    IExcelCells Cell { get; }

    /*实现本API请遵循以下规范：
      #如果底层实现支持，则应该尽可能裁剪空白单元格，例如：
      假设工作表中只有R3C3一个单元格有值，则该属性应该返回R1C1:R3C3，
      这是为了遍历单元格时尽可能地节约性能*/
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
    #region 指定图像
    /// <summary>
    /// 向工作表中添加图像，
    /// 并返回新添加的图像
    /// </summary>
    /// <param name="image">待添加的图像</param>
    /// <returns></returns>
    IExcelObj<IImage> CreateImage(IImage image);
    #endregion
    #region 指定图像路径
    /// <param name="path">图像所在的路径</param>
    /// <inheritdoc cref="CreateImage(IImage)"/>
    IExcelObj<IImage> CreateImage(PathText path);
    #endregion
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
