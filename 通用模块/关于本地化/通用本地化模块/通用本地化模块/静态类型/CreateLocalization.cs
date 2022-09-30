namespace System.Localization;

/// <summary>
/// 这个静态类可以用来创建和本地化有关的对象
/// </summary>
public static class CreateLocalization
{
    #region 获取一个根据拼音排序的比较器
    /// <summary>
    /// 获取一个复杂的比较器，它专门为中文优化，
    /// 按照拼音，以及文本中出现的编号进行排序
    /// </summary>
    public static IComparer<string> ComparableStringChinese { get; }
        = FastRealize.ComparerFromNumbering(new ComparableStringChinese());
    #endregion
}
