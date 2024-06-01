using System.Design;

namespace System.Underlying;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个命令行上下文，
/// 释放它的时候可以退出命令行
/// </summary>
public interface ICommandLineContext : IAsyncDisposable, IInstruct
{
    #region 返回一个枚举所有输出的异步枚举器
    /// <summary>
    /// 返回一个枚举所有命令行输出的异步枚举器
    /// </summary>
    IAsyncEnumerable<CommandLineOutput> Output { get; }
    #endregion
}
