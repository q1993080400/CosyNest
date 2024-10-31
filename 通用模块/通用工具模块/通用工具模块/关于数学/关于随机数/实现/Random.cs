using System.Numerics;

namespace System.MathFrancis;

/// <summary>
/// 这个类型是<see cref="IRandom"/>的实现，
/// 可以用来生成对安全要求不严格的随机数
/// </summary>
/// <param name="random">封装的随机数生成器</param>
sealed class Random(System.Random random) : IRandom
{
    #region 生成大于0小于1的随机数
    public Num Rand<Num>()
        where Num : IFloatingPoint<Num>
        => Num.CreateChecked(random.NextDouble());
    #endregion
    #region 用随机数据填满字节数组
    public byte[] RangdBytes(int length)
    {
        var array = new byte[length];
        random.NextBytes(array);
        return array;
    }
    #endregion
    #region 打乱集合的顺序
    public IEnumerable<Obj> Shuffle<Obj>(IEnumerable<Obj> collections)
    {
        var array = collections.ToArray();
        random.Shuffle(array);
        return array;
    }
    #endregion
}
