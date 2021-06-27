namespace System
{
    /// <summary>
    /// 有关异常的工具类
    /// </summary>
    public static class ToolException
    {
        #region 忽略指定的异常
        #region 无返回值
        /// <summary>
        /// 执行代码块，并忽略掉指定类型的异常
        /// </summary>
        /// <typeparam name="Exception">要忽略的异常类型</typeparam>
        /// <param name="try">在try代码块中执行的委托</param>
        /// <param name="catch">在catch代码块中执行的委托，
        /// 如果为<see langword="null"/>，则不会执行</param>
        /// <param name="finally">在finally代码块中执行的委托，
        /// 如果为<see langword="null"/>，则不会执行</param>
        /// <returns>如果成功执行，未出现异常，则返回<see langword="true"/>，否则返回<see langword="false"/></returns>
        public static bool Ignore<Exception>(Action @try, Action? @catch = null, Action? @finally = null)
            where Exception : System.Exception
        {
            try
            {
                @try();
                return true;
            }
            catch (Exception)
            {
                @catch?.Invoke();
                return false;
            }
            finally
            {
                @finally?.Invoke();
            }
        }
        #endregion
        #region 有返回值
        /// <summary>
        /// 执行代码块，并忽略掉指定类型的异常，并返回返回值
        /// </summary>
        /// <typeparam name="Exception">要忽略的异常类型</typeparam>
        /// <typeparam name="Ret">返回值类型</typeparam>
        /// <param name="try">在try代码块中执行的委托</param>
        /// <param name="catch">当执行<paramref name="try"/>出现异常时，通过这个延迟对象获取返回值</param>
        /// <param name="finally">在finally代码块中执行的委托，
        /// 如果为<see langword="null"/>，则不会执行</param>
        /// <returns>一个元组，它的第一个项是是否执行成功，第二个项通过委托获取的返回值</returns>
        public static (bool IsSuccess, Ret? Return) Ignore<Exception, Ret>(Func<Ret?> @try, LazyPro<Ret>? @catch = null, Action? @finally = null)
            where Exception : System.Exception
        {
            try
            {
                return (true, @try());
            }
            catch (Exception)
            {
                return (false, @catch);
            }
            finally
            {
                @finally?.Invoke();
            }
        }
        #endregion
        #endregion
    }
}
