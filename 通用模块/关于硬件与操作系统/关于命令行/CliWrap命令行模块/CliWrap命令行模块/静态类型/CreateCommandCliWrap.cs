namespace System.Underlying;

/// <summary>
/// 这个静态类可以用来创建有关Wrap实现的命令行的对象
/// </summary>
public static class CreateCommandCliWrap
{
    #region 创建命令行执行委托
    /// <summary>
    /// 创建一个用来执行命令行的委托
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="CommandLineContextCliWrap(string, string, CancellationToken)"/>
    public static CommandLineExecute CommandLineExecute(string filePath = "powershell.exe")
        => (script, cancellationToken) => new CommandLineContextCliWrap(filePath, script, cancellationToken);
    #endregion
}
