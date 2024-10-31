using System.MathFrancis;

namespace System.Office;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个Office对象
/// </summary>
public interface IOfficeObject
{
    #region 对象的位置
    /// <summary>
    /// 获取这个对象左上角的坐标
    /// </summary>
    IPoint<double> Pos { get; set; }
    #endregion
    #region 对象的大小
    /// <summary>
    /// 获取对象的大小，
    /// 注意：为符合Office的习惯，单位是厘米
    /// </summary>
    ISize<double> Size { get; set; }
    #endregion
    #region 是否置于文字的顶层
    /// <summary>
    /// 如果这个属性为<see  langword="true"/>，
    /// 表示这个对象置于文字的顶层，否则表示置于底层
    /// </summary>
    bool InTextTop { get; set; }
    #endregion
    #region 旋转度数
    /// <summary>
    /// 获取或设置这个对象旋转的度数
    /// </summary>
    double Rotation { get; set; }
    #endregion
}
