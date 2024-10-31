using System.Numerics;

namespace System.MathFrancis;

/// <summary>
/// 这个类型是<see cref="ISizePos{Num}"/>的实现，
/// 可以视为一个具有位置的二维平面，
/// 它仅支持以浮点数作为坐标和平面大小的数字
/// </summary>
/// <inheritdoc cref="ISizePos{Num}"/>
sealed class SizePosFloatingPoint<Num> : SizePos<Num>
    where Num : IFloatingPoint<Num>
{
    #region 抽象实现：返回全部四个顶点
    public override IReadOnlyList<IPoint<Num>> Vertex
    {
        get
        {
            var (width, height) = Size;
            return [Position,
            Position.Move(width,Num.Zero),
            Position.Move(width,-height),
            Position.Move(Num.Zero,-height)];
        }
    }
    #endregion
}