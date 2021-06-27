using System.Maths;

namespace System.IOFrancis
{
    /// <summary>
    /// 这个类型是<see cref="IUTStorage"/>的实现，
    /// 可以视为一个计算机存储单位
    /// </summary>
    class UTStorage : UT, IUTStorage
    {
        #region 返回单位的类型
        protected override Type UTType
            => typeof(IUTStorage);
        #endregion
        #region 构造方法
        /// <inheritdoc cref="UT(string, Num)"/>
        public UTStorage(string name, Num size)
            : base(name, size)
        {

        }
        #endregion
    }
}
