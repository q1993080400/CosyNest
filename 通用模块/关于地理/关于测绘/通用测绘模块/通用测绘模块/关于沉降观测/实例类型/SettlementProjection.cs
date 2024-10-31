using System.MathFrancis;

namespace System.Mapping.Settlement;

/// <summary>
/// 表示一个<see cref="ISettlement"/>的投影，
/// 它可以用来更方便地序列化和反序列化<see cref="ISettlement"/>
/// </summary>
sealed record SettlementProjection
{
    #region 说明文档
    /*问：本类型具有什么作用？
      答：ISettlement具有复杂的引用关系，
      而且很多属性不需要也无法被转换，
      所以在执行序列化与反序列化时非常不方便，
      因此作者声明了本类型，它是ISettlement的投影，
      仅简单地包含需要被转换的属性，本类型无需任何转换器即可被JsonSerializer转换

      问：为什么要公开本类型，而不是仅公开一个支持ISettlement的转换器？
      答：因为ISettlement的基准点的创建比较复杂，需要SettlementRootBuild的参与，
      如果仅公开转换器的话，这个转换器需要封装SettlementRootBuild，
      因此它不能是单例的，必须为每次沉降观测创建一个新的对象，
      且必须动态地传入不同的SettlementRootBuild，这在很多框架（如ASP.NET）中会出现问题

      问：ISettlement使用递归的树形结构来描述沉降观测路线，
      但是本类型为什么不用递归，而是使用一个记录的集合来进行描述？
      答：这是因为递归极其浪费序列化深度，如果使用递归结构的话，
      需要手动为JsonSerializerOptions.MaxDepth指定一个比较大的值，
      否则转换无法正常进行，这个操作不是不可以，但是根据作者的设计，
      对本类型的转换应当无需对JsonSerializer进行任何配置，
      否则本类型就失去了意义，还不如直接开发一个转换器*/
    #endregion
    #region 有关投影与还原
    #region 投影
    /// <summary>
    /// 将<see cref="ISettlementPoint"/>投影为<see cref="SettlementProjection"/>，
    /// 它可以更加方便地执行序列化与反序列化
    /// </summary>
    /// <param name="point">待投影的<see cref="SettlementProjection"/>，
    /// 函数实际上不会投影它，而是会投影它的基准点</param>
    /// <returns></returns>
    public static SettlementProjection[] ToProjection(ISettlementPoint point)
    {
        #region 迭代器
        static IEnumerable<SettlementProjection> Fun(ISettlementPoint point)
        {
            foreach (var observatory in point.Son)
            {
                foreach (var p in observatory.Son)
                {
                    #region 用于转换的本地函数
                    static double Convert(ISettlement settlement)
                        => settlement.Recording!.Convert(CreateMapping.UTSettlement);
                    #endregion
                    yield return new(point.Name, Convert(observatory), p.Name, Convert(p));
                    foreach (var item in Fun(p))
                    {
                        yield return item;
                    }
                }
            }
        }
        #endregion
        return Fun(point.Ancestors).ToArray();
    }
    #endregion
    #region 还原
    /// <summary>
    /// 将<see cref="SettlementProjection"/>还原为<see cref="ISettlementPoint"/>，
    /// 并返回它的基准点
    /// </summary>
    /// <param name="settlements">待还原的<see cref="SettlementProjection"/>，
    /// 函数假定它的基准点位于集合的第一个元素，且集合的顺序与观测点被添加的顺序一致，
    /// 如果不是这种情况，将无法正确地完成工作</param>
    /// <param name="build">该对象封装了创建基准点所需要的数据，
    /// 注意：它的<see cref="SettlementRootBuild.Name"/>会被替换为<see cref="FrontName"/></param>
    /// <returns></returns>
    public static ISettlementPoint FromProjection(IEnumerable<SettlementProjection> settlements, SettlementRootBuild build)
    {
        var root = CreateMapping.SettlementPointRoot(build with
        {
            Name = settlements.First().FrontName
        });
        var dictionary = new Dictionary<string, ISettlementPoint>()
        {
            [root.Name] = root
        };
        foreach (var item in settlements)
        {
            #region 用于转换的本地函数
            static IUnit<IUTLength> Convert(double record)
                => CreateBaseMath.Unit(record, CreateMapping.UTSettlement);
            #endregion
            var father = dictionary[item.FrontName];
            var front = Convert(item.FrontRecord);
            var behindName = item.BehindName;
            var behind = Convert(item.BehindRecord);
            dictionary[behindName] = father.Add(front, behindName, behind);
        }
        return root;
    }
    #endregion
    #endregion
    #region 有关前后站名称与记录
    #region 前站名称
    /// <summary>
    /// 获取前站名称
    /// </summary>
    public string FrontName { get; init; }
    #endregion
    #region 前站记录
    /// <summary>
    /// 获取前站记录，以百分之一毫米为单位
    /// </summary>
    public double FrontRecord { get; init; }
    #endregion
    #region 后站名称
    /// <summary>
    /// 获取后站名称
    /// </summary>
    public string BehindName { get; init; }
    #endregion
    #region 后站记录
    /// <summary>
    /// 获取后站记录，以百分之一毫米为单位
    /// </summary>
    public double BehindRecord { get; init; }
    #endregion
    #endregion
    #region 构造函数
    /// <summary>
    /// 使用指定的参数初始化对象
    /// </summary>
    /// <param name="frontName">前站名称</param>
    /// <param name="frontRecord">前站记录，以百分之一毫米为单位</param>
    /// <param name="behindName">后站名称</param>
    /// <param name="behindRecord">后站记录，以百分之一毫米为单位</param>
    public SettlementProjection(string frontName, double frontRecord, string behindName, double behindRecord)
    {
        this.FrontName = frontName;
        this.FrontRecord = frontRecord;
        this.BehindName = behindName;
        this.BehindRecord = behindRecord;
    }
    #endregion
}
