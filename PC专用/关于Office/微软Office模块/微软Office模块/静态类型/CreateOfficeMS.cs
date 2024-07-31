using System.IOFrancis.FileSystem;
using System.Office.Excel;
using System.Office.Word;

namespace System.Office;

/// <summary>
/// 这个静态类为创建通过COM组件实现的Office对象提供帮助
/// </summary>
public static class CreateOfficeMS
{
    #region 公开成员
    #region 创建Excel对象
    #region 直接创建
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
    #region 创建并升级
    /// <summary>
    /// 创建一个底层由COM组件实现的Excel工作簿，
    /// 如果它是旧版本的Excel，还会将其升级为新版本
    /// </summary>
    /// <param name="path">Excel工作簿的路径</param>
    /// <returns>一个元组，它的项分别是工作簿的路径，
    /// 如果发生升级，可能和旧路径不同，以及创建好的工作簿</returns>
    /// <inheritdoc cref="ExcelBookLast(string, bool)"/>
    public static async Task<(string NewPath, IExcelBook ExcelBook)> ExcelBookLast(string path, bool autoSave)
    {
        var newPath = await UpdateOfficeFile(path);
        return (newPath, ExcelBook(newPath, autoSave));
    }
    #endregion
    #endregion
    #region 创建Word对象
    #region 直接创建
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
    #region 创建并升级
    /// <summary>
    /// 创建一个底层由COM组件实现的Word文档，
    /// 如果它是旧版本的Word，还会将其升级为新版本
    /// </summary>
    /// <param name="path">Word文档的路径</param>
    /// <returns>一个元组，它的项分别是文档的路径，
    /// 如果发生升级，可能和旧路径不同，以及创建好的Word文档</returns>
    /// <inheritdoc cref="WordDocument(string, bool)"/>
    public static async Task<(string NewPath, IWordDocument WordDocument)> WordDocumentLast(string path, bool autoSave)
    {
        var newPath = await UpdateOfficeFile(path);
        return (newPath, WordDocument(newPath, autoSave));
    }
    #endregion
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
    #endregion
    #region 内部成员
    #region 升级Office文件
    /// <summary>
    /// 升级一个Office文件，并返回升级后的文件路径
    /// </summary>
    /// <param name="path">Office文件的路径，
    /// 如果不需要升级，或者不是Office文件，则原路返回</param>
    /// <returns></returns>
    private static async Task<string> UpdateOfficeFile(string path)
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
        return ToolPath.SplitFilePath(path).Extended switch
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
