namespace System.MathFrancis;

/// <summary>
/// 凡是实现这个接口的类型，
/// 都可以用来生成随机数
/// </summary>
public interface IRandom
{
    #region 说明文档
    /*实现本接口请遵循以下规范：
      #本接口的所有API都应该是线程安全的，
      或者应该提供一个其他机制来保证线程安全，
      例如：通过一个使用ThreadStatic修饰的静态属性来获取随机数生成器，
      因为随机数生成器一般是静态属性，很可能在多个线程中访问它们*/
    #endregion
    #region 有关数学的随机数
    #region 生成大于0小于1的随机数
    /// <summary>
    /// 生成一个大于等于0，且小于1的浮点数
    /// </summary>
    /// <returns></returns>
    Num Rand();
    #endregion
    #region 生成指定范围内的随机数
    /// <summary>
    /// 生成一个位于指定范围内的随机数
    /// </summary>
    /// <param name="min">随机数的最小值</param>
    /// <param name="max">随机数的最大值，注意：
    /// 最后生成的随机数可能等于该最大值</param>
    /// <returns></returns>
    Num RandRange(Num min, Num max)
    {
        if (min > max)
            (min, max) = (max, min);
        return Math.Min((decimal)max, min + (max - min) * 1.01 * Rand());
    }

    /*问：为什么在生成随机数的时候，
      最大值和最小值的差要乘以1.01？
      答：因为Rand方法生成的浮点数永远小于1，
      所以如果不执行这个操作的话，上限max会永远达不到，
      尤其是在从集合中随机选取元素的时候会更加明显，这会导致最后一个元素永远无法选择到，
      这个不太符合正常人的思维习惯，但是，
      它也会轻微地增加生成更大的随机数的概率，
      在一般情况下，1%的波动是可以接受的，特殊情况需自己斟酌*/
    #endregion
    #region 随机掷骰子
    /// <summary>
    /// 随机掷一次骰子，然后返回是否命中了这个概率
    /// </summary>
    /// <param name="molecular">命中概率的分子</param>
    /// <param name="denominator">命中概率的分母</param>
    /// <returns></returns>
    bool RollDice(double molecular, double denominator = 100)
        => Rand() < molecular / denominator;
    #endregion
    #region 将一个数字按照比例随机分配
    /// <summary>
    /// 将一个数字按照比例随机分配
    /// </summary>
    /// <param name="num">待分配的数字</param>
    /// <param name="count">指示将数字分配为多少份</param>
    /// <param name="discrete">指示分配的离散程度，也就是最大值最多是最小值的多少倍</param>
    /// <returns></returns>
    Num[] RandDistribute(Num num, int count, double discrete = 2)
    {
        ExceptionIntervalOut.Check(1d, null, discrete);
        var weight = Enumerable.Range(1, count).Select(x => RandRange(1, discrete)).ToArray();
        return ToolArithmetic.Segmentation(num, weight);
    }
    #endregion
    #endregion
    #region 有关集合的随机数
    #region 用随机数据填满字节数组
    /// <summary>
    /// 用随机数据填满字节数组，并返回
    /// </summary>
    /// <param name="length">字节数组的长度</param>
    /// <returns></returns>
    byte[] RangdBytes(int length);
    #endregion
    #region 返回随机索引
    /// <summary>
    /// 返回一个集合的随机合法索引，
    /// 如果集合中没有任何元素，返回-1
    /// </summary>
    /// <typeparam name="Obj">集合元素的类型</typeparam>
    /// <param name="collections">要返回索引的集合</param>
    /// <param name="count">集合元素的数量，
    /// 直接指定它可以改善性能，否则函数会尝试获取这个值</param>
    /// <returns></returns>
    int RandIndex<Obj>(IEnumerable<Obj> collections, int? count = null)
    {
        var maxIndex = (count ?? collections.Count()) - 1;
        if (maxIndex is < 0)
            return -1;
        return RandRange(0, maxIndex);
    }
    #endregion
    #region 返回随机元素
    /// <summary>
    /// 返回一个集合的随机元素，
    /// 如果该集合没有元素，则会引发异常
    /// </summary>
    /// <typeparam name="Obj">集合元素的类型</typeparam>
    /// <param name="collections">要返回元素的集合</param>
    /// <returns></returns>
    Obj RandElement<Obj>(IEnumerable<Obj> collections)
        => collections.ElementAt(RandIndex(collections));
    #endregion
    #region 挑选出若干不重复元素
    /// <summary>
    ///从一个集合中挑选出若干不重复元素
    /// </summary>
    /// <typeparam name="Obj">集合元素的类型</typeparam>
    /// <param name="collections">要返回不重复元素的集合</param>
    /// <param name="count">指示应该获取多少个不重复元素</param>
    /// <returns></returns>
    IEnumerable<Obj> RandElements<Obj>(IEnumerable<Obj> collections, int count)
    {
        var list = collections.Distinct().ToList();
        var c = list.Count;
        if (c is 0)
            return Array.Empty<Obj>();
        if (count >= c)
            return list;
        var index = new HashSet<int>();
        var isBeyond = count > c / 2;       //根据要返回的元素数量和总元素数量的比，选择性能最优的方案
        while (index.Count < (isBeyond ? c - count : count))
        {
            index.Add(RandIndex(list, c));
        }
        if (isBeyond)
        {
            index.OrderDescending().ForEach(list.RemoveAt);
            return list;
        }
        return index.Select(x => list[x]).ToArray();
    }

    /*说明文档
      问：isBeyond变量的作用是什么？
      答：它通过要返回的元素数量，决定使用什么模式来提高性能，举例说明：
      假设集合有10个元素，只需要挑选1个，则从集合中随机挑选1个元素性能最高
      假设集合有10个元素，需要挑选9个，则从集合中随机移除1个元素性能最高
    
      问：为什么不使用Random.GetItems方法？
      答：因为它的结果可能重复，甚至即便在原始集合不重复的情况下，
      也有可能生成重复的结果*/
    #endregion
    #region 打乱集合的顺序
    /// <summary>
    /// 打乱集合的顺序
    /// </summary>
    /// <typeparam name="Obj">集合元素的类型</typeparam>
    /// <param name="collections">要打乱顺序的集合</param>
    /// <returns></returns>
    IEnumerable<Obj> Shuffle<Obj>(IEnumerable<Obj> collections);
    #endregion
    #endregion
    #region 生成随机字符串
    /// <summary>
    /// 生成随机字符串
    /// </summary>
    /// <param name="minLength">生成字符串的最小长度</param>
    /// <param name="maxLength">生成字符串的最大长度</param>
    /// <param name="category">这个数组枚举允许生成的字符所在区间，
    /// 如果为空数组，则表示可生成任意字符</param>
    /// <returns></returns>
    string RandText(int minLength, int maxLength, params IIntervalSpecific<char>[] category)
    {
        if (category.Any(x => !x.IsClosed))
            throw new Exception($"{nameof(category)}中的全部元素必须为封闭区间");
        category = category.Length is not 0 ? category : [IInterval.CreateSpecific((char?)ushort.MinValue, (char?)ushort.MaxValue)];      //如果不指定合法字符范围，则默认可以生成任何UTF16字符
        int len = RandRange(minLength, maxLength);
        var charts = new char[len];
        for (int i = 0; i < len; i++)
        {
            var (beg, end) = RandElement(category);
            charts[i] = (char)RandRange(beg!.Value, end!.Value);
        }
        return new string(charts);
    }
    #endregion
}
