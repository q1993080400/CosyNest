using System.Collections.Concurrent;

namespace System;

/// <summary>
/// 该类型允许对对象执行自定义转换
/// </summary>
sealed class ConvertObject
{
    #region 静态类型
    /// <summary>
    /// 该字典注册特殊转换方法，它可以被<see cref="ExtendTool.To{Ret}(object?, bool, LazyPro{Ret}?)"/>所识别，
    /// 它的键是转换的源类型和目标类型，值是注册进去的特殊转换方法
    /// </summary>
    public static IAddOnlyDictionary<(Type From, Type To), ConvertObject> Conversion { get; }
    = new ConcurrentDictionary<(Type From, Type To), ConvertObject>().FitDictionary(false);
    #endregion
    #region 用于储存转换委托的集合
    /// <summary>
    /// 该集合枚举用于转换对象的委托
    /// </summary>
    private ConcurrentBag<Func<object, (bool IsSuccess, object Result)>> ConvertDelegate { get; } = new();
    #endregion
    #region 添加转换委托
    /// <summary>
    /// 添加用于转换的委托，然后返回这个对象本身
    /// </summary>
    /// <param name="convert">该委托的参数是待转换的对象，
    /// 返回值是一个元组，它的项分别是是否成功转换，以及转换结果</param>
    public ConvertObject Add(Func<object, (bool IsSuccess, object Result)> convert)
    {
        ConvertDelegate.Add(convert);
        return this;
    }
    #endregion
    #region 转换对象
    /// <summary>
    /// 尝试转换对象，并返回一个元组，
    /// 它的项分别是是否成功转换，以及转换结果
    /// </summary>
    /// <param name="obj">待转换的对象</param>
    /// <returns></returns>
    public (bool IsSuccess, object Result) TryConvert(object obj)
    {
        foreach (var item in ConvertDelegate)
        {
            var (isSuccess, result) = item(obj);
            if (isSuccess)
                return (true, result);
        }
        return default;
    }
    #endregion
    #region 静态构造函数
    static ConvertObject()
    {
        var parseDateTimeOffset = new ConvertObject().Add(static x =>
        {
            var isSuccess = DateTimeOffset.TryParse((string)x, out var result);
            return (isSuccess, result);
        });
        var parseBool = new ConvertObject().Add(static x =>
        {
            var text = (string)x;
            var isSuccess = bool.TryParse(text, out var result);
            return isSuccess ? (true, result) : text switch
            {
                "是" => (true, true),
                "否" => (true, false),
                _ => default
            };
        });
        Conversion.Add((typeof(string), typeof(DateTimeOffset)), parseDateTimeOffset);
        Conversion.Add((typeof(string), typeof(bool)), parseBool);
    }
    #endregion
}
