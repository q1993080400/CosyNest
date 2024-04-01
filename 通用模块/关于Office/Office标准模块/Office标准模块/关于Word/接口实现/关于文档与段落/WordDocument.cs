using System.IOFrancis;
using System.IOFrancis.FileSystem;
using System.Performance;

namespace System.Office.Word.Realize;

/// <summary>
/// 在实现<see cref="IWordDocument"/>的时候，
/// 可以继承自本类型，以减少重复的工作
/// </summary>
/// <remarks>
/// 用指定的文件路径初始化Word文档
/// </remarks>
/// <param name="path">文档所在的文件路径，
/// 如果文档不是通过文件创建的，则为<see langword="null"/></param>
public abstract class WordDocument(PathText? path) : FromIO(path), IWordDocument
{
    #region 返回IWordDocument接口
    /// <summary>
    /// 返回这个对象的<see cref="IWordDocument"/>接口形式，
    /// 使其可以调用显式实现的成员
    /// </summary>
    public IWordDocument Interface
        => this;
    #endregion
    #region 关于文件与文档
    #region 返回页面对象
    public abstract IPage Page { get; }
    #endregion
    #region 枚举所有段落
    public abstract IEnumerable<IWordParagraph> Paragraphs { get; }
    #endregion
    #endregion
    #region 关于文本
    #region 返回无格式文本
    public abstract string Text { get; }
    #endregion
    #region 插入文本
    public abstract IWordRange CreateWordRange(string text, Index? begin = null);
    #endregion
    #region 插入段落
    public abstract IWordParagraphText CreateParagraph(string text, Index? begin = null);
    #endregion
    #region 获取Word范围
    public abstract IWordRange this[Range range] { get; }
    #endregion
    #region 替换文本
    public abstract void Replace(string old, string @new);
    #endregion
    #endregion
    #region 关于位置
    #region 获取文档的长度
    public abstract int Length { get; }
    #endregion
    #region 枚举非文本段落的索引
    /// <summary>
    /// 这个集合枚举文档中非文本部分的索引，
    /// 按照规范，它的元素应该按底层实现位置升序排序
    /// </summary>
    protected abstract IEnumerable<WordPos> NotTextIndex { get; }
    #endregion
    #region 有关位置转换
    #region 将文本位置转换为实际位置
    public int ToIndexActual(int indexText)
    {
        var close = NotTextIndex.LastOrDefault(x => indexText >= x.IndexText.Begin);
        if (close is null)
            return indexText;
        var ((_, te), (_, ae), _) = close;
        return indexText <= te ? ae : indexText + ae - te;
    }
    #endregion
    #region 将实际位置转换为文本位置
    public int ToIndexText(int indexActual)
    {
        var close = NotTextIndex.LastOrDefault(x => indexActual >= x.IndexActual.Begin);
        if (close is null)
            return indexActual;
        var ((_, te), (_, ae), _) = close;
        return indexActual <= ae ? te : indexActual - (ae - te);
    }
    #endregion
    #region 将实际位置转换为底层实现位置
    /// <summary>
    /// 将实际位置转换为底层实现位置
    /// </summary>
    /// <param name="indexActual">待转换的实际位置</param>
    /// <param name="getBegin">当实际位置位于一个Office对象的底层实现位置之间的时候，
    /// 如果这个参数为<see langword="true"/>，取底层实现位置的开始，否则取结束</param>
    /// <returns></returns>
    public virtual int ToUnderlying(int indexActual, bool getBegin)
    {
        var close = NotTextIndex.LastOrDefault(x => indexActual >= x.IndexActual.Begin);
        if (close is null)
            return indexActual;
        var (_, (_, ae), (ub, ue)) = close;
        return indexActual <= ae ?
            (getBegin ? ub : ue) : ue + indexActual - ae;
    }
    #endregion
    #region 将底层实现位置转换为实际位置
    /// <summary>
    /// 将底层实现位置转换为实际位置
    /// </summary>
    /// <param name="indexUnderlying">待转换的底层实现位置</param>
    /// <param name="getBegin">当底层实现位置位于一个Office对象的实际位置之间的时候，
    /// 如果这个参数为<see langword="true"/>，取实际位置的开始，否则取结束</param>
    /// <returns></returns>
    public virtual int FromUnderlying(int indexUnderlying, bool getBegin)
    {
        var close = NotTextIndex.LastOrDefault(x => indexUnderlying >= x.IndexUnderlying.Begin);
        if (close is null)
            return indexUnderlying;
        var (_, (ab, ae), (_, ue)) = close;
        return indexUnderlying <= ue ?
            (getBegin ? ab : ae) : indexUnderlying - (ue - ae);
    }
    #endregion
    #endregion
    #endregion
    #region 关于事件
    #region 长度被改变时引发的事件
    #region 调用事件
    #region 指定位置，新文本和旧文本
    /// <summary>
    /// 调用<see cref="LengthChange"/>事件，通知长度已更改
    /// </summary>
    /// <param name="pos">发生修改的位置</param>
    /// <param name="oldText">修改前的旧文本，如果为<see langword="null"/>，代表是添加新文本</param>
    /// <param name="newText">被修改后的新文本，如果为<see langword="null"/>，代表删除旧文本</param>
    public void CallLengthChange(int pos, string? oldText = null, string? newText = null)
    {
        var newLen = newText?.Length ?? 0;
        var oldLen = oldText?.Length ?? 0;
        CallLengthChange(pos..(pos + oldLen + 1), newLen - oldLen);
    }
    #endregion
    #region 指定位置和改变的长度
    /// <summary>
    /// 调用<see cref="LengthChange"/>事件，通知长度已更改
    /// </summary>
    /// <param name="range">发生修改的范围</param>
    /// <param name="change">指示执行完改变以后，总长度变更了多少</param>
    public void CallLengthChange(Range range, int change)
    {
        if (LengthChangeWeak is { } && change is not 0)
            LengthChangeWeak.Invoke(range, change);
    }
    #endregion
    #endregion
    #region 弱事件封装
    /// <inheritdoc cref="LengthChange"/>
    private WeakDelegate<Action<Range, int>>? LengthChangeWeak;
    #endregion
    #region 正式事件
    public event Action<Range, int> LengthChange
    {
        add => ToolPerformance.AddWeakDel(ref LengthChangeWeak, value);
        remove => LengthChangeWeak?.Remove(value);
    }
    #endregion
    #endregion
    #endregion
}
