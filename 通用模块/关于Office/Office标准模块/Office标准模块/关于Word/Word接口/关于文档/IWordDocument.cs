using System.IOFrancis;

namespace System.Office.Word;

/// <summary>
/// 所有实现本接口的类型，
/// 都可以视作一个Word文档
/// </summary>
public interface IWordDocument : IFromIO, IWordFindReplace
{
    #region 返回页面对象
    /// <summary>
    /// 返回一个用于管理Word页面和打印的对象
    /// </summary>
    IWordPages Pages { get; }
    #endregion
    #region 返回文档的主体部分
    /// <summary>
    /// 返回文档的主体部分
    /// </summary>
    IWordRange Content { get; }
    #endregion
    #region 从剪贴板粘贴
    /// <summary>
    /// 将剪贴板的内容粘贴到指定位置，
    /// 并返回粘贴的目标位置
    /// </summary>
    /// <param name="pos">要粘贴的目标位置</param>
    IWordRange Paste(IWordPos pos);
    #endregion
    #region 图片管理对象
    /// <summary>
    /// 返回一个对象，
    /// 它可以用来管理Word图片
    /// </summary>
    IWordImageManage ImageManage { get; }
    #endregion
}
