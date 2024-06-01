using System.Design;

using CliWrap;
using CliWrap.EventStream;

namespace System.Underlying;

/// <summary>
/// 本类型是底层使用CliWrap实现的命令行上下文
/// </summary>
sealed class CommandLineContextCliWrap : ReleaseAsync, ICommandLineContext
{
    #region 接口实现
    #region 返回枚举输出的枚举器
    public IAsyncEnumerable<CommandLineOutput> Output
    {
        get
        {
            #region 本地函数
            async IAsyncEnumerable<CommandLineOutput> Fun()
            {
                await using var enumerator = OutputCommand.GetAsyncEnumerator();
                while (true)
                {
                    var hasNext = false;
                    try
                    {
                        hasNext = await enumerator.MoveNextAsync();
                    }
                    catch (OperationCanceledException)
                    {
                        yield break;
                    }
                    if (!hasNext)
                        yield break;

                    CommandLineOutput commandLineOutput = enumerator.Current switch
                    {
                        StartedCommandEvent command => new()
                        {
                            Text = $"命令行启动，进程ID是{command.ProcessId}",
                            Type = CommandLineOutputType.Star
                        },
                        StandardOutputCommandEvent command => new()
                        {
                            Text = command.Text.Trim(),
                            Type = CommandLineOutputType.Standard
                        },
                        StandardErrorCommandEvent command => new()
                        {
                            Text = command.Text.Trim(),
                            Type = CommandLineOutputType.Error
                        },
                        ExitedCommandEvent command => new()
                        {
                            Text = $"命令行已退出，代码是{command.ExitCode}",
                            Type = CommandLineOutputType.Exited
                        },
                        var command => throw new NotSupportedException($"无法识别{command.GetType()}类型的命令行输出")
                    };
                    if (!commandLineOutput.Text.IsVoid())
                        yield return commandLineOutput;
                }
            }
            #endregion
            return Fun();
        }
    }
    #endregion
    #endregion
    #region 抽象类实现
    #region 释放对象
    protected override async ValueTask DisposeAsyncRealize()
    {
        await CancellationTokenSource.CancelAsync();
        CancellationTokenSource.Dispose();
    }
    #endregion
    #endregion
    #region 内部成员
    #region 取消源对象
    /// <summary>
    /// 这个对象可以用来取消命令行输出
    /// </summary>
    private CancellationTokenSource CancellationTokenSource { get; }
    #endregion
    #region 命令行输出对象
    /// <summary>
    /// 获取封装的命令行输出对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private IAsyncEnumerable<CommandEvent> OutputCommand { get; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="filePath">要启动命令行的文件路径</param>
    /// <param name="script">命令行要执行的脚本</param>
    /// <param name="cancellationToken">一个用来取消异步操作的令牌</param>
    public CommandLineContextCliWrap(string filePath, string script, CancellationToken cancellationToken)
    {
        CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        OutputCommand = Cli.Wrap(filePath).WithArguments(script).ListenAsync(CancellationTokenSource.Token);
    }
    #endregion
}
