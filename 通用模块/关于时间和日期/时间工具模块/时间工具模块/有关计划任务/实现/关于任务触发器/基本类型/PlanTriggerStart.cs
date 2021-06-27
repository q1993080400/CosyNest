namespace System.Time.Plan
{
    /// <summary>
    /// 表示在硬件启动时执行计划任务
    /// </summary>
    sealed class PlanTriggerStart : IPlanTriggerStart
    {
        #region 重写的ToString方法
        public override string ToString()
            => "此计划任务在硬件启动时执行";
        #endregion
    }
}
