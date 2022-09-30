namespace System.Office.Excel;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个Excel工作表集合，
/// 它可以枚举，添加，删除工作簿中的工作表
/// </summary>
public interface IExcelSheetCollection : IList<IExcelSheet>
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
        => GetSheetOrNull(name) ??
        (createTable ? Add(name) : throw new KeyNotFoundException($"工作簿中不存在名为{name}的工作表"));
    #endregion
    #region 可能返回null
    /// <returns>具有指定名称的工作表，如果没有找到，则返回<see langword="null"/></returns>
    /// <inheritdoc cref="this[string, bool]"/>
    IExcelSheet? GetSheetOrNull(string name)
        => this.FirstOrDefault(x => x.Name == name);
    #endregion
    #endregion
    #region 添加工作表
    #region 添加空白表
    /// <summary>
    /// 添加一个具有指定名称的空白工作表，
    /// 并将其放在工作表集合的末尾
    /// </summary>
    /// <param name="name">工作表名，如果它已经重复，则自动将其重命名</param>
    /// <returns>新添加的空白工作表</returns>
    IExcelSheet Add(string name = "Sheet");
    #endregion
    #region 复制末尾工作表，并添加
    /// <summary>
    /// 复制最末尾的一个工作表，
    /// 并将新表重命名后放在集合的末尾
    /// </summary>
    /// <param name="renamed">一个用于修改工作表名，使其不重名的委托，
    /// 它的第一个参数是旧名称，第二个参数是尝试失败的次数，从2开始，返回值就是新的名称，
    /// 如果为<see langword="null"/>，则使用一个默认方法</param>
    /// <returns></returns>
    IExcelSheet CopyLast(Func<string, int, string>? renamed = null)
        => this.Last().Copy(renamed, this);
    #endregion
    #endregion
    #region 删除工作表
    #region 根据名称
    /// <summary>
    /// 删除具有指定名称的工作表
    /// </summary>
    /// <param name="name">指定的名称</param>
    /// <returns>如果删除成功，则为<see langword="true"/>，否则为<see langword="false"/></returns>
    bool Remove(string name)
    {
        var sheet = this.FirstOrDefault(x => x.Name == name);
        if (sheet is null)
            return false;
        sheet.Delete();
        return true;
    }
    #endregion
    #endregion
}
