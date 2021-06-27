using System.Collections.Generic;

namespace System
{
    /// <summary>
    /// 这个类型可以使用委托来进行对象的相等比较
    /// </summary>
    /// <typeparam name="Obj">进行相等比较的对象类型</typeparam>
    class EqualityComparer<Obj> : IEqualityComparer<Obj>
    {
        #region 封装的委托
        #region 进行相等比较的委托
        /// <summary>
        /// 在进行相等比较时，实际执行这个委托
        /// </summary>
        private Func<Obj?, Obj?, bool> EqualsDelegate { get; }
        #endregion
        #region 计算哈希值的委托
        /// <summary>
        /// 在计算哈希值的时候，实际执行这个委托
        /// </summary>
        private Func<Obj, int> GetHashCodeDelegate { get; }
        #endregion
        #endregion
        #region 接口实现
        public bool Equals(Obj? x, Obj? y)
            => EqualsDelegate(x, y);
        public int GetHashCode(Obj obj)
            => GetHashCodeDelegate(obj);
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="equals">这个委托用于执行相等比较</param>
        /// <param name="getHashCode">这个委托用于计算哈希值</param>
        public EqualityComparer(Func<Obj, Obj, bool> equals, Func<Obj, int> getHashCode)
        {
            this.EqualsDelegate = (x, y) => ToolEqual.JudgeNull(x, y) ?? equals(x!, y!);
            this.GetHashCodeDelegate = x => x is null ? 0 : getHashCode(x);
        }
        #endregion
    }
}
