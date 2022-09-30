using System.IOFrancis;
using System.IOFrancis.FileSystem;

using iText.Kernel.Pdf;

namespace System.Media.Drawing.PDF;

/// <summary>
/// 这个类型是底层使用IText实现的PDF文档
/// </summary>
sealed class DocumentIText : FromIO, IPDFDocument
{
#pragma warning disable CS8618

    #region 说明文档
    /*问：IText的底层模块将PDF文档分为读取器和写入器，
      但是本类型似乎抹平了这种区别，它是怎么做到的？
      答：本类型在内部仍然有一个读取器和写入器，但是区别是，
      在访问读取器的时候，如果状态被修改，写入器会将自己的数据同步到读取器，并将其重新加载，
      在外部看来，就好像只有一个统一的读写器一样
    
      这种设计提高了便利程度，但是也存在性能隐患，
      因为它会在内存中频繁地复制大量数据，并在大型堆上产生对象，
      建议仅在不会长时间运行的程序中使用本类型*/
    #endregion
    #region 作者对底层PdfDocument对象的了解
    /*经过查阅官方文档和自行测试，
      作者对本接口所依赖的PdfDocument对象得到了以下认知：
      
      #PdfDocument分为只读模式和只写模式，似乎还有一个印章模式，
      但是这个模式的用途非常有限，因为有些操作要求必须位于只读或只写模式，
      这个设计思路非常怪异，如果它的目的是为了推销该模块的付费版，那么这确实是成功的
    
      #PdfReader在初始化后，会一次性读取流中的所有数据，
      此后这个流可以被安全释放
    
      #PdfWriter在初始化时会先写入第一页空白PDF，
      然后调用Close方法时向流中写入全部内容，因此，
      不要试图在阅读器和书写器中共用一个流，因为写入空白页会破坏PDF流的结构*/
    #endregion
    #region 实现接口所需底层成员
    #region 封装的对象
    #region 阅读器
    private PdfDocument ReadField;

    /// <summary>
    /// 返回用于阅读PDF的对象
    /// </summary>
    internal PdfDocument Read
    {
        get
        {
            if (IsSave)
                throw new NotSupportedException(@"该PDF文档已经被保存，它应该被废弃掉，不要继续使用它");
            if (!IsChange)
                return ReadField;
            ReadField.Close();
            Write.Close();
            StreamWrite.Reset();
            ReadField = new(new PdfReader(StreamWrite));
            StreamWrite.Dispose();
            InitializationWrite();
            IsChange = false;
            return ReadField;
        }
    }
    #endregion
    #region 书写器
    private PdfDocument WriteField;

    /// <summary>
    /// 返回用于写入PDF的对象
    /// </summary>
    internal PdfDocument Write
    {
        get => IsSave ?
            throw new NotSupportedException(@"该PDF文档已经被保存，它应该被废弃掉，不要继续使用它") :
            WriteField;
        set
        {
            value.SetCloseWriter(false);
            WriteField = value;
        }
    }
    #endregion
    #region 写入PDF的流
    /// <summary>
    /// 获取用于写入PDF的流
    /// </summary>
    private Stream StreamWrite { get; set; }
    #endregion
    #endregion
    #region 有关状态
    #region 是否已经保存
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 代表该PDF已经被保存，按照设计，它应该被废弃，不能继续使用
    /// </summary>
    private bool IsSave { get; set; }
    #endregion
    #region 是否为全新创建
    /// <summary>
    /// 如果这个值为<see langword="true"/>，代表本对象是全新创建的，
    /// 它需要删除默认添加的第一个空白页
    /// </summary>
    private bool IsNew { get; }
    #endregion
    #region 是否改变了状态
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 代表状态被改变，需要重新加载读取器和写入器，
    /// 在修改PDF文档时，请将其设为<see langword="true"/>
    /// </summary>
    internal bool IsChange { get; set; }
    #endregion
    #region 初始化写入器
    /// <summary>
    /// 调用本方法初始化写入器，
    /// 它还会将读取器中的所有内容复制到写入器，
    /// 必须在读取器初始化完毕后调用
    /// </summary>
    private void InitializationWrite()
    {
        StreamWrite = new MemoryStream();
        Write = new(new PdfWriter(StreamWrite));
        ReadField.CopyPagesTo(1, ReadField.GetNumberOfPages(), Write);
    }
    #endregion
    #endregion
    #endregion
    #region 对象的格式
    protected override string FormatTemplate => "pdf";
    #endregion
    #region 有关保存和释放PDF
    #region 保存PDF
    protected override async Task SaveRealize(string path, bool isSitu)
    {
        using var file = CreateIO.FileStream(path);
        if (IsNew && Write.GetNumberOfPages() > 1)
            Write.RemovePage(1);
        Write.Close();
        StreamWrite.Reset();
        await StreamWrite.CopyToAsync(file);
        IsSave = true;
    }
    #endregion
    #region 释放PDF
    protected override ValueTask DisposeAsyncActualRealize()
    {
        ReadField.Close();
        WriteField.Close();
        StreamWrite.Dispose();
        return ValueTask.CompletedTask;
    }
    #endregion
    #endregion
    #region 枚举所有页面
    public IPDFCollect Pages { get; }
    #endregion
    #region 构造函数
    /// <summary>
    /// 通过指定的流初始化PDF
    /// </summary>
    /// <param name="stream">一个用来读取PDF的流</param>
    /// <param name="path">指示PDF所在的文件路径，
    /// 如果它只存在于内存中，则为<see langword="null"/></param>
    public DocumentIText(Stream stream, PathText? path)
        : base(path, IPDFDocument.FileTypePDF)
    {
        using var memory = stream.CopyToMemory();
        ReadField = new(new PdfReader(memory));
        InitializationWrite();
        Pages = new PagesIText(this);
        IsNew = path is null;
    }
    #endregion
}
