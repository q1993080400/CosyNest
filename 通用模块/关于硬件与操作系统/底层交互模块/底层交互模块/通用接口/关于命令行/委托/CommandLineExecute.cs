namespace System.Underlying;

/// <summary>
/// 启动命令行，并返回命令行的上下文
/// </summary>
/// <param name="script">命令行要执行的脚本</param>
/// <param name="cancellationToken">一个用来取消异步操作的令牌</param>
/// <returns></returns>
public delegate ICommandLineContext CommandLineExecute(string script, CancellationToken cancellationToken = default);
