using Microsoft.Office.Interop.Word;

namespace System.Office.Word;

/// <summary>
/// 这个静态类声明了有关Word的扩展方法
/// </summary>
static class ExtendWord
{
    #region 获取MS文档
    /// <summary>
    /// 获取一个<see cref="IWordDocument"/>封装的<see cref=" Document"/>对象
    /// </summary>
    /// <param name="document">Word文档对象，
    /// 如果它不是微软COM组件实现的Word文档，则引发异常</param>
    /// <returns></returns>
    public static Document GetMSDocument(this IWordDocument document)
        => document is WordDocumentMicrosoft { Document: { } msDocument } ?
        msDocument :
        throw new NotSupportedException($"{document.GetType()}不是微软实现的Word文档，无法获取它的COM文档");
    #endregion
    #region 获取文档位置
    /// <summary>
    /// 获取这个文档位置
    /// </summary>
    /// <param name="pos">文档位置</param>
    /// <param name="document">位置所在的文档</param>
    /// <returns></returns>
    public static MSWordRange GetRange(this IWordPos pos, IWordDocument document)
    {
        var range = pos.PrecisePos(document);
        return range is WordRangeMicrosoft { WordRange: { } wordRange } ?
            wordRange :
            throw new NotSupportedException($"{range.GetType()}不是微软COM组件实现的Word范围，无法获取它的位置");
    }
    #endregion
}
