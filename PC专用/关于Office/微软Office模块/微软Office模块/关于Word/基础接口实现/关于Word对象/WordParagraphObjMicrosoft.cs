using System.Maths.Plane;
using System.Office.Excel;
using System.Office.Word.Realize;

using Microsoft.Office.Interop.Word;

namespace System.Office.Word;

/// <summary>
/// 这个类型代表由微软COM组件实现的封装Office对象的Word段落
/// </summary>
/// <typeparam name="Obj">段落封装的Office对象的类型</typeparam>
abstract class WordParagraphObjMicrosoft<Obj> : WordParagraph, IWordParagraphObj<Obj>
{
    #region 封装的对象
    /// <summary>
    /// 获取封装的形状对象，
    /// 本对象的功能就是通过它实现的
    /// </summary>
    internal InlineShape PackShape { get; }
    #endregion
    #region 对象的信息
    #region 段落包含的内容
    public abstract Obj Content { get; }
    #endregion
    #region 对象的大小
    public ISize Size
    {
        get => PackShape.GetSize();
        set => PackShape.SetSize(value);
    }
    #endregion
    #region 段落的对齐样式
    public override OfficeAlignment Alignment
    {
        get => throw new Exception("不支持此API");
        set => throw new Exception("不支持此API");
    }
    #endregion
    #endregion
    #region 对象的操作
    #region 移动段落
    public override void Move(Index? pos)
        => throw new NotImplementedException("本API尚未实现");
    #endregion
    #region 删除段落
    protected override void DeleteRealize()
        => PackShape.Delete();
    #endregion
    #region 复制Office对象
    #region 复制到工作表
    public abstract IExcelObj<Obj> Copy(IExcelSheet target);
    #endregion
    #region 复制到文档
    public abstract IWordParagraphObj<Obj> Copy(IWordDocument target, Index? pos = null);
    #endregion
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的文档和形状初始化段落
    /// </summary>
    /// <param name="document">这个段落所在的文档</param>
    /// <param name="packShape">这个段落所封装的形状对象</param>
    public WordParagraphObjMicrosoft(WordDocument document, InlineShape packShape)
        : base(document, document.FromUnderlying(packShape.Range.Start, true))
    {
        this.PackShape = packShape;
    }
    #endregion
}
