namespace System.DataFrancis;

/// <summary>
/// 有关数据的工具类
/// </summary>
public static class ToolDataObj
{
    #region 显式初始化模块
    /// <summary>
    /// 显式执行模块初始化器，
    /// 初始化器通常隐式执行，但某些情况下可能需要显式控制它
    /// </summary>
    public static void InitializerModule()
    {
        CreateDataObj.DataEmpty();
    }
    #endregion
}
