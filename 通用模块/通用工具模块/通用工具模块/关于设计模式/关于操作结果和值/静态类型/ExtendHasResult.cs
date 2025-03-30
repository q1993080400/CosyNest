namespace System;

public static partial class ExtendDesign
{
    //这个静态类专门用来声明有关操作结果和值的方法

    #region 解构对象
    /// <summary>
    /// 将<see cref="IHasResultValue{Return}"/>解构为操作是否成功，以及操作的值
    /// </summary>
    /// <param name="hasResultValue">要解构的对象</param>
    /// <param name="success">用来接收操作是否成功的变量</param>
    /// <param name="value">用来接收操作的值的变量</param>
    /// <inheritdoc cref="IHasResultValue{Return}"/>
    public static void Deconstruct<Return>(this IHasResultValue<Return>? hasResultValue, out bool success, out Return? value)
    {
        if (hasResultValue is null)
        {
            success = false;
            value = default;
            return;
        }
        success = hasResultValue.Success;
        value = hasResultValue.Value;
    }
    #endregion
    #region 获取值，不可为null
    /// <summary>
    /// 获取返回值，如果操作不成功，或者值为<see langword="null"/>，
    /// 则引发一个异常
    /// </summary>
    /// <param name="hasResultValue">要获取返回值的对象</param>
    /// <returns></returns>
    /// <inheritdoc cref="IHasResultValue{Return}"/>
    public static Return GetValue<Return>(this IHasResultValue<Return>? hasResultValue)
        => hasResultValue is { Success: true, Value: { } value } ?
        value :
        throw new NullReferenceException($"操作不成功，或者返回值为null，请先检查{nameof(IHasResultValue<>.Success)}属性，再获取值");
    #endregion
}
