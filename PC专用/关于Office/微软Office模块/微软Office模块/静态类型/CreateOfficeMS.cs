using System.IOFrancis;
using System.IOFrancis.FileSystem;
using System.Office.Excel;
using System.Office.Word;

namespace System.Office;

/// <summary>
/// 这个静态类为创建通过COM组件实现的Office对象提供帮助
/// </summary>
public static class CreateOfficeMS
{
    #region 关于Excel
    #region 创建Excel对象
    /// <summary>
    /// 创建一个底层由COM组件实现的Excel对象
    /// </summary>
    /// <param name="autoSave">如果这个值为<see langword="true"/>，
    /// 释放工作簿的时候，会自动保存</param>
    /// <returns></returns>
    /// <inheritdoc cref="ExcelBookMicrosoft(string?)"/>
    public static IExcelBook ExcelBook(string? path, bool autoSave)
        => new ExcelBookMicrosoft(path)
        {
            AutoSave = autoSave
        };
    #endregion
    #region 筛选Excel文件
    /// <summary>
    /// 返回某个文件是否是受本模块支持的Excel文件
    /// </summary>
    /// <param name="file">待判断的文件</param>
    /// <returns></returns>
    public static bool FilterExcelFile(IFile file)
        => !FilterOfficeTemporaryFile(file) && file.NameExtension is "xls" or "xlsx";
    #endregion
    #endregion
    #region 关于Word
    #region 创建Word对象
    /// <summary>
    /// 创建一个底层由COM组件实现的Word文档
    /// </summary>
    /// <param name="autoSave">如果这个值为<see langword="true"/>，
    /// 释放文档的时候，会自动保存</param>
    /// <returns></returns>
    /// <inheritdoc cref="WordDocumentMicrosoft(string?)"/>
    public static IWordDocument WordDocument(string? path, bool autoSave)
        => new WordDocumentMicrosoft(path)
        {
            AutoSave = autoSave
        };
    #endregion
    #region 创建Word位置
    #region 创建特殊位置
    /// <summary>
    /// 创建Word特殊位置
    /// </summary>
    /// <param name="pos">描述特殊位置的枚举</param>
    /// <returns></returns>
    public static IWordPosSpecial WordPosSpecial(WordPosSpecialEnum pos)
        => new WordPosSpecialMicrosoft()
        {
            Pos = pos
        };
    #endregion
    #region 创建页位置
    /// <summary>
    /// 创建Word页位置，它按照页查找Word位置
    /// </summary>
    /// <param name="pageIndex">页的索引，从0开始</param>
    /// <returns></returns>
    public static IWordPosPage WordPosPage(int pageIndex)
        => new WordPosPageMicrosoft()
        {
            PageIndex = pageIndex
        };
    #endregion
    #endregion
    #region 筛选Word文件
    /// <summary>
    /// 返回某个文件是否是受本模块支持的Word文件
    /// </summary>
    /// <param name="file">待判断的文件</param>
    /// <returns></returns>
    public static bool FilterWordFile(IFile file)
        => !FilterOfficeTemporaryFile(file) && file.NameExtension is "doc" or "docx";
    #endregion
    #endregion
    #region 关于Office文件
    #region 筛选Office临时文件
    /// <summary>
    /// 返回某个文件是否为Office临时文件，
    /// 它通常应该被直接忽略
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static bool FilterOfficeTemporaryFile(IFile file)
        => file.NameSimple.StartsWith("~$");
    #endregion
    #region 升级Office文件
    /// <summary>
    /// 升级一个Office文件，并返回升级后的文件路径
    /// </summary>
    /// <param name="path">Office文件的路径，
    /// 如果不需要升级，或者不是Office文件，则原路返回</param>
    /// <returns></returns>
    public static async Task<string> UpdateOfficeFile(string path)
    {
        #region 用来升级的本地函数
        async Task<string> Update(Func<IOfficeUpdate> createOffice)
        {
            #region 本地函数
            async Task<string> Fun()
            {
                await using var office = createOffice();
                return office.Update();
            }
            #endregion
            var newPath = await Fun();
            File.Delete(path);
            return newPath;
        }
        #endregion
        var file = CreateIO.File(path);
        if (FilterOfficeTemporaryFile(file))
            return path;
        return file.NameExtension switch
        {
            "xls" => await Update(() => new ExcelBookMicrosoft(path)
            {
                AutoSave = false
            }),
            "doc" => await Update(() => new WordDocumentMicrosoft(path)
            {
                AutoSave = false
            }),
            _ => path
        };
    }
    #endregion 
    #endregion
}
