namespace System.Underlying.PC;

/// <summary>
/// 这个类型是<see cref="IStartManagement"/>的实现，
/// 可以用来管理应用自动启动
/// </summary>
sealed class StartManagement : IStartManagement
{
    #region 枚举或添加自启动项
    public ICollection<string> AutomaticStart { get; } = new StartCollections();
    #endregion 
}
