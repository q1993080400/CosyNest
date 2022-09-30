namespace System.Underlying;

/// <summary>
/// 凡是实现这个接口的类型，都可以视为一个启动管理面板，
/// 它可以用来管理应用自启动
/// </summary>
public interface IStartManagement
{
    #region 获取是否自动启动
    /// <summary>
    /// 获取或设置是否自动启动本应用
    /// </summary>
    bool IsAutomaticStart
    {
        get => AutomaticStart.Contains(Environment.ProcessPath!);
        set
        {
            var path = Environment.ProcessPath!;
            if (value)
                AutomaticStart.Add(path);
            else AutomaticStart.Remove(path);
        }
    }
    #endregion
    #region 枚举或添加自启动项
    /// <summary>
    /// 该集合能够根据路径，枚举，添加，删除当前自启动项
    /// </summary>
    ICollection<string> AutomaticStart { get; }
    #endregion
}
