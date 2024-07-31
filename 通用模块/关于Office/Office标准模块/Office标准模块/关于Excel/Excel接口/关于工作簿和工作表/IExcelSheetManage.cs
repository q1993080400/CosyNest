namespace System.Office.Excel;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来管理工作簿中的工作表
/// </summary>
public interface IExcelSheetManage : IReadOnlyList<IExcelSheet>
{
    #region 说明文档
    /*实现本接口请遵循以下规范：
      #在实现Clear方法时，如果底层Excel引擎允许工作簿中没有工作表，
      则删除全部工作表，否则执行重置操作，也就是将工作簿恢复到仅有一个空白工作表的初始状态*/
    #endregion
    #region 返回工作簿
    /// <summary>
    /// 返回这个容器所在的工作簿
    /// </summary>
    IExcelBook Book { get; }
    #endregion
    #region 关于返回工作表
    #region 为null时引发异常
    /// <summary>
    /// 根据工作表名，返回工作表，
    /// 如果该工作表不存在，则引发异常
    /// </summary>
    /// <param name="name">工作表名称</param>
    /// <returns></returns>
    IExcelSheet this[string name]
        => GetSheetOrNull(name) ??
        throw new KeyNotFoundException($"没有找到名称为{name}的工作表");
    #endregion
    #region 不可能返回null
    /// <summary>
    /// 根据工作表名，获取工作表
    /// </summary>
    /// <param name="createTable">当工作簿内不存在指定名称的工作表的时候，
    /// 如果这个值为<see langword="true"/>，则创建新表，否则抛出异常</param>
    /// <returns>具有指定名称的工作表，它不可能为<see langword="null"/></returns>
    /// <inheritdoc cref="this[string]"/>
    IExcelSheet this[string name, bool createTable]
    {
        get
        {
            var sheet = GetSheetOrNull(name);
            return (sheet, createTable) switch
            {
                ({ } s, _) => s,
                (null, true) => Add(name),
                (null, false) => throw new KeyNotFoundException($"没有找到名称为{name}的工作表")
            };
        }
    }
    #endregion
    #region 可能返回null
    /// <returns>具有指定名称的工作表，如果没有找到，则返回<see langword="null"/></returns>
    /// <inheritdoc cref="this[string, bool]"/>
    IExcelSheet? GetSheetOrNull(string name);
    #endregion
    #endregion
    #region 关于添加工作表
    #region 添加空白表
    /// <summary>
    /// 添加一个具有指定名称的空白工作表
    /// </summary>
    /// <param name="name">工作表名，如果它已经重复，则自动将其重命名</param>
    /// <param name="pos">新添加的工作表所在的位置，
    /// 如果为<see langword="null"/>，表示位于最后</param>
    /// <returns>新添加的空白工作表</returns>
    IExcelSheet Add(string name = "Sheet", Index? pos = null);
    #endregion
    #region 插入非空白表
    /// <summary>
    /// 将一个工作表复制到集合中
    /// </summary>
    /// <param name="sheet">要复制过来的工作表</param>
    /// <returns>新复制过来的工作表</returns>
    /// <inheritdoc cref="Add(string, Index?)"/>
    IExcelSheet Add(IExcelSheet sheet, Index? pos = null);
    #endregion
    #endregion
}
