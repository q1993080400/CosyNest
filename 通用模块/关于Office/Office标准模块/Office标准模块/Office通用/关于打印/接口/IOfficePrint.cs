﻿using System.IOFrancis.FileSystem;
using System.Underlying;

namespace System.Office;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个Office打印对象，
/// 它可以打印工作表，工作簿或Word文档，
/// 但是不提供页面设置功能
/// </summary>
public interface IOfficePrint
{
    #region 返回页数
    /// <summary>
    /// 返回全部页数
    /// </summary>
    int PageCount { get; }
    #endregion
    #region 关于打印
    #region 说明文档
    /*实现本API请遵循以下规范：
      #有关打印的API返回的Task用于等待整个打印过程完成，
      对于打印到文件，这个规范比较重要，因为打印完成之前，目标文件会被占用，
      对于打印到纸张，则没有那么重要，你可以选择不等待这个Task
    
      如果不知道打印完成的具体时间，则根据实际情况，估算一个时间并等待它，
      这个时间应该尽可能的充裕，因为不出Bug比性能重要*/
    #endregion
    #region 打印到纸张
    /// <summary>
    /// 按照页码将这个Excel工作表，工作簿，或Word文档打印到纸张
    /// </summary>
    /// <param name="page">这个参数指示打印的页码范围，
    /// 如果为<see langword="null"/>，代表全部打印，页码从0开始</param>
    /// <param name="number">打印的份数</param>
    /// <param name="printer">执行打印的打印机，如果为<see langword="null"/>，则使用默认打印机</param>
    /// <returns>一个用于等待打印完成的<see cref="Task"/></returns>
    Task PrintFromPage(Range? page = null, int number = 1, IPrinter? printer = null);
    #endregion
    #region 打印到文件
    /// <summary>
    /// 按照页码将这个Excel工作表，工作簿或Word文档打印到文件
    /// </summary>
    /// <param name="filePath">指定打印的目标文件路径，
    /// 函数会根据该路径的扩展名自动判断应使用哪个打印机</param>
    /// <inheritdoc cref="PrintFromPage(Range?, int, IPrinter?)"/>
    Task PrintFromPageToFile(PathText filePath, Range? page = null);
    #endregion
    #endregion 
}
