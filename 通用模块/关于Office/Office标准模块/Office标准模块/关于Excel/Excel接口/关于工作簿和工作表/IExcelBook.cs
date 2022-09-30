using System.IOFrancis;

namespace System.Office.Excel;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以被视作一个Excel工作簿
/// </summary>
public interface IExcelBook : IFromIO
{
    #region 说明文档
    /*实现本接口请遵循以下规范：
      #在创建对象时，如果指定了路径，则自动从这个路径中加载工作簿，
      如果没有指定，则创建一个空白工作簿*/
    #endregion
    #region 关于工作簿
    #region 开启或关闭自动计算
    /// <summary>
    /// 如果这个值为<see langword="true"/>，则会启用自动计算，
    /// 将本属性设置为<see langword="false"/>可以改善性能
    /// </summary>
    bool AutoCalculation { get; set; }

    /*实现本API请遵循以下规范：
      #如果该Excel实现没有自动计算这个概念，
      事实上，恐怕也只有COM组件实现的工作簿有自动计算这种说法，
      则该属性读访问器直接返回false，写访问器不执行任何操作*/
    #endregion
    #region 删除工作簿
    /// <summary>
    /// 释放工作簿所占用的资源，并删除文件
    /// </summary>
    async ValueTask DeleteBook()
    {
        await DisposeAsync();
        if (IsExist)
            File.Delete(Path!);
    }
    #endregion
    #region 返回打印对象
    /// <summary>
    /// 返回Office打印对象，
    /// 它可以用来打印整个工作簿
    /// </summary>
    IOfficePrint Print { get; }
    #endregion
    #endregion
    #region 关于工作表
    #region 根据索引返回工作表
    /// <summary>
    /// 根据索引，返回工作表
    /// </summary>
    /// <param name="index">工作表的索引</param>
    /// <returns></returns>
    IExcelSheet this[Index index]
        => Sheets[index];
    #endregion
    #region 根据名称返回工作表
    #region 不可能返回null
    /// <summary>
    /// 根据工作表名，获取工作表
    /// </summary>
    /// <param name="name">工作表名称</param>
    /// <param name="createTable">当工作簿内不存在指定名称的工作表的时候，
    /// 如果这个值为<see langword="true"/>，则创建新表，否则抛出异常</param>
    /// <returns>具有指定名称的工作表，它不可能为<see langword="null"/></returns>
    IExcelSheet this[string name, bool createTable = false]
        => Sheets[name, createTable];
    #endregion
    #region 可能返回null
    /// <inheritdoc cref="IExcelSheetCollection.GetSheetOrNull(string)"/>
    IExcelSheet? GetSheetOrNull(string name)
        => Sheets.GetSheetOrNull(name);
    #endregion
    #endregion 
    #region 返回工作表的容器
    /// <summary>
    /// 返回工作表的容器，
    /// 它可以枚举，添加，删除工作簿中的工作表
    /// </summary>
    IExcelSheetCollection Sheets { get; }
    #endregion
    #endregion
    #region 根据绝对地址返回单元格
    /// <summary>
    /// 根据绝对地址返回单元格
    /// </summary>
    /// <param name="absA1Address">单元格所在的A1绝对地址，它必须包含工作表的名称</param>
    /// <returns></returns>
    IExcelCells ExcelCellsAbs(string absA1Address)
    {
        var separation = absA1Address.Split('!');
        return this[separation[0]][separation[1]];
    }
    #endregion
}
