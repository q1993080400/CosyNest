
using static System.ExceptionIntervalOut;
using static System.MathFrancis.ToolArithmetic;

namespace System.MathFrancis;

/// <summary>
/// 关于位运算与非十进制的工具类
/// </summary>
public static class ToolBit
{
    #region 关于进制转换
    #region 从十进制转换为其他进制
    /// <summary>
    /// 将一个十进制数转换为其他进制，并返回一个元组，
    /// 它的项分别是新数字的整数部分和小数部分，以及每一位数字的值
    /// </summary>
    /// <param name="num">待转换的十进制数</param>
    /// <param name="bit">目标数字的位权，也就是几位数</param>
    /// <param name="progress">如果转换后的新数字不是整数，则这个参数指示其最大精度</param>
    /// <returns></returns>
    public static (List<int> Integer, List<int> Decimal) FromDecimal(Num num, int bit = 2, int progress = 6)
    {
        #region 本地函数
        int Fun(int index)
            => num < Pow(bit, index) ? index : Fun(++index);
        #endregion
        var maxBit = Fun(1) - 1;                                        //首先求出转换后整数的位数
        var (i, d) = (new List<int>(), new List<int>());
        while (d.Count <= progress)
        {
            var (div, rem, _) = Div(num, Pow(bit, maxBit));
            (maxBit >= 0 ? i : d).Add(div);
            num = rem;
            if (num == 0)                           //如果待转换的数被除尽，则停止循环
            {
                if (maxBit > 0)                     //如果数字被除尽，但是还有多余的位数
                    i.AddRange(Enumerable.Repeat(0, maxBit));               //则将这些位数补0
                break;
            }
            maxBit--;
        }
        return (i, d);
    }
    #endregion
    #region 从其他进制转换为十进制
    /// <summary>
    /// 将其他进制的数字转换为十进制
    /// </summary>
    /// <param name="bit">待转换的数字的位权，也就是几进制数</param>
    /// <param name="numInteger">枚举待转换数字整数部分每一位的值，如果为<see langword="null"/>，代表整数部分为0</param>
    /// <param name="numDecimal">枚举待转换数字小数部分每一位的值，如果为<see langword="null"/>，代表没有小数部分</param>
    /// <returns></returns>
    public static Num ToDecimal(int bit, IEnumerable<int>? numInteger, IEnumerable<int>? numDecimal)
    {
        numInteger ??= new[] { 0 };
        numDecimal ??= Array.Empty<int>();
        var maxBit = numInteger.Count();
        return numInteger.Union(false, numDecimal).Aggregate((Num)0, (seed, source) =>
        {
            maxBit--;
            return Pow(bit, maxBit) * source + seed;
        });
    }
    #endregion
    #region 将任意进制的数字互相转换
    /// <summary>
    /// 将任意进制的数字互相转换
    /// </summary>
    /// <param name="fromBit">待转换的数字的位权，也就是几进制数</param>
    /// <param name="fromInteger">枚举待转换数字整数部分每一位的值，如果为<see langword="null"/>，代表整数部分为0</param>
    /// <param name="fromDecimal">枚举待转换数字小数部分每一位的值，如果为<see langword="null"/>，代表没有小数部分</param>
    /// <param name="toBit">转换目标数字的位权</param>
    /// <param name="progress">如果转换后的新数字不是整数，则这个参数指示其最大精度</param>
    /// <returns></returns>
    public static (List<int> Integer, List<int> Decimal) ToOther(int fromBit, IEnumerable<int>? fromInteger, IEnumerable<int>? fromDecimal,
        int toBit, int progress = 6)
    {
        var dec = ToDecimal(fromBit, fromInteger, fromDecimal);
        return FromDecimal(dec, toBit, progress);
    }
    #endregion
    #endregion
    #region 转换为比特位
    #region 传入字符串
    /// <summary>
    /// 将字符串转换为比特位数组
    /// </summary>
    /// <param name="byteText">待转换的字符串，它只能有1和0两个字符</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">出现了1和0以外的字符</exception>
    public static byte[] ToBit(string byteText)
        => byteText.Select(x => x switch
         {
             '0' => (byte)0,
             '1' => (byte)1,
             var t => throw new ArgumentException($"出现未知字符{t}，它无法转换为比特位")
         }).ToArray();
    #endregion
    #region 传入字节
    /// <summary>
    /// 将一个字节拆开，返回它的每个比特位
    /// </summary>
    /// <param name="byte">待拆开的字节</param>
    /// <returns></returns>
    public static byte[] ToBit(byte @byte)
    {
        var array = new byte[8];
        for (byte i = 8; i > 0; i--)
        {
            array[i - 1] = (byte)(@byte % 2);
            @byte /= 2;
        }
        return array;
    }
    #endregion 
    #endregion
    #region 转换为字节
    /// <summary>
    /// 将比特位组合起来，使其成为字节
    /// </summary>
    /// <param name="bytes">待组合的比特位，
    /// 如果它的长度不是8，或者包含了1和0以外的数字，则会引发异常</param>
    /// <returns></returns>
    public static byte ToBytes(byte[] bytes)
    {
        if (bytes.Length is not 8)
            throw new Exception("字节数组必须有8个元素");
        byte num = 0;
        for (byte i = 8, pow = 1; i > 0; i--)
        {
            var bit = bytes[i - 1];
            if (bit > 1)
                throw new Exception("比特位只允许0和1两个值");
            num += (byte)(bit * pow);
            pow *= 2;
        }
        return num;
    }
    #endregion
    #region 关于位域
    #region 枚举整数的所有位域
    /// <summary>
    /// 枚举一个整数的所有位域，类似设置了<see cref="FlagsAttribute"/>的<see cref="Enum"/>，
    /// 集合元素是一个元组，它的项分别是位域的幂和指数
    /// </summary>
    /// <param name="num">要枚举位域的整数</param>
    /// <returns></returns>
    public static IEnumerable<(int Power, int Index)> AllFlag(int num)
    {
        Check(0, null, num);
        for (int power = 1, index = 0; index <= 30; index++, power <<= 1)
        {
            if ((power & num) == power)
                yield return (power, index);
        }
    }
    #endregion
    #region 从整数中删除位域
    /// <summary>
    /// 从整数中删除位域
    /// </summary>
    /// <param name="num">要删除位域的整数</param>
    /// <param name="remove">待删除的位域</param>
    /// <returns></returns>
    public static int RemoveFlag(int num, params int[] remove)
    {
        Check(0, null, num);
        return remove.Aggregate(num, (seed, num) => seed & ~num); ;
    }
    #endregion
    #region 创建位域
    /// <summary>
    /// 创建一个位域，并返回
    /// </summary>
    /// <param name="index">该位域所兼容的以2为底的指数</param>
    /// <returns></returns>
    public static int CreateFlag(params int[] index)
    {
        index = index.Distinct().ToArray();
        index.ForEach(x => Check(0, 30, x));
        return index.Select(x => (int)Math.Pow(2, x)).Aggregate((x, y) => x | y);
    }
    #endregion
    #endregion
}
