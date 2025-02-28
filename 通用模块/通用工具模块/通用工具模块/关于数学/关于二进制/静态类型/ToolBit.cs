using System.Numerics;

namespace System.MathFrancis;

/// <summary>
/// 关于位运算与非十进制的工具类
/// </summary>
public static class ToolBit
{
    #region 关于进制转换
    #region 从十进制转换为其他进制
    /// <summary>
    /// 将一个十进制数转换为其他进制，
    /// 并返回一个集合，它的值是转换后每一位的值，
    /// 从高位排列到低位
    /// </summary>
    /// <typeparam name="Num">数字的类型</typeparam>
    /// <param name="num">待转换的十进制数</param>
    /// <param name="bit">目标数字的位权，也就是几位数</param>
    /// <returns></returns>
    public static IReadOnlyList<Num> FromDecimal<Num>(Num num, Num bit)
        where Num : IBinaryInteger<Num>
    {
        #region 本地函数
        IEnumerable<Num> Fun()
        {
            var recursionNum = num;
            while (true)
            {
                var (quotient, remainder) = Num.DivRem(recursionNum, bit);
                yield return remainder;
                if (quotient == Num.Zero)
                    yield break;
                recursionNum = quotient;
            }
        }
        #endregion
        return [.. Fun().Reverse()];
    }
    #endregion
    #region 从其他进制转换为十进制
    /// <summary>
    /// 将其他进制的数字转换为十进制
    /// </summary>
    /// <param name="bit">待转换的数字的位权，也就是几进制数</param>
    /// <param name="num">枚举待转换数字整数部分每一位的值，
    /// 要求位权的顺序从高位到低位</param>
    /// <returns></returns>
    public static Num ToDecimal<Num>(Num bit, IEnumerable<Num> num)
        where Num : IBinaryInteger<Num>
    {
        var reverse = num.Reverse().ToArray();
        if (reverse.Length is 0)
            throw new NotSupportedException($"{nameof(num)}是一个空集合，无法将其转换为十进制数字");
        var pow = Num.One;
        var result = Num.Zero;
        foreach (var item in reverse)
        {
            if (item >= bit)
                throw new NotSupportedException($"{item}大于等于它的位权{bit}，这是不合法的数字");
            result += item * pow;
            pow *= bit;
        }
        return result;
    }
    #endregion
    #endregion 
}
