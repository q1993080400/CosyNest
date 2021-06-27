using System.Maths;

namespace System.DrawingFrancis
{
    /// <summary>
    /// 这个静态类储存了一些绘图时常用的单位
    /// </summary>
    public static class DrawingUnitsCom
    {
        #region 长度单位
        #region 返回代表磅的单位
        /// <summary>
        /// 返回代表磅的长度单位，
        /// 它在Office中被经常使用
        /// </summary>
        public static IUTLength LengthPoint { get; }
        = IUTLength.Create("英美磅", 0.0003527);
        #endregion
        #endregion
    }
}
