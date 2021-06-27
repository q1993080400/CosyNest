using System.Collections.Generic;
using System.Linq;

namespace System
{
    /// <summary>
    /// 这个静态类提供一些方法，
    /// 可以直接使用委托实现一些比较简单的接口
    /// </summary>
    public static class FastRealize
    {
        #region 实现IEqualityComparer
        #region 复杂实现
        /// <summary>
        /// 通过委托实现<see cref="IEqualityComparer{Obj}"/>
        /// </summary>
        /// <typeparam name="Obj">要比较的对象类型</typeparam>
        /// <param name="equals">用于比较对象的委托</param>
        /// <param name="getHash">用于计算哈希值的委托</param>
        /// <returns></returns>
        public static IEqualityComparer<Obj> EqualityComparer<Obj>(Func<Obj, Obj, bool> equals, Func<Obj, int> getHash)
            => new EqualityComparer<Obj>(equals, getHash);
        #endregion
        #region 简单实现
        /// <summary>
        /// 通过委托实现<see cref="IEqualityComparer{Obj}"/>，
        /// 它从<typeparamref name="Obj"/>对象中提取键，
        /// 然后逐个比较它们，作为判断相等的标准
        /// </summary>
        /// <typeparam name="Obj">要比较的对象类型</typeparam>
        /// <param name="getKey">从<typeparamref name="Obj"/>对象中提取键的委托</param>
        /// <returns></returns>
        public static IEqualityComparer<Obj> EqualityComparer<Obj>(params Func<Obj, object>[] getKey)
        {
            getKey.AnyCheck("用来获取键的委托");
            return EqualityComparer<Obj>(
                (x, y) => getKey.All(del => Equals(del(x), del(y))),
                x => ToolEqual.CreateHash(getKey.Select(del => del(x)).ToArray()));
        }
        #endregion
        #endregion
        #region 实现IDisposable
        /// <summary>
        /// 获取一个实现<see cref="IDisposable"/>的锁，
        /// 它可以利用using语句自动完成某些操作，以免遗漏
        /// </summary>
        /// <param name="initialization">在锁初始化的时候，这个委托会被立即执行，
        /// 如果为<see langword="null"/>，则会被忽略</param>
        /// <param name="dispose">在锁被释放的时候，这个委托会被执行</param>
        /// <returns></returns>
        public static IDisposable Disposable(Action? initialization, Action dispose)
            => new Lock(initialization, dispose);
        #endregion
    }
}
