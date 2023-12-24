using System.Collections;


namespace System.Media.Drawing.PDF;

/// <summary>
/// 该类型是一个PDF页面集合
/// </summary>
sealed class PagesIText(DocumentIText pdf) : IPDFCollect
{
    #region 封装的PDF对象
    /// <summary>
    /// 获取封装的PDF对象
    /// </summary>
    private DocumentIText PDF { get; } = pdf;
    #endregion
    #region PDF文档
    IPDFDocument IPDFCollect.PDF => PDF;
    #endregion
    #region 关于集合本身
    #region 获取页数
    public int Count
        => PDF.Write.GetNumberOfPages();
    #endregion
    #region 获取是否可变
    public bool IsReadOnly => false;
    #endregion
    #region 获取枚举器
    public IEnumerator<IPDFPage> GetEnumerator()
    {
        for (int i = 0, c = Count; i < c; i++)
        {
            yield return new PageIText(PDF.Write.GetPage(i + 1), PDF);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    #endregion
    #region 复制到数组
    public void CopyTo(IPDFPage[] array, int arrayIndex)
    {
        foreach (var (item, count, _) in this.PackIndex())
        {
            array[count + arrayIndex] = item;
        }
    }
    #endregion
    #region 按索引枚举元素
    public IPDFPage this[int index]
    {
        get => new PageIText(PDF.Write.GetPage(index + 1), PDF);
        set
        {
            RemoveAt(index);
            Insert(index, value);
        }
    }
    #endregion
    #region 复制到另一个集合
    public void CopyTo(IPDFCollect target, Range? range, Index? pos = null)
    {
        var read = PDF.Read;
        if (Count is 0)
            return;
        var (b, r) = (range ?? Range.All).GetOffsetAndEnd(this);
        var p = (pos ?? ^0).GetOffset(target) + 1;                  //页码从1开始，所以索引需加1
        var targetPDF = target.To<PagesIText>().PDF;
        read.CopyPagesTo(b + 1, r, targetPDF.Write, p);
        targetPDF.IsChange = true;
    }
    #endregion
    #region 根据元素返回索引
    public int IndexOf(IPDFPage item)
    {
        if (Contains(item) && item is PageIText { Page: var sp })
        {
            foreach (var (page, index, _) in this.PackIndex())
            {
                if (page is PageIText { Page: var cp } && cp.Equals(sp))
                    return index;
            }
        }
        return -1;
    }
    #region 检查页是否存在于集合中
    public bool Contains(IPDFPage item)
        => item.PDF.Pages.Equals(this);
    #endregion
    #endregion
    #endregion
    #region 关于添加移除元素
    #region 移除元素（按照索引）
    public void RemoveAt(int index)
    {
        PDF.Write.RemovePage(index + 1);
        PDF.IsChange = true;
    }
    #endregion
    #region 移除元素（按照元素）
    public bool Remove(IPDFPage item)
    {
        if (Contains(item))
            return true;
        PDF.Write.RemovePage(item.To<PageIText>().Page);
        PDF.IsChange = true;
        return false;
    }
    #endregion
    #region 移除全部元素
    public void Clear()
    {
        for (int i = Count; i > 0; i--)
        {
            PDF.Write.RemovePage(i);
        }
        PDF.IsChange = true;
    }
    #endregion
    #region 添加元素
    public void Add(IPDFPage item)
    {
        PDF.Write.AddPage(item.To<PageIText>().Page);
        PDF.IsChange = true;
    }
    #endregion
    #region 添加空白页
    public IPDFPage Add()
    {
        var @new = new PageIText(PDF.Write.AddNewPage(), PDF);
        PDF.IsChange = true;
        return @new;
    }
    #endregion
    #region 插入元素
    public void Insert(int index, IPDFPage item)
    {
        var source = item.PDF.Pages;
        var sourceIndex = source.IndexOf(item);
        source.CopyTo(this, sourceIndex..(sourceIndex + 1), index);
        PDF.IsChange = true;
    }

    #endregion
    #endregion
    #region 构造函数
    #endregion
}
