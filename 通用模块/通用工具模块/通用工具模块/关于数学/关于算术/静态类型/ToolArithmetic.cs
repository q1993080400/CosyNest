namespace System.Maths;

/// <summary>
/// 关于算术和初等数学的工具类
/// </summary>
public static class ToolArithmetic
{
    #region 关于运算
    #region 求一个数字的幂
    /// <summary>
    /// 求一个数字的幂
    /// </summary>
    /// <param name="num">求幂的底数</param>
    /// <param name="index">求幂的指数，暂时只支持整型，默认为求平方</param>
    /// <returns></returns>
    public static Num Pow(Num num, int index = 2)
    {
        if (index == 0)
            return num == 0 ? throw new Exception("不能计算0的0次方") : 1;
        Num Fun(Num num2, int index)
           => index == 1 ? num2 : num * Fun(num2, index - 1);
        var results = Fun(num, Abs(index));
        return index > 0 ? results : 1 / results;

        /*注释：求幂的约定：
           1.任何不等于零的数的零次幂都等于1
           2.任何不等于零的数的-p（p是正整数）次幂，等于这个数的p次幂的倒数。*/
    }
    #endregion
    #region 整除运算
    /// <summary>
    /// 将两个对象a和b相除，并返回一个元组，
    /// 元组的第一个项是相除得出的商（只保留整数），第二个项是余数，第三个项是是否除尽
    /// </summary>
    /// <param name="numa">对象a，可以是任何能够同时进行减法，取余，除法运算的类型</param>
    /// <param name="numb">对象b，可以是任何能够同时进行减法，取余，除法运算的类型</param>
    /// <returns></returns>
    public static (Num Divisor, Num Remainder, bool IsDivisor) Div(Num numa, Num numb)
    {
        var dic = numa % numb;
        var rem = (numa - dic) / numb;
        return (rem, dic, dic == 0);
    }
    #endregion
    #region 求最大公约数
    /// <summary>
    /// 求多个整数的最大公约数
    /// </summary>
    /// <param name="first">第一个整数</param>
    /// <param name="second">第二个整数</param>
    /// <param name="num">如果还有更多的整数，则在这里传入</param>
    /// <returns></returns>
    public static Num GCD(Num first, Num second, params Num[] num)
        => num.Concat(new[] { first, second }).Sort(false).Aggregate((a, b) =>
             {
                 //注意：本方法使用的算法是辗转相除法

                 #region 本地函数
                 static Num Fun(Num max, Num min)
                 {
                     if (max < min)
                         (min, max) = (max, min);
                     return min == 0 ? max : Fun(min, max % min);
                 }
                 #endregion
                 return Fun(a, b);
             });

    #endregion
    #region 求最小公倍数
    /// <summary>
    /// 求多个整数的最小公倍数
    /// </summary>
    /// <param name="first">第一个整数</param>
    /// <param name="second">第二个整数</param>
    /// <param name="num">如果还有更多的整数，则在这里传入</param>
    /// <returns></returns>
    public static Num LCM(Num first, Num second, params Num[] num)
        => num.Concat(new[] { first, second }).Aggregate(
            (x, y) => x * y / GCD(x, y));
    #endregion
    #region 返回两个数字的比
    /// <summary>
    /// 以整数形式返回两个非零数字的比
    /// </summary>
    /// <param name="numA">数字A</param>
    /// <param name="numB">数字B</param>
    /// <returns></returns>
    public static (int NumA, int NumB) Proportion(Num numA, Num numB)
    {
        #region 本地函数
        static int Fun(Num num, int pre)
            => num * Pow(10, pre);
        #endregion
        Num pow = Limit(true, Pre(numA).Decimal, Pre(numB).Decimal);
        var (a, b) = (Fun(numA, pow), Fun(numB, pow));                  //首先将两个数字全部化成比例相等的整数
        var gcd = GCD(a, b);                                         //然后求出它们的最大公约数
        return (a / gcd, b / gcd);                                                      //再除以最大公约数，得出比值
    }
    #endregion
    #endregion
    #region 关于转化数字
    #region 求数字的绝对值
    /// <summary>
    /// 求一个数字的绝对值
    /// </summary>
    /// <param name="num">要求绝对值的数字</param>
    /// <returns></returns>
    public static Num Abs(Num num)
        => num >= 0 ? num : -num;
    #endregion
    #region 将一个数转化为正数或负数
    /// <summary>
    /// 将一个数字转化为正数或负数
    /// </summary>
    /// <param name="num">要转化的数字</param>
    /// <param name="toPositive">指示转化的行为，如果为<see langword="true"/>，转化为正数，
    /// 为<see langword="false"/>，转化为负数，为<see langword="null"/>，则将正负数取反</param>
    /// <returns></returns>
    public static Num Reve(Num num, bool? toPositive)
    {
        var isPositive = num > 0;                              //判断数字是否是正数
        var toPositive2 = toPositive ?? !isPositive;        //判断应该转化为正数还是负数
        return isPositive == toPositive2 ? num : -num;
    }
    #endregion
    #region 分割数字
    /// <summary>
    /// 将一个数字按照权重分割
    /// </summary>
    /// <param name="num">待分割的数字</param>
    /// <param name="weight">分割数字的权重</param>
    /// <returns></returns>
    public static Num[] Segmentation(Num num, params Num[] weight)
    {
        var atomic = num / weight.Sum();
        return weight.Select(x => atomic * x).ToArray();
    }
    #endregion
    #region 将数字的整数部分和小数部分拆分
    /// <summary>
    /// 将一个数字的整数部分和小数部分拆分，并返回一个元组，
    /// 第一个项是整数部分，第二个项是小数部分，
    /// 第三个项是小数部分是否等于0，也就是这个数字是不是整数
    /// </summary>
    /// <param name="num">要拆分的数字</param>
    /// <returns></returns>
    public static (Num Integer, Num Decimal, bool IsInt) Split(Num num)
    {
        var remainder = num % 1;
        return (num - remainder, remainder, remainder == 0);
    }
    #endregion
    #endregion
    #region 关于近似值和精度
    #region 返回数字的整数位数和小数位数
    /// <summary>
    /// 返回一个元组，指示一个数字的整数位数和小数位数
    /// </summary>
    /// <param name="num">要检查的数字</param>
    /// <returns></returns>
    public static (int Integer, int Decimal) Pre(Num num)
    {
        var (i, d, isInt) = Split(Reve(num, true));
        var integer = i == 0 ? 1 : Math.Log10(i) + 1;
        #region 求小数位数的本地函数
        int Fun()
        {
            for (int @decimal = 1; true; @decimal++)
                if (Split(d *= 10).IsInt)
                    return @decimal;
        }
        #endregion
        return ((int)integer, isInt ? 0 : Fun());
    }
    #endregion
    #region 将数字抹平
    /// <summary>
    /// 将数字在指定位数抹平，
    /// 并将后面的位数用0填补，然后返回一个元组，
    /// 分别是抹平后的数字和被抹掉的零头
    /// </summary>
    /// <param name="num">要抹平的数字</param>
    /// <param name="digits">如果这个值为正数，代表抹平N位整数，
    /// 为负数，代表抹平N位小数精度</param>
    /// <returns></returns>
    public static (Num Num, Num Remainder) Fla(Num num, int digits)
    {
        var remainder = num % Pow(10, digits > 0 ? digits - 1 : digits);        //需要这个三元表达式的原因在于：10的0次方才是1，而不是1次方
        return (num - remainder, remainder);

        /*注意：
           如果抹平的整数位数大于数字的整数位数，
           则会返回0，例如：将100抹平4位整数，就会返回0，
           但抹平小数位数时不会发生这种情况*/
    }
    #endregion
    #region 对数字取近似值
    /// <summary>
    /// 求一个数字的近似值
    /// </summary>
    /// <param name="num">需要求近似值的数字</param>
    /// <param name="pre">如果这个值为正数或0，代表取N位整数精度，
    /// 为负数，代表取N位小数精度</param>
    /// <param name="isProgressive">如果这个值为<see langword="true"/>，用进一法取近似值，
    /// 为<see langword="false"/>，用去尾法，为<see langword="null"/>，用四舍五入法</param>
    /// <returns></returns>
    public static Num Sim(Num num, int pre = 0, bool? isProgressive = true)
    {
        #region 本地函数
        static bool Fun(Num rem, bool? mod)
        {
            var text = rem.ToString().TrimStart('0', '.').First();
            var num2 = Convert.ToInt32(text.ToString());                //获取第一位非0非小数点字符
            return mod == true ?                                    //在这里不需要判断是否为去尾法，因为在Sim中已经判断过
                num2 != 0 : num2 >= 5;
        }
        #endregion
        var (fla, rem) = Fla(num, pre);            //将数字抹平
        if (fla == 0 || rem == 0)                           //如果数字的位数比要求的精度要低，则直接返回，不需要取近似值
            return num;
        return isProgressive == false || !Fun(rem, isProgressive) ?        //如果用去尾法取近似值，或经过判断后应该去尾
            fla : fla + Pow(10, pre > 0 ? pre - 1 : pre);                                                     //则返回去尾后的近似值，否则返回进1后的近似值
    }
    #endregion
    #endregion
    #region 关于比较
    #region 返回极限
    /// <summary>
    /// 返回若干个对象的极限，也就是它们当中最大或最小的值
    /// </summary>
    /// <typeparam name="Obj">要返回极限的对象</typeparam>
    /// <param name="returnMax">如果这个值为<see langword="true"/>，返回集合的最大值，为<see langword="false"/>，返回最小值</param>
    /// <param name="objs">要返回极限的对象</param>
    /// <returns></returns>
    public static Obj Limit<Obj>(bool returnMax, params Obj[] objs)
        => objs.Limit(returnMax);
    #endregion
    #endregion
    #region 关于相对值
    /// <summary>
    /// 设两个数字A和B，
    /// 如果要求返回绝对值，则直接返回B，
    /// 如果要求返回相对值，则返回A+B
    /// </summary>
    /// <param name="numA">数字A</param>
    /// <param name="numB">数字B，如果为0，在相对值下代表和数字A相等</param>
    /// <param name="isAbs">如果这个值为<see langword="true"/>，则返回绝对值，为<see langword="false"/>，则返回相对值</param>
    /// <returns></returns>
    public static Num Rel(Num numA, Num numB, bool isAbs)
        => isAbs ? numB : numA + numB;
    #endregion
    #region 使数字处于区间中
    /// <summary>
    /// 如果一个数字小于区间的最小值，则返回最小值，
    /// 大于区间的最大值，则返回最大值，
    /// 正好位于区间中，则返回它本身
    /// </summary>
    /// <param name="num">待检查的数字</param>
    /// <param name="interval">对数字的区间约束</param>
    /// <returns></returns>
    public static Num InInterval(Num num, IIntervalSpecific<Num> interval)
        => interval.CheckInInterval(num) switch
        {
            IntervalPosition.Insufficient => interval.Min!.Value,
            IntervalPosition.Overflow => interval.Max!.Value,
            IntervalPosition.Located => num,
            var e => throw new NotSupportedException($"无法识别{e}类型的枚举")
        };
    #endregion
}
