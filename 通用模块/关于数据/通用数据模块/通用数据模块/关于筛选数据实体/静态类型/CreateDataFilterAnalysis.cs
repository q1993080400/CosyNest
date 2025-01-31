namespace System.DataFrancis;

public static partial class CreateDataObj
{
    //这个部分类专门用来声明有关创建筛选数据实体的对象的方法

    #region 创建表达式解析器
    /// <summary>
    /// 返回一个<see cref="IDataFilterAnalysis"/>的默认实现，
    /// 它可以将<see cref="DataFilterDescription"/>解析为表达式树
    /// </summary>
    public static IDataFilterAnalysis DataFilterAnalysis { get; }
        = new DataFilterAnalysisDefault();
    #endregion
}
