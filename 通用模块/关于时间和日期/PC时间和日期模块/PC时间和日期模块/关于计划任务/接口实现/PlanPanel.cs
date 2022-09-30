
using TaskScheduler;

namespace System.TimeFrancis.Plan;

/// <summary>
/// 这个类型是借助COM实现的计划任务面板，
/// 使用此类型需请求管理员权限
/// </summary>
public class PlanPanel : IPlanPanel
{
    #region 封装的对象
    #region 计划任务面板
    /// <summary>
    /// 获取被封装的计划任务面板，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    internal ITaskService PackPanel { get; }
    #endregion
    #region 计划任务文件夹
    /// <summary>
    /// 获取被封装的计划任务文件夹
    /// </summary>
    internal ITaskFolder PacklFolder { get; }
    #endregion
    #endregion
    #region 枚举所有计划任务
    public IEnumerable<IPlanTask> Tasks
         => PacklFolder.GetTasks(1).
        OfType<IRegisteredTask>().
        Select(x => new PlanTask(x)).
        Where(x => x.Triggers.Any() && x.Start.Any());
    #endregion
    #region 创建计划任务
    public IPlanTask CreatePlan(IPlanTaskInfo info)
    {
        PacklFolder.DeleteTask(info.Name, 0);
        var task = PackPanel.NewTask(0);
        task.RegistrationInfo.Description = info.Describe;
        foreach (var (path, parameters) in info.Start)
        {
            var actions = (IExecAction)task.Actions.Create(_TASK_ACTION_TYPE.TASK_ACTION_EXEC);
            actions.Arguments = parameters;
            actions.Path = path;
        }
        info.Triggers.ForEach(x => PlanRealize.CreateTrigger(task.Triggers, x));
        task.Settings.WakeToRun = info.CanAwaken;
        var regTask = PacklFolder.RegisterTaskDefinition(info.Name, task, (int)_TASK_CREATION.TASK_CREATE,
            null, null, _TASK_LOGON_TYPE.TASK_LOGON_INTERACTIVE_TOKEN, "");
        regTask.Run(null);
        return new PlanTask(regTask);
    }
    #endregion
    #region 关于构造函数与创建对象
    #region 返回本类型对象的唯一实例（实现形式）
    /// <summary>
    /// 返回本类型对象的唯一一个实例
    /// </summary>
    internal static PlanPanel OnlyObj { get; }
    = new PlanPanel();
    #endregion
    #region 返回本类型对象的唯一实例（接口形式）
    /// <summary>
    /// 返回本类型对象的唯一一个实例
    /// </summary>
    public static IPlanPanel Only
        => OnlyObj;
    #endregion
    #region 无参数构造函数
    /// <summary>
    /// 无参数构造函数，禁止外部调用
    /// </summary>
    private PlanPanel()
    {
        PackPanel = new TaskScheduler.TaskScheduler();
        PackPanel.Connect();
        PacklFolder = PackPanel.GetFolder("\\");
    }
    #endregion
    #endregion
}
