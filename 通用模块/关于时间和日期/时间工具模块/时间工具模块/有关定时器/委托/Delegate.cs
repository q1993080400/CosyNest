namespace System.Time
{
    #region 说明文档
    /*问：该委托与IPlanTrigger有何区别？
      答：本委托假设执行定时器的时候，.net进程仍然处于运行状态，
      所以能够更灵活的确定时间，但是前提条件是不能退出程序，
      而IPlanTrigger没有这个限制，它可以将触发条件告知给操作系统或其他进程，
      因此可以在程序关闭的时候继续触发计划任务，但代价是计算触发时间没有那么灵活
      为了保证兼容，IPlanTriggerTiming也有一个NextDate方法，
      它的签名和本委托相同，可以直接赋值给这个委托*/
    #endregion
    #region 获取下一次执行时间的委托
    /// <summary>
    /// 获取从某个时间点开始，下一次执行定时器的时间
    /// </summary>
    /// <param name="date">当前时间，如果为<see langword="null"/>，
    /// 默认为<see cref="DateTimeOffset.MinValue"/>，等同于计算第一次执行定时器的时间</param>
    /// <returns>下一次执行定时器的时间，如果为<see langword="null"/>，表示定时器已终止</returns>
    public delegate DateTimeOffset? NextDate(DateTimeOffset? date = null);
    #endregion 
}
