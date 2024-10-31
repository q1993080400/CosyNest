using System.MathFrancis;

using Microsoft.Office.Interop.Word;

using Application = Microsoft.Office.Interop.Word.Application;

namespace System.Office.Word;

/// <summary>
/// 这个抽象类是所有Word对象的基类
/// </summary>
/// <param name="shape">封装的Word形状，本对象的功能就是通过它实现的</param>
abstract class WordObjectMicrosoft(Shape shape) : IWordObject
{
    #region 公开成员
    #region 对象的坐标
    public IPoint<double> Pos
    {
        get
        {
            var right = Application.PointsToCentimeters(shape.Left);
            var top = Application.PointsToCentimeters(-shape.Top);
            return CreateMath.Point<double>(right, top);
        }
        set
        {
            var (right, top) = value;
            shape.Left = Application.CentimetersToPoints((float)right);
            shape.Top = Application.CentimetersToPoints((float)-top);
        }
    }
    #endregion
    #region 对象的大小
    public ISize<double> Size
    {
        get => throw new NotImplementedException();
        set
        {
            var (width, height) = value;
            shape.Width = Application.CentimetersToPoints((float)width);
            shape.Height = Application.CentimetersToPoints((float)height);
        }
    }
    #endregion
    #region 是否置于顶层
    public bool InTextTop
    {
        get => shape.WrapFormat.Type is WdWrapType.wdWrapFront;
        set => shape.WrapFormat.Type = value ? WdWrapType.wdWrapFront : WdWrapType.wdWrapBehind;
    }
    #endregion
    #region 旋转的度数
    public double Rotation
    {
        get => shape.Rotation;
        set => shape.Rotation = (float)value;
    }
    #endregion
    #endregion
    #region 内部成员
    #region App对象
    /// <summary>
    /// 获取封装的App对象
    /// </summary>
    protected Application Application
        => shape.Application;
    #endregion
    #endregion
}
