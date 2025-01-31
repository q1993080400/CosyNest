namespace System.MathFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以提供一个不断递增的对象，注意：
/// 它不一定是线程安全的，请不要将它设计为单例模式
/// </summary>
public interface IIncremental<out Obj>
{
    #region 获取递增的时间
    /// <summary>
    /// 获取一个<typeparamref name="Obj"/>，
    /// 每次调用它的时候，返回值都会递增
    /// </summary>
    /// <returns></returns>
    Obj Incremental();
    #endregion
}
