using System.Collections;

namespace System.Office.Excel.Realize;

/// <summary>
/// 在实现<see cref="IExcelSheetCollection"/>时，
/// 可以继承自本类型，以减少重复的工作
/// </summary>
/// <remarks>
/// 将指定的工作簿封装进对象
/// </remarks>
/// <param name="book">指定的工作簿</param>
public abstract class ExcelSheetCollection(IExcelBook book) : IExcelSheetCollection
{
    #region 返回工作簿
    public IExcelBook Book { get; } = book;
    #endregion
    #region 关于工作表集合
    #region 获取工作表
    #region 枚举工作表的枚举器
    public abstract IEnumerator<IExcelSheet> GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    #endregion
    #region 返回工作表的索引
    public virtual int IndexOf(IExcelSheet item) =>
        this.BinarySearch(item, false);
    #endregion
    #region 根据索引获取工作表
    public virtual IExcelSheet this[int index]
    {
        get => this.ToArray()[index];
        set
        {
            Insert(index, value);
            RemoveAt(index + 1);
        }
    }
    #endregion
    #endregion
    #region 检查某工作表是否存在
    public virtual bool Contains(IExcelSheet item)
        => item.Book == Book;
    #endregion
    #region 返回工作表数量
    public virtual int Count
        => this.ToArray().Length;
    #endregion
    #region 返回集合是否只读
    public virtual bool IsReadOnly
        => false;
    #endregion
    #endregion
    #region 对工作表的操作
    #region 关于添加工作表
    #region 插入工作表
    public abstract void Insert(int index, IExcelSheet item);
    #endregion
    #region 添加空白表
    public abstract IExcelSheet Add(string name = "Sheet");
    #endregion
    #region 添加新工作表
    public virtual void Add(IExcelSheet item)
        => item.Copy(collection: this);
    #endregion
    #endregion
    #region 关于移除工作表
    #region 移除指定索引处的工作表
    public virtual void RemoveAt(int index)
        => this[index].Delete();
    #endregion
    #region 移除全部工作表
    public virtual void Clear()
        => this.ToArray().ForEach(x => x.Delete());
    #endregion
    #region 移除指定工作表
    public virtual bool Remove(IExcelSheet item)
    {
        if (item.Book != this)
            return false;
        item.Delete();
        return true;
    }
    #endregion
    #endregion
    #region 复制工作表到另一个数组
    public virtual void CopyTo(IExcelSheet[] array, int arrayIndex)
    {
        foreach (var i in this)
            array[arrayIndex++] = i;
    }

    #endregion
    #endregion
    #region 构造函数
    #endregion
}
