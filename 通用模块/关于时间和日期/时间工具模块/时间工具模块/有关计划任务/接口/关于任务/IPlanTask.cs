namespace System.Time.Plan
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个计划任务，
    /// 它可以在指定的时间启动进程
    /// </summary>
    public interface IPlanTask : IPlanTaskInfo
    {
        #region 删除计划任务
        /// <summary>
        /// 终止这个计划任务，并将其删除
        /// </summary>
        void Delete();
        #endregion
    }
}
