using System.IOFrancis.FileSystem;
using System.Office.Word;
using System.Office.Word.Realize;

using Microsoft.Office.Interop.Word;

using Task = System.Threading.Tasks.Task;

namespace System.Office;

/// <summary>
/// 底层使用微软COM组件实现的Word文档
/// </summary>
sealed class WordDocumentMicrosoft : WordDocument, IOfficeUpdate
{
    #region 公开成员
    #region 返回打印对象
    public override IWordPages Pages { get; }
    #endregion
    #region 从剪贴板粘贴
    public override IWordRange Paste(IWordPos pos)
    {
        var range = pos.GetRange(this);
        range.Select();
        Application.Selection.Paste();
        range.End += 1;
        return new WordRangeMicrosoft(range);
    }
    #endregion
    #region 图片管理对象
    public override IWordImageManage ImageManage { get; }
    #endregion
    #region 返回文档的主体部分
    public override IWordRange Content
        => new WordRangeMicrosoft(Document.Content);
    #endregion
    #region 查找
    public override IReadOnlyCollection<IWordRange> Find(string condition)
    {
        #region 获取搜索结果的本地函数
        IEnumerable<IWordRange> FindRange()
        {
            var find = Document.Content.Find;
            while (find.Execute(condition))
            {
                var range = find.Parent;
                var start = range.Start;
                var end = range.End;
                var newRange = Document.Range(start, end);
                yield return new WordRangeMicrosoft(newRange);
            }
        }
        #endregion
        return [.. FindRange()];
    }
    #endregion
    #region 升级Office文件
    public string Update()
    {
        Document.Convert();
        var newPath = ToolPath.RefactoringPath(Path!, refactoringExtended: static _ => "docx");
        Document.SaveAs(newPath, WdSaveFormat.wdFormatDocumentDefault);
        return newPath;
    }
    #endregion
    #endregion
    #region 内部成员
    #region 封装的对象
    #region 封装的Application
    /// <summary>
    /// 获取封装的Application对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    public Application Application
        => Document.Application;
    #endregion
    #region 封装的Word文档
    /// <summary>
    /// 获取封装的Word文档对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    public Document Document { get; }
    #endregion
    #endregion
    #region 关于文件或路径
    #region 默认格式
    protected override string DefaultFormat
        => "docx";
    #endregion
    #region 检查文件扩展名
    protected override bool CheckExtensionName(string extensionName)
        => extensionName is "doc" or "docx";
    #endregion
    #region 保存文件
    protected override Task SaveRealize(string path, bool isSitu)
    {
        if (isSitu)
            Document.Save();
        else
            Document.SaveAs(path);
        return Task.CompletedTask;
    }
    #endregion
    #endregion 
    #region 释放对象
    protected override ValueTask DisposeAsyncActualRealize()
    {
        var application = Application;
        Document.Close(WdSaveOptions.wdDoNotSaveChanges);
        application.Quit(WdSaveOptions.wdDoNotSaveChanges);
        return ValueTask.CompletedTask;
    }
    #endregion 
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的路径初始化对象
    /// </summary>
    /// <inheritdoc cref="WordDocument(string?)"/>
    public WordDocumentMicrosoft(string? path)
        : base(path)
    {
        var application = new Application()
        {
            Visible = false,
            DisplayAlerts = WdAlertLevel.wdAlertsNone,
        };
        var docs = application.Documents;
        Document = path is null || !File.Exists(path) ?
            docs.Add() : docs.Open(path);
        Pages = new WordPagesMicrosoft(Document);
        ImageManage = new WordImageManageMicrosoft(this, Document.Shapes, Document.InlineShapes);
    }
    #endregion
}
