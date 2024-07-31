using System.Collections;

using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;

using Shape = Microsoft.Office.Interop.Word.Shape;
using Shapes = Microsoft.Office.Interop.Word.Shapes;

namespace System.Office.Word;

/// <summary>
/// 这个类型可以用来管理Word文档的所有图片
/// </summary>
/// <param name="document">封装的Word文档对象，本对象的功能就是通过它实现的</param>
/// <param name="shapes">封装的图形对象</param>
/// <param name="inlineShapes">封装的行内图形对象</param>
sealed class WordImageManageMicrosoft(WordDocumentMicrosoft document, Shapes shapes, InlineShapes inlineShapes) : IWordImageManage
{
    #region 公开成员
    #region 添加图片
    public IWordImage Add(string path, IWordPos pos)
    {
        var range = pos.GetRange(document);
        var image = inlineShapes.AddPicture(path, Range: range).ConvertToShape();
        image.RelativeHorizontalPosition = WdRelativeHorizontalPosition.wdRelativeHorizontalPositionPage;
        image.RelativeVerticalPosition = WdRelativeVerticalPosition.wdRelativeVerticalPositionPage;
        return new WordImageMicrosoft(image);
    }
    #endregion
    #region 图片的数量
    public int Count
        => Images.Count();
    #endregion
    #region 返回图片枚举器
    public IEnumerator<IWordImage> GetEnumerator()
        => Images.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    #endregion 
    #endregion
    #region 内部成员
    #region 枚举所有Word图片
    /// <summary>
    /// 枚举所有Word图片
    /// </summary>
    private IEnumerable<IWordImage> Images
    {
        get
        {
            foreach (var item in shapes.OfType<Shape>().Where(x => x.Type is MsoShapeType.msoPicture))
            {
                yield return new WordImageMicrosoft(item);
            }
            foreach (var item in inlineShapes.OfType<InlineShape>().Where(x => x.Type is WdInlineShapeType.wdInlineShapePicture))
            {
                yield return new WordInlineImageMicrosoft();
            }
        }
    }
    #endregion
    #endregion
}
