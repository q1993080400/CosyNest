namespace System.Underlying;

/// <summary>
/// 这个枚举表示命令行输出的类型
/// </summary>
public enum CommandLineOutputType
{
    /// <summary>
    /// 表示进程开始时的输出
    /// </summary>
    Star,
    /// <summary>
    /// 表示正常输出
    /// </summary>
    Standard,
    /// <summary>
    /// 表示发生了一个错误
    /// </summary>
    Error,
    /// <summary>
    /// 表示退出进程时产生的输出
    /// </summary>
    Exited
}
