using System.Collections.Generic;
using System.IOFrancis.FileSystem;

namespace System.Time.Plan
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以作为一个计划任务的配置选项
    /// </summary>
    public interface IPlanTaskInfo
    {
        #region 计划任务的名称
        /// <summary>
        /// 获取计划任务的名称
        /// </summary>
        string Name { get; }
        #endregion
        #region 对计划任务的描述
        /// <summary>
        /// 对计划任务的描述，
        /// 如果为<see langword="null"/>，代表没有描述
        /// </summary>
        string? Describe { get; }
        #endregion
        #region 计划任务的触发器
        /// <summary>
        /// 获取计划任务的所有触发器
        /// </summary>
        IEnumerable<IPlanTrigger> Triggers { get; }
        #endregion
        #region 是否允许任务唤醒硬件
        /// <summary>
        /// 如果执行任务时硬件正好处于休眠状态，
        /// 这个属性为<see langword="true"/>代表允许唤醒硬件，否则代表不允许
        /// </summary>
        bool CanAwaken { get; }
        #endregion
        #region 执行计划任务时的操作
        /// <summary>
        /// 这个集合枚举执行计划任务时，
        /// 要启动的进程路径，以及传递给进程的参数
        /// </summary>
        IEnumerable<(PathText Path, string? Parameters)> Start { get; }
        #endregion
    }
}
