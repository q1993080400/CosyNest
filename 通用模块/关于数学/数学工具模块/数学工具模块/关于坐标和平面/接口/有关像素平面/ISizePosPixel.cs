using System.Collections.Generic;

namespace System.Maths
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个具有位置的像素平面
    /// </summary>
    public interface ISizePosPixel : ISizePixel
    {
        #region 说明文档
        /*实现本接口请遵循以下规范：
          #本接口的所有API所返回的IPoint对象不是坐标，
          而是通过像素来表示位置，例如(1,1)表示该像素位于第2行第2列，
          而不是X和Y坐标都为1

          #基于上一条，虽然IPoint使用有理数类型Num来表示，
          但是本接口所有API所返回的IPoint对象都只能出现整数，
          这是因为像素是原子化不可分割的*/
        #endregion
        #region 第一个像素的位置
        /// <summary>
        /// 返回左上角第一个像素的位置
        /// </summary>
        IPoint FirstPixel { get; }
        #endregion
        #region 关于顶点
        #region 返回全部四个顶点
        /// <summary>
        /// 返回一个集合，它枚举像素平面的四个顶点的像素位置
        /// </summary>
        IReadOnlyList<IPoint> Vertex
        {
            get
            {
                var (h, v) = this;
                h = ToolArithmetic.Limit(true, h - 1, 0);
                v = -ToolArithmetic.Limit(true, v - 1, 0);
                return new[]
                {
                    FirstPixel,
                    FirstPixel.Move(h,0),
                    FirstPixel.Move(h,v),
                    FirstPixel.Move(0,v)
                };
            }
        }
        #endregion
        #region 返回左上角和右下角的顶点
        /// <summary>
        /// 返回这个像素平面的界限，
        /// 也就是它左上角和右下角的像素位置
        /// </summary>
        (IPoint TopLeft, IPoint BottomRight) Boundaries
        {
            get
            {
                var vertex = this.Vertex;
                return (vertex[0], vertex[2]);
            }
        }
        #endregion
        #endregion
        #region 将像素平面转换为坐标平面
        /// <summary>
        /// 将像素平面转换为具有位置的坐标平面
        /// </summary>
        /// <param name="pixelSize">每个像素的大小，
        /// 如果为<see langword="null"/>，则默认为(1,1)</param>
        /// <returns></returns>
        ISizePos ToSizePos(ISize? pixelSize = null)
        {
            var (width, height) = pixelSize ??= CreateMath.Size(1, 1);
            var (r, t) = FirstPixel;
            var size = ToSize(pixelSize);
            return CreateMath.SizePos(
                CreateMath.Point(width * (r - 1), height * (t - 1)), size);
        }
        #endregion
        #region 偏移像素平面
        /// <summary>
        /// 偏移和扩展像素平面，并返回偏移后的新像素平面
        /// </summary>
        /// <param name="extendedHorizontal">水平方向扩展的像素数量，
        /// 在加上原有的像素数量后不能为负值</param>
        /// <param name="extendedVertical">垂直方向扩展的像素数量，
        /// 在加上原有的像素数量后不能为负值</param>
        /// <param name="offsetRight">平面左上角向右偏移的像素数量</param>
        /// <param name="offsetTop">平面左上角向上偏移的像素数量</param>
        /// <returns></returns>
        ISizePosPixel Transform(int extendedHorizontal, int extendedVertical, int offsetRight, int offsetTop)
            => CreateMath.SizePosPixel(
                FirstPixel.Move(offsetRight, offsetTop),
                Transform(extendedHorizontal, extendedVertical));
        #endregion
        #region 解构ISizePosPixel
        /// <summary>
        /// 解构本对象
        /// </summary>
        /// <param name="horizontal">用来接收水平方向像素点数量的对象</param>
        /// <param name="vertical">用来接收垂直方向像素点数量的对象</param>
        /// <param name="firstPixel">用来接收左上角第一个像素的位置的对象</param>
        void Deconstruct(out int horizontal, out int vertical, out IPoint firstPixel)
        {
            var (h, v) = this;
            horizontal = h;
            vertical = v;
            firstPixel = this.FirstPixel;
        }
        #endregion
    }
}
