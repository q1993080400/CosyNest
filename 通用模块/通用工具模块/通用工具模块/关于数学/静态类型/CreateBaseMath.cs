namespace System.MathFrancis;

/// <summary>
/// 这个静态类可以用来帮助创建基本数学对象
/// </summary>
public static partial class CreateBaseMath
{
    #region 创建IRandom
    #region 获取公用的随机数生成器
    /// <summary>
    /// 获取一个公用的随机数生成器，
    /// 它是线程安全的
    /// </summary>
    public static IRandom RandomShared
        => new Random(System.Random.Shared);
    #endregion
    #region 使用指定的种子创建
    /// <summary>
    /// 使用指定的种子创建一个随机数生成器
    /// </summary>
    /// <param name="seed">用来生成随机数的种子，
    /// 如果为<see langword="null"/>，则默认使用时间作为种子</param>
    /// <returns></returns>
    public static IRandom Random(int? seed = null)
        => new Random(seed is { } s ? new System.Random(s) : new());
    #endregion
    #endregion
}
