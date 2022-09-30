using Options = System.Collections.Generic.IReadOnlyDictionary<string, object>;
using System.Maths.Plane;
using System.Office.Excel.Realize;
using System.Office.Excel;
using System.Maths;

namespace System.DataFrancis.Office.Block;

/// <summary>
/// 该记录是块建造者的基类，
/// 它通过元数据获取块的排列方式，
/// 并可以创建一个用来拉取或推送数据的管道
/// </summary>
/// <typeparam name="Pipe">想要建造的数据管道</typeparam>
public abstract record BlockBuildBase<Pipe>
{
    #region 使用说明
    /*使用说明：
      本类型比较复杂，它按照以下流程执行拉取或推送操作，每个流程都有详细说明：

      0.首先需要在被拉取或推送的工作簿中创建一个工作表，
      它的表名和MetadataSheetName相同，它是该工作簿的元数据，
      用来提供配置选项，并告诉数据位于工作表的哪个位置，元数据工作表可以隐藏，但是不要删除。
      为了提高性能，同一个工作簿中的所有工作表都可以共享该元数据。
     
      使用元数据带来的好处是：无需重新编译程序，就可以适配不同格式的工作表，
      因为输入元数据是直接在Excel上进行的

      1.创建对象并初始化所有属性，其中只有ColumnName是必填项，
      凡是在这个集合中的列名，都一定会被推送到工作表，或出现在被拉取的IData中，
      FriendlyName在大部分情况下也是必填项
    
      2.进入配置阶段，函数会调用CreateOptions来创建选项，选项是一系列键值对，
      它是推送或拉取过程的配置表，能够被之后的操作访问，如果选项包含ColumnName中的值，
      则这个值对应的单元格的公式必须包含对工作表对应数据区域的引用，该引用会被转换成ISizePosPixel，
      如果ColumnName的值不在选项中，它会获得对(0,0)单元格的引用，
      这是为了使调用者能够访问单元格，进一步访问整个工作表
    
      3.函数会通过选项初始化数据地图，数据地图规定了数据以及数据的列在工作表的什么位置，
      数据是排他的，两条数据永远不会重叠在一起，在处理了一条数据以后，
      函数会将当前位置移动到下一条数据
    
      4.完成配置阶段，将控制权交给本类型的派生类
    
      基于元数据的拉取和推送非常复杂，难以学习，但是功能也非常强大，
      作者建议在实际使用的时候，对它进行二次封装，因为很多简单的场景可能并不需要它*/
    #endregion
    #region 关于配置选项
    #region 配置阶段
    #region 说明文档
    /*问：为什么不采取这种模式：
      只使用一个字典，键是实体类列名，值是友好名称，
      这样可以合并两个属性，使其更加简洁？
      答：这是因为实体类列名和友好名称是两个完全不同的东西，
      友好名称影响的是配置选项，但是实体类列名是最终被写入Data的东西，
      它可以与配置选项有关，也可以无关*/
    #endregion
    #region 元数据工作表名
    #region 静态属性
    /// <summary>
    /// 返回<see cref="MetadataSheetName"/>的默认值，
    /// 设置它可以使整个应用程序的配置选项保持一致
    /// </summary>
    public const string MetadataSheetNameDefaults = "元数据";
    #endregion
    #region 实例属性
    /// <summary>
    /// 获取储存元数据的工作表名，
    /// 如果不初始化它，默认使用<see cref="MetadataSheetNameDefaults"/>
    /// </summary>
    public string MetadataSheetName { get; init; } = MetadataSheetNameDefaults;
    #endregion
    #endregion 
    #region 元数据开始的行列号
    /// <summary>
    /// 指示元数据开始的行列号，
    /// 它位于元数据区域的最左上角
    /// </summary>
    public (int Row, int Column) Begin { get; init; }
    #endregion
    #region 友好名称
    private IReadOnlyDictionary<string, string>? FriendlyNameField;

    /// <summary>
    /// 这个字典的键是友好名称，值是数据真实的列名，
    /// 友好名称指的是显示在工作表中，供非专业人士阅读的名称
    /// </summary>
    public IReadOnlyDictionary<string, string> FriendlyName
    {
        get => FriendlyNameField ??= CreateCollection.EmptyDictionary(FriendlyNameField);
        init => FriendlyNameField = value;
    }
    #endregion
    #region 枚举实体类的列名
    /// <summary>
    /// 枚举实体类的列名，
    /// 它们会被实际推送到工作表，或拉取到数据中
    /// </summary>
    public IEnumerable<string> ColumnName { get; init; } = Array.Empty<string>();
    #endregion
    #endregion
    #region 提取选项阶段
    #region 用来提取选项的委托
    #region 默认方法
    /// <summary>
    /// 用来提取选项的默认方法
    /// </summary>
    /// <param name="keyAndCell">枚举从<see cref="Begin"/>开始，
    /// 选项的名称以及包含值的单元格，这些单元格全部是有效数据，无需执行检查</param>
    /// <returns>从单元格中提取到的选项名称和选项，按照约定，
    /// 它的值如果包含对公式的引用，则会将其转换为<see cref="ISizePosPixel"/>，
    /// 如果为<see langword="null"/>，则指示停止提取选项</returns>
    public static IEnumerable<(string Key, object Value)> CreateOptionsDefault(IEnumerable<(string Key, IExcelCells Value)> keyAndCell)
        => keyAndCell.Select(x =>
        {
            object? value = x.Value switch
            {
                { FormulaA1: { } f } when f.IndexOf("!") is var i && i >= 0 &&
                    f[(i + 1)..] is { } add && ExcelRealizeHelp.MatchA1.IsMatch(add)
                    => ExcelRealizeHelp.AddressToSizePos(add),
                var cell => cell.Value.ToText
            };
            return (x.Key, value ?? throw new NullReferenceException("配置选项的值不能为null"));
        });
    #endregion
    #region 委托
    /// <summary>
    /// 该委托用于从工作表中提取选项，
    /// 它的参数是选项的键和值所在的单元格，
    /// 返回值是提取出来的选项
    /// </summary>
    public Func<IEnumerable<(string Key, IExcelCells Value)>, IEnumerable<(string Key, object Value)>> CreateOptions { get; init; } = CreateOptionsDefault;
    #endregion
    #endregion 
    #region 通过选项创建数据地图
    #region 默认方法
    /// <summary>
    /// 根据选项创建数据地图的默认方法
    /// </summary>
    /// <param name="columnName">实体类的列名，它们会被实际写入到工作表中</param>
    /// <param name="options">从工作表中提取出来的选项，按照约定，它的键已经从友好名称转换，
    /// 它的值如果包含对公式的引用，则会将其转换为<see cref="ISizePosPixel"/></param>
    /// <returns>新创建的数据地图</returns>
    public static DataMap InitializationDataMapDefault(IEnumerable<string> columnName, Options options)
    {
        var boundary = options[nameof(DataMap.Boundary)].To<ISizePosPixel>();
        return new()
        {
            Boundary = boundary,
            IsVertical = options.GetValueOrDefault(nameof(DataMap.IsVertical)).To<bool?>(false) ?? true,
            IsVerticalElement = options.GetValueOrDefault(nameof(DataMap.IsVerticalElement)).To<bool?>(false) ?? true,
            ColumnBoundary = columnName.Select(key =>
            {
                if (options.TryGetValue(key, out var value))
                {
                    var pos = value.To<ISizePosPixel>();
                    return (key, pos.Transform(pos.FirstPixel.ToRel(boundary.FirstPixel)));
                }
                return (key, CreateMath.SizePosPixel(0, 0, 1, 1));
            }).ToDictionary(true)
        };
    }
    #endregion
    #region 委托
    /// <summary>
    /// 该委托的第一个参数是实体类的列名，
    /// 第二个参数是通过元数据获取到的选项，
    /// 返回值就是创建好的数据地图，如果不指定，则使用一个默认方法，
    /// 由于with记录会复制字段，请务必使用静态委托
    /// </summary>
    public Func<IEnumerable<string>, Options, DataMap> InitializationDataMap { get; init; } = InitializationDataMapDefault;
    #endregion
    #endregion
    #endregion
    #endregion
    #region 创建对象
    #region 缓存数据管道
    /// <summary>
    /// 对数据管道的缓存，
    /// 它可以在相同工作簿的不同工作表中通用
    /// </summary>
    protected Pipe? CachePipe { get; set; }
    #endregion
    #region 初始化数据地图
    /// <summary>
    /// 初始化数据地图，并返回一个元组，
    /// 它的项分别是加载好的数据地图，以及从元数据那里读取到的配置
    /// </summary>
    /// <param name="book">待加载数据地图的工作簿</param>
    /// <returns></returns>
    protected (DataMap DataMap, Options Options) Initialization(IExcelBook book)
    {
        #region 枚举配置单元格的本地函数
        IEnumerable<(string Key, IExcelCells Value)> Fun()
        {
            var sheet = book[MetadataSheetName, false];
            var (r, c) = Begin;
            var position = CreateMath.SizePosPixel(c, -r, 2, 1);
            var pipe = CreateBlock.BlockFromSimple(sheet, position, new BlockSimpleColumn[]
            {
                new()
                {
                    ColumnName ="Key",
                    GetValue = x =>
                    {
                        var value =x.Value.ToText;
                        return value is null ? null: FriendlyName.GetValueOrDefault(value)??value;
                    }
                },
                new()
                {
                    ColumnName = "Value",
                    GetValue = static x => x
                }
            });
            return pipe.Query<IData>().ToArray().Select(x => (x["Key"]!.ToString()!, x["Value"].To<IExcelCells>()!));
        }
        #endregion
        var options = CreateOptions(Fun()).ToDictionary(true);
        return (InitializationDataMap(ColumnName, options), options);
    }
    #endregion
    #region 正式方法
    /// <summary>
    /// 通过工作表，创建想要建造的对象
    /// </summary>
    /// <param name="sheet">数据所在的工作表</param>
    /// <returns></returns>
    public abstract Pipe Create(IExcelSheet sheet);
    #endregion 
    #endregion
}
