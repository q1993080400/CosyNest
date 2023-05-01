using System.Diagnostics.CodeAnalysis;
using System.Performance;

namespace System.IOFrancis.FileSystem;

/// <summary>
/// 表示一个专用于文件路径的文本，
/// 会进行一些检查并剔除掉非法字符，
/// 在编写任何与路径有关的方法时，
/// 请尽量以本类型为参数
/// </summary>
public sealed record PathText
{
    #region 说明文档
    /*需要本类型的原因在于：
      从属性，安全选项卡中复制的路径，
      因为不明原因，第一个字符是一个隐形的空字符（字符编码8234），
      这个字符时常造成路径无效，需要被清除掉，
      但是如果每次编写一个新方法都需要进行一次判断，会非常麻烦，
      因此编写了本类型，自带验证字符有效性的功能*/
    #endregion
    #region 静态部分
    #region 隐式类型转换
    [return: NotNullIfNotNull(nameof(a))]
    public static implicit operator string?(PathText? a)
        => a?.Path;

    [return: NotNullIfNotNull(nameof(a))]
    public static implicit operator PathText?(string? a)
        => a is null ? null : new(a);
    #endregion
    #region 重载+号
    /// <summary>
    /// 将两个路径文本的路径拼接起来，
    /// 注意，它是使用<see cref="Path.Combine(string, string)"/>进行拼接，
    /// 所以当两个参数都是，或第二个参数是绝对路径时，不要进行这个操作
    /// </summary>
    /// <param name="a">要拼接的第一个路径文本</param>
    /// <param name="b">要拼接的第二个路径文本</param>
    /// <returns></returns>
    public static PathText operator +(PathText a, PathText b)
        => IO.Path.Combine(a, b);
    #endregion
    #region 缓存字典
    /// <summary>
    /// 这个字典缓存Trim后的路径文本，
    /// 以避免不必要的性能损失
    /// </summary>
    private static ICache<string, string> Cache { get; }
    = CreatePerformance.CacheThreshold(x =>
    {
        x = ToolPath.Trim(x);
        IOExceptionFrancis.CheckNotLegal(x);
        return x;
    }, 150, Cache);
    #endregion
    #endregion
    #region 路径文本
    /// <summary>
    /// 封装的路径文本，已经检查有效性，
    /// 并剔除掉非法字符
    /// </summary>
    public string Path { get; }
    #endregion
    #region 重写ToString
    public override string ToString()
        => Path ?? "";
    #endregion
    #region 构造方法
    /// <summary>
    /// 将指定的路径文本封装进对象中
    /// </summary>
    /// <param name="path">指定的路径文本</param>
    public PathText(string path)
    {
        Path = path.IsVoid() ?
               throw new ArgumentException("路径文本不能为null或空字符串") :
               Cache[path];
    }
    #endregion
}
