using System.IOFrancis;
using System.Office.Word.Realize;

namespace System.Office.Word;

/// <summary>
/// 所有实现本接口的类型，
/// 都可以视作一个Word文档
/// </summary>
public interface IWordDocument : IFromIO
{
    #region 说明文档
    /*实现本接口请遵循以下规范：
      #在创建对象时，如果指定了路径，则自动从这个路径中加载文档，
      如果没有指定，则创建一个空白文档

      #本接口的所有API中，除非特别说明，
      否则有关位置的参数和返回值指的都是实际位置

      问：文本位置指的是什么？
      答：指的是某一对象在无格式文本中的位置，
      举例说明：如果某一字符在无格式文本中的索引为3，
      且它的后面有一张图片，那么这个字符和图片的文本位置都是3

      问：实际位置指的是什么？
      答：指的是文本字符和图片，图表等Office对象在文档中的位置，
      按照规范，任何Office对象，无论其大小都占用1个字符的位置，
      在存在Office对象的情况下，文本位置和实际位置可能不同，举例说明：
      如果一个字符的索引为3，且它的后面有一张图片，
      那么字符的实际位置是3，图片的实际位置是4

      问：类似超链接这样的Office对象占用额外的实际位置吗？
      答：不占用，因为根据思维习惯，超链接应该是一种特殊的文本，
      而不是Office对象，它占用的位置应该和它的显示文本相同

      问：在某些Word实现中，还存在底层实现位置，这是什么意思？
      答：指的是可以被底层Word库识别的位置，举例说明：
      按照规范，超链接不占用额外的实际位置，但是在微软COM实现的Word中，
      超链接需要占用位置来储存地址，那么就需要进行实际位置和底层实现位置的转换，以避免位置错误
      底层实现位置的规则由引用的Word底层实现来决定，在不同的实现中，它可能不存在，也有可能规则不同

      问：为什么需要文本位置和实际位置两种定位方式？
      答：这两种定位方式有不同的分工，
      文本位置在处理无格式文本时比较方便，
      它可以让一些通用API（例如正则表达式）返回的索引在Word中继续有效，
      而实际位置可以更方便的定位图片等Office对象*/
    #endregion
    #region 关于文件与文档
    #region 返回页面对象
    /// <summary>
    /// 返回页面对象，
    /// 它可以管理这个Word文档的页面设置和打印
    /// </summary>
    IPage Page { get; }
    #endregion
    #region 枚举所有段落
    /// <summary>
    /// 获取一个枚举所有段落的枚举器
    /// </summary>
    IEnumerable<IWordParagraph> Paragraphs { get; }
    #endregion
    #endregion
    #region 关于文本
    #region 获取Word范围
    /// <summary>
    /// 获取指定的文本范围
    /// </summary>
    /// <param name="range">描述范围开始和结束的对象</param>
    /// <returns></returns>
    IWordRange this[Range range] { get; }
    #endregion
    #region 返回无格式文本
    /// <summary>
    /// 返回Word文档的无格式文本
    /// </summary>
    string Text { get; }

    /*实现本API请遵循以下规范：
      #本API要求性能达到O(1)，
      因为它的调用非常广泛和频繁*/
    #endregion
    #region 插入文本
    /// <summary>
    /// 在指定的位置插入文本
    /// </summary>
    /// <param name="text">待插入的文本</param>
    /// <param name="begin">指定新文本开始的位置，如果为<see langword="null"/>，默认为在文档末尾追加文本</param>
    /// <returns>新插入的文本范围</returns>
    IWordRange CreateWordRange(string text, Index? begin = null);
    #endregion
    #region 插入段落
    /// <summary>
    /// 在指定的位置插入文本段落
    /// </summary>
    /// <param name="text">待插入的文本</param>
    /// <param name="begin">指定新文本开始的位置，如果为<see langword="null"/>，默认为在文档末尾追加文本</param>
    /// <returns>新插入的文本段落</returns>
    IWordParagraphText CreateParagraph(string text, Index? begin = null);
    #endregion
    #region 替换文本
    /// <summary>
    /// 将文档中的所有旧文本替换为新文本
    /// </summary>
    /// <param name="old">旧文本</param>
    /// <param name="new">新文本</param>
    void Replace(string old, string @new);
    #endregion
    #endregion
    #region 关于位置
    #region 获取文档的长度
    /// <summary>
    /// 获取文档的长度，
    /// 这里指的是实际位置
    /// </summary>
    int Length { get; }
    #endregion
    #region 签发书签
    /// <summary>
    /// 签发一个本文档的书签
    /// </summary>
    /// <param name="index">书签的开始索引</param>
    /// <returns></returns>
    IWordBookmark GetBookmark(Index index)
        => new WordBookmark(this, index.GetOffset(Length));
    #endregion
    #region 将文本位置转换为实际位置
    /// <summary>
    /// 将文本位置转换为实际位置
    /// </summary>
    /// <param name="indexText">待转换的文本位置</param>
    /// <returns></returns>
    int ToIndexActual(int indexText);
    #endregion
    #region 将实际位置转换为文本位置
    /// <summary>
    /// 将实际位置转换为文本位置
    /// </summary>
    /// <param name="indexActual">待转换的文本位置</param>
    /// <returns></returns>
    int ToIndexText(int indexActual);
    #endregion
    #endregion
    #region 事件
    #region 当长度改变时触发
    /// <summary>
    /// 当文档的长度被改变时，触发这个事件，
    /// 事件的参数分别是受影响的片段的范围，
    /// 以及执行完改变以后，总长度变更了多少
    /// </summary>
    event Action<Range, int> LengthChange;

    /*说明文档：
      这个事件一般为IWordBookmark提供服务，
      在实现该事件时，请遵循以下规范：

      #举例说明事件参数的含义：

      1.假设在文档的最开头插入“微软”，
      则事件参数分别是0..0，2

      2.假设文档一共有10个字，在末尾删除“开发者”，
      则事件参数分别是7..9，-3

      3.假设“谷歌地球”的第一个字位于文档的5号索引，
      把这一段替换为“谷歌”，则事件的参数分别为：5..8，-2
      这个事件参数的值只和字数变动有关，与新文本的内容无关，例如：
      如果把上文的“谷歌地球”替换成“微软”，那么参数的值和上面仍然一样

      #在调用这个事件时，第一个参数必须为封闭范围，例如0..3，
      不能是像3..这样的开放范围，因为开放范围无法被书签追踪

      #该事件必须使用弱事件实现，因为大量的书签都会注册本事件，
      如果不使用弱事件，会造成严重的内存泄露*/
    #endregion
    #endregion
}
