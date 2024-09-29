namespace System.TimeFrancis.Plan;

/// <summary>
/// 这个类型是<see cref="IPlanTaskInfo"/>的实现，
/// 封装了注册计划任务所需要的信息
/// </summary>
sealed record PlanTaskInfo : IPlanTaskInfo
{
    #region 计划任务的名称
    public string Name { get; }
    #endregion
    #region 对计划任务的描述
    public string? Describe { get; init; }
    #endregion
    #region 计划任务的触发器
    IEnumerable<IPlanTrigger> IPlanTaskInfo.Triggers
       => Triggers;

    /// <summary>
    /// 获取计划任务的所有触发器
    /// </summary>
    public IList<IPlanTrigger> Triggers { get; }
    #endregion
    #region 是否允许任务唤醒硬件
    public bool CanAwaken { get; }
    #endregion
    #region 执行计划任务时的操作
    IEnumerable<(string Path, string? Parameters)> IPlanTaskInfo.Start
        => Start;

    /// <summary>
    /// 这个集合枚举执行计划任务时，
    /// 要启动的进程路径，以及传递给进程的参数
    /// </summary>
    public IList<(string Path, string? Parameters)> Start { get; }
    #endregion
    #region 构造函数
    #region 直接创建
    /// <summary>
    /// 使用指定的名称，触发器和启动操作初始化对象
    /// </summary>
    /// <param name="name">计划任务的名称</param>
    /// <param name="triggers">计划任务的触发器</param>
    /// <param name="start">这个集合枚举执行计划任务时，
    /// 要启动的进程路径，以及传递给进程的参数</param>
    public PlanTaskInfo(string name, IEnumerable<IPlanTrigger> triggers, IEnumerable<(string Path, string? Parameters)> start)
    {
        Name = name;
        Triggers = triggers.ToList();
        Start = start.ToList();
    }
    #endregion
    #region 直接创建，且仅有一个触发器和启动操作
    /// <summary>
    /// 使用指定的名称，触发器和启动操作初始化对象
    /// </summary>
    /// <param name="name">计划任务的名称</param>
    /// <param name="trigger">计划任务的触发器</param>
    /// <param name="startPath">执行计划任务时要启动的进程路径</param>
    /// <param name="startParameters">执行计划任务时传递给待启动进程的参数</param>
    public PlanTaskInfo(string name, IPlanTrigger trigger, string startPath, string? startParameters = null)
        : this(name, [trigger], [(startPath, startParameters)])
    {

    }
    #endregion
    #endregion
}
