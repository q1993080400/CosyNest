namespace System;

/// <summary>
/// 表示由于类型非法所引发的异常
/// </summary>
/// <remarks>
/// 用指定的非法和合法类型初始化异常
/// </remarks>
/// <param name="unlawfulType">引发异常的非法类型，
/// 如果它是<see cref="Type"/>，则返回它本身，否则调用<see cref="object.GetType"/>方法获取它的类型</param>
/// <param name="legalType">受支持的合法类型，可以为多个，
/// 它的元素会通过与上一个参数相同的方法转换为<see cref="Type"/></param>
public sealed class TypeUnlawfulException(object? unlawfulType, params object[] legalType) : InvalidCastException(new[] { "错误原因：类型非法，且无法转换为合法类型" ,
            $"引发异常的非法类型：{(object?)unlawfulType?.GetTypeObj() ?? "null"}",
            $"受支持的合法类型：{legalType.Join(x => x.GetTypeObj().ToString(), "，")}"}.
              Join(Environment.NewLine))
{
    #region 检查类型的合法性
    /// <summary>
    /// 检查一个对象，如果它不是合法类型，则抛出一个异常
    /// </summary>
    /// <param name="check">待检查的类型，如果它不是<see cref="Type"/>，则会获取它的类型</param>
    /// <param name="legal">如果<paramref name="check"/>不能赋值给这个集合中的任何一个类型，则会引发异常</param>
    public static void Check(object check, params object[] legal)
    {
        var type = check.GetTypeObj();
        if (legal.Select(x => x.GetTypeObj()).
            All(x => !x.IsAssignableFrom(type)))
        {
            throw new TypeUnlawfulException(check, legal);
        }
    }
    #endregion
    #region 非法类型
    /// <summary>
    /// 获取引发异常的非法类型
    /// </summary>
    public Type UnlawfulType { get; } = unlawfulType?.GetTypeObj() ?? typeof(object);
    #endregion
    #region 合法类型
    /// <summary>
    /// 获取受支持的合法类型
    /// </summary>
    public IEnumerable<Type> LegalType { get; } = legalType.Select(x => x.GetTypeObj());

    #endregion
    #region 构造方法
    #endregion
}
