using System.Diagnostics.CodeAnalysis;

namespace System.Office.Excel;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个Excel行或者列，
/// 它可以包含一个或多个子行或子列
/// </summary>
public interface IExcelRC : IExcelRange, IEnumerable<IExcelRC>
{
    #region 说明文档
    /*问：按照其他框架的做法，行和列是两个不同的类型，
      为什么本框架不这么设计？
      答：因为作者考虑到，它们除了一个是行一个是列以外，
      在其他地方没有任何区别，因此作者认为这种差别不足以使它们成为两个类型，
      只需要一个属性来区分它们就可以了*/
    #endregion
    #region 返回对象是否为行
    /// <summary>
    /// 如果这个属性返回<see langword="true"/>，
    /// 代表这个对象是行，否则代表是列
    /// </summary>
    bool IsRow { get; }
    #endregion
    #region 关于行列的位置和规模
    #region 返回开始和结束行列数
    /// <summary>
    /// 返回开始和结束行列数
    /// </summary>
    (int Begin, int End) Range { get; }
    #endregion
    #region 返回行或列的数量
    /// <summary>
    /// 返回行或列的数量
    /// </summary>
    int Count
    {
        get
        {
            var (b, e) = Range;
            return e - b + 1;
        }
    }
    #endregion
    #endregion
    #region 关于行与列的样式
    #region 隐藏和取消隐藏
    /// <summary>
    /// 获取或设置这个行或列是否被隐藏，
    /// 如果返回<see langword="null"/>，
    /// 代表该对象包含多个行或列，
    /// 且有的被隐藏，有的没有
    /// </summary>
    /// <exception cref="NotSupportedException"><see cref="IsHide"/>禁止写入<see langword="null"/>值</exception>
    bool? IsHide { get; set; }
    #endregion
    #region 获取或设置高度或宽度
    /// <summary>
    /// 如果这个对象是行，则获取或设置它的高度，
    /// 如果这个对象是列，则获取或设置它的宽度，
    /// 如果多个行列的高宽不相同，则返回<see langword="null"/>
    /// </summary>
    [DisallowNull]
    double? HeightOrWidth { get; set; }

    /*实现本API请遵循以下规范：
      #本属性可能返回null，但是不能写入null，
      在发生这种情况时，请抛出异常*/
    #endregion
    #region 自动调整行高与列宽
    /// <summary>
    /// 调用这个方法自动调整行高与列宽，
    /// 使其刚好容纳下行列的内容
    /// </summary>
    void AutoFit();
    #endregion
    #endregion
    #region 删除行或者列
    /// <summary>
    /// 删除这个行或者列的所有数据
    /// </summary>
    void Delete();
    #endregion
}
