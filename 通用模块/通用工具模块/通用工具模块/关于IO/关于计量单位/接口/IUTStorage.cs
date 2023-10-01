using System.MathFrancis;

namespace System.IOFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个计算机存储单位
/// </summary>
public interface IUTStorage : IUT
{
    #region 返回预设单位
    #region 返回比特
    /// <summary>
    /// 返回代表比特的单位，8比特等于一字节
    /// </summary>
    public static IUTStorage Bit { get; }
    = new UTStorage("比特", 1.0 / 8);
    #endregion
    #region 返回字节（公制单位）
    /// <summary>
    /// 获取一个表示字节的单位，这是计算机存储单位的公制单位
    /// </summary>
    public static IUTStorage ByteMetric { get; }
    = new UTStorage("字节", 1);
    #endregion
    #region 返回KB
    /// <summary>
    /// 返回代表KB的单位，1KB等于1024字节
    /// </summary>
    public static IUTStorage KB { get; }
    = new UTStorage("KB", 1024);
    #endregion
    #region 返回MB
    /// <summary>
    /// 返回代表MB的单位，1MB等于1024KB
    /// </summary>
    public static IUTStorage MB { get; }
    = new UTStorage("MB", 1024 * 1024);
    #endregion
    #region 返回GB
    /// <summary>
    /// 返回代表GB的单位，1GB等于1024MB   
    /// </summary>
    public static IUTStorage GB { get; }
    = new UTStorage("GB", Math.Pow(1024, 3));
    #endregion
    #region 返回TB
    /// <summary>
    /// 返回代表TB的单位，1TB等于1024GB
    /// </summary>
    public static IUTStorage TB { get; }
    = new UTStorage("TB", Math.Pow(1024, 4));
    #endregion
    #endregion
    #region 创建单位
    /// <inheritdoc cref="IUTLength.Create(string, Num)"/>
    public static IUTStorage Create(string name, Num size)
        => new UTStorage(name, size);
    #endregion
}
