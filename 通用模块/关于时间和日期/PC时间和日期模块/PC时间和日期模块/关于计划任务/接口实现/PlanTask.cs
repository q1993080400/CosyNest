using System.IOFrancis.FileSystem;

using TaskScheduler;

namespace System.TimeFrancis.Plan;

/// <summary>
/// 这个类型是借助COM实现的计划任务
/// </summary>
class PlanTask : IPlanTask
{
    #region 封装的对象
    #region 计划任务
    /// <summary>
    /// 获取被封装的计划任务，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    private IRegisteredTask PackTask { get; }
    #endregion
    #region 对计划任务的定义
    /// <summary>
    /// 获取对计划任务的定义
    /// </summary>
    private ITaskDefinition PackDefinition
        => PackTask.Definition;
    #endregion
    #endregion
    #region 删除计划任务
    public void Delete()
        => PlanPanel.OnlyObj.PacklFolder.DeleteTask(Name, 0);
    #endregion
    #region 计划任务的定义
    #region 计划任务的名称
    public string Name
         => PackTask.Name;
    #endregion
    #region 对计划任务的描述
    public string? Describe
        => PackDefinition.RegistrationInfo.Description;
    #endregion
    #region 计划任务的触发器
    public IEnumerable<IPlanTrigger> Triggers
         => PackDefinition.Triggers.OfType<ITrigger>().
        Select(PlanRealize.ToTrigger).
        Where(x => x is { })!;
    #endregion
    #region 是否允许任务唤醒硬件
    public bool CanAwaken
        => PackDefinition.Settings.WakeToRun;
    #endregion
    #region 执行计划任务时的操作
    public IEnumerable<(PathText Path, string? Parameters)> Start
         => PackDefinition.Actions.OfType<IExecAction>().
        Select(x => ((PathText)x.Path, x.Arguments))!;
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 将指定的计划任务对象封装进对象中
    /// </summary>
    /// <param name="packTask">被封装的计划任务对象</param>
    public PlanTask(IRegisteredTask packTask)
    {
        this.PackTask = packTask;
    }
    #endregion
}
