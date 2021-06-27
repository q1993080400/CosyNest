using System.Collections.Generic;
using System.Linq;

namespace System.Maths
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以用来生成随机数
    /// </summary>
    public interface IRandom
    {
        #region 说明文档
        /*实现本接口请遵循以下规范：
          #本接口的所有API都应该是线程安全的，
          因为随机数生成器一般是静态属性，
          而且很可能在多个线程中访问它们*/
        #endregion
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
        /// <param name="max">随机数的最大值</param>
        /// <returns></returns>
        Num RandRange(Num min, Num max)
        {
            if (min > max)
                Tool.Exchange(ref min, ref max);
            return min + (max - min) * Rand();
        }
        #endregion
        #region 有关集合的随机数
        #region 返回随机索引
        /// <summary>
        /// 返回一个集合的随机合法索引，
        /// 不考虑集合中没有任何元素的情况
        /// </summary>
        /// <typeparam name="Obj">集合元素的类型</typeparam>
        /// <param name="collections">要返回索引的集合</param>
        /// <returns></returns>
        int RandIndex<Obj>(IEnumerable<Obj> collections)
            => RandRange(0, collections.Count());
        #endregion
        #region 返回随机元素
        /// <summary>
        /// 返回一个集合的随机元素，
        /// 如果该集合没有元素，则会引发异常
        /// </summary>
        /// <typeparam name="Obj">集合元素的类型</typeparam>
        /// <param name="collections">要返回元素的集合</param>
        /// <returns></returns>
        Obj RandElements<Obj>(IEnumerable<Obj> collections)
            => collections.ElementAt(RandIndex(collections));
        #endregion
        #endregion
        #region 生成随机字符串
        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="minLength">生成字符串的最小长度</param>
        /// <param name="maxLength">生成字符串的最大长度</param>
        /// <param name="category">这个数组枚举允许生成的字符所在区间</param>
        /// <returns></returns>
        string RandText(int minLength, int maxLength, params IIntervalSpecific<char>[] category)
        {
            if (category.Any(x => !x.IsClosed))
                throw new Exception($"{nameof(category)}中的全部元素必须为封闭区间");
            category = category.Any() ? category : new[] { IInterval.Create((char?)ushort.MinValue, (char?)ushort.MaxValue) };      //如果不指定合法字符范围，则默认可以生成任何UTF16字符
            int len = RandRange(minLength, maxLength);
            var charts = new char[len];
            for (int i = 0; i < len; i++)
            {
                var (beg, end) = RandElements(category);
                charts[i] = (char)RandRange(beg!.Value, end!.Value);
            }
            return new string(charts);
        }
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
    }
}
