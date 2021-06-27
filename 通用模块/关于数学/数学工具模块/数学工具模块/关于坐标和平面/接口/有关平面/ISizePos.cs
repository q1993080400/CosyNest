using System.Collections.Generic;

namespace System.Maths
{
    /// <summary>
    /// 这个接口代表一个具有位置的二维平面
    /// </summary>
    public interface ISizePos : ISize
    {
        #region 二维平面的位置
        /// <summary>
        /// 返回二维平面左上角的坐标
        /// </summary>
        IPoint Position { get; }
        #endregion
        #region 返回平面的中心
        /// <summary>
        /// 返回这个平面中心的点坐标
        /// </summary>
        IPoint Center
        {
            get
            {
                var (w, h) = Size;
                return Position.Move(w / 2, h / -2);
            }
        }
        #endregion
        #region 关于顶点
        #region 返回全部四个顶点
        /// <summary>
        /// 返回一个集合，它枚举二维平面的四个顶点
        /// </summary>
        IReadOnlyList<IPoint> Vertex
        {
            get
            {
                var (w, h) = this;
                h = -h;
                return new[]
                {
                    Position,
                    Position.Move(w,0),
                    Position.Move(w,h),
                    Position.Move(0,h)
                };
            }
        }
        #endregion
        #region 返回左上角和右下角的顶点
        /// <summary>
        /// 返回这个平面的界限，
        /// 也就是它左上角和右下角的顶点
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
        #region 转换为像素平面
        /// <summary>
        /// 将这个对象转换为带位置的像素平面
        /// </summary>
        /// <param name="pixelSize">指定单个像素的大小</param>
        /// <param name="rounding">当这个平面的宽高不能整除像素的大小时，
        /// 如果这个值为<see langword="true"/>，代表将多余的部分抛弃，
        /// 否则代表将多余的部分补齐为一个像素</param>
        /// <returns></returns>
        ISizePosPixel ToSizePosPixel(ISize pixelSize, bool rounding)
        {
            var size = ToSizePixel(pixelSize, rounding);
            var (r, t) = Position;
            var (pw, ph) = pixelSize;
            var point = CreateMath.Point(
                ToolArithmetic.Sim(r * pw, isProgressive: !rounding),
                ToolArithmetic.Sim(t * ph, isProgressive: !rounding));
            return CreateMath.SizePosPixel(point, size);
        }
        #endregion
        #region 偏移平面
        /// <summary>
        /// 偏移和扩展平面，并返回偏移后的新平面
        /// </summary>
        /// <param name="extendedWidth">扩展平面的宽度，加上原有的宽度以后不能为负值</param>
        /// <param name="extendedHeight">扩展平面的高度，加上原有的高度以后不能为负值</param>
        /// <param name="offsetRight">平面左上角向右偏移的坐标</param>
        /// <param name="offsetTop">平面左上角向上偏移的坐标</param>
        /// <returns></returns>
        ISizePos Transform(Num extendedWidth, Num extendedHeight, Num offsetRight, Num offsetTop)
            => CreateMath.SizePos(Position.Move(offsetRight, offsetTop),
                Transform(extendedWidth, extendedHeight));
        #endregion
        #region 解构ISizePos
        /// <summary>
        /// 将本对象解构为宽，高和位置
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="pos">位置</param>
        void Deconstruct(out Num width, out Num height, out IPoint pos)
        {
            var (w, h) = this.Size;
            width = w;
            height = h;
            pos = this.Position;
        }
        #endregion
    }
}
