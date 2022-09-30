using System.Maths.Plane;
using System.Office.Excel;
using System.Office.Word;

namespace System.Office;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以视为一个Office对象
/// </summary>
/// <typeparam name="Obj">Office对象封装的内容类型</typeparam>
public interface IOfficeObj<out Obj>
{
    #region Office对象的大小
    /// <summary>
    /// 获取或设置Office对象的大小
    /// </summary>
    ISize Size { get; set; }
    #endregion
    #region Office对象的内容
    /// <summary>
    /// 获取Office对象的内容
    /// </summary>
    Obj Content { get; }
    #endregion
    #region 删除Office对象
    /// <summary>
    /// 将这个Office对象删除
    /// </summary>
    void Delete();
    #endregion
    #region 复制Office对象
    #region 复制到工作表
    /// <summary>
    /// 将这个对象复制到目标Excel工作表
    /// </summary>
    /// <param name="target">复制的目标工作表</param>
    /// <returns>复制后的Excel对象</returns>
    IExcelObj<Obj> Copy(IExcelSheet target);
    #endregion
    #region 复制到文档
    /// <summary>
    /// 将这个对象复制到目标Word文档
    /// </summary>
    /// <param name="target">复制的目标文档</param>
    /// <param name="pos">复制的目标位置，
    /// 如果为<see langword="null"/>，代表复制到文档的末尾</param>
    /// <returns>复制后的Word对象</returns>
    IWordParagraphObj<Obj> Copy(IWordDocument target, Index? pos = null);
    #endregion
    #endregion
}
