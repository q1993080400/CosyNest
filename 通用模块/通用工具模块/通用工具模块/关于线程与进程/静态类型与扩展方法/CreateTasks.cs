namespace System.Threading.Tasks
{
    /// <summary>
    /// 这个静态类可以用来创建和多线程有关的对象
    /// </summary>
    public static class CreateTasks
    {
        #region 创建ValueTask
        #region 通过委托返回ValueTask
        #region 返回ValueTask
        /// <summary>
        /// 返回一个<see cref="Tasks.ValueTask"/>，
        /// 它可以执行并等待一个委托
        /// </summary>
        /// <param name="delegate">待执行的委托</param>
        /// <returns></returns>
        public static ValueTask ValueTask(Action @delegate)
            => Task.Run(@delegate).ToValueTask();
        #endregion
        #region 返回ValueTask<TResult>
        /// <summary>
        /// 返回一个<see cref="Tasks.ValueTask{TResult}"/>，
        /// 它可以执行并等待一个委托
        /// </summary>
        /// <param name="delegate">待执行的委托</param>
        /// <returns></returns>
        public static ValueTask<TResult> ValueTask<TResult>(Func<TResult> @delegate)
            => Task.Run(@delegate).ToValueTask();
        #endregion
        #endregion
        #endregion
    }
}
