using System.Collections;
using System.Linq;

namespace System.Office.Excel
{
    /// <summary>
    /// 这个类型表示Excel单元格的值
    /// </summary>
    public readonly struct RangeValue
    {
        #region 说明文档
        /*说明文档：
           ExcelRange的Value一般是object，
           但本框架则使用这个类型，原因在于：

           虽然Excel是弱类型的模型，
           但是Value的合法类型实际上就只有四种：
           文本，数字，日期和数组，使用这个类型可以屏蔽不合法的输入，
           并且在互相转换的时候会更加方便*/
        #endregion
        #region 隐式转换
        #region 从String转换
        public static implicit operator RangeValue(string? value)
            => new(value);
        #endregion
        #region 从Num转换
        public static implicit operator RangeValue(Num value)
            => new(value);
        #endregion
        #region 从Double转换
        public static implicit operator RangeValue(double value)
            => new(value);
        #endregion
        #region 从DateTime转换
        public static implicit operator RangeValue(DateTime value)
            => new(value);
        #endregion
        #region 从DateTimeOffset转换
        public static implicit operator RangeValue(DateTimeOffset d)
            => new(d);
        #endregion
        #region 从数组转换
        public static implicit operator RangeValue(Array? value)
            => new(value);
        #endregion
        #endregion
        #region 值的内容
        private readonly object? ContentField;

        /// <summary>
        /// 储存单元格Value的实际值
        /// </summary>
        public object? Content
        {
            get => ContentField;
            init
            {
                #region 转换数组的本地函数
                static Array ConvertArray(Array array)
                {
                    switch (array.Rank)
                    {
                        case 1:
                            return array.OfType().Select(Convert).ToArray();
                        case 2:
                            var length = array.GetLength();
                            var newArray = new object?[length[0], length[1]];
                            newArray.ForEach((col, row, _) => newArray[col, row] = Convert(array.GetValue(col, row)));
                            return newArray;
                        case var rank:
                            throw new NotSupportedException($"最多支持二维数组，但是传入了一个{rank}维数组");
                    }
                }
                #endregion
                #region 转换并检查类型的本地函数
                static object? Convert(object? content)
                    => content switch
                    {
                        null or string or int or double or DateTime => content,
                        Array a => ConvertArray(a),
                        Num n => (double)n,
                        DateTimeOffset d => d.DateTime,
                        IEnumerable list => ConvertArray(list.ToArray<object>()),
                        _ => throw new ExceptionTypeUnlawful(content,
                        typeof(string), typeof(Num), typeof(double), typeof(DateTime),
                        typeof(Array), typeof(DateTimeOffset), typeof(IEnumerable)),
                    };
                #endregion
                ContentField = Convert(value);
            }
        }
        #endregion
        #region 解释值
        #region 解释为数组
        /// <summary>
        /// 如果这个单元格值是数组，则返回数组，
        /// 否则返回<see langword="null"/>
        /// </summary>
        public Array? ToArray
            => Content as Array;

        /*问：此处为什么不直接使用Object[]，
          而是返回数组的基类Array？
          答：因为单元格值返回的数组不一定是一维的，
          还有可能是二维的*/
        #endregion
        #region 解释为文本
        /// <summary>
        /// 将这个单元格值解释为<see cref="string"/>
        /// </summary>
        public string ToText
            => Content?.ToString() ?? "";
        #endregion
        #region 解释为数字
        /// <summary>
        /// 如果这个单元格值是数字，或可以转换为数字，
        /// 则返回它， 否则返回<see langword="null"/>
        /// </summary>
        public double? ToDouble
            => Content.To<double?>(false);
        #endregion
        #region 解释为日期
        /// <summary>
        /// 如果单元格值是日期，则返回日期，
        /// 否则返回<see langword="null"/>
        /// </summary>
        public DateTime? ToDateTime
            => Content.To<DateTime?>(false);
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 将指定的内容封装进对象
        /// </summary>
        /// <param name="content">单元格Value的实际值</param>
        public RangeValue(object? content)
        {
            ContentField = null;
            Content = content;
        }
        #endregion
        #region 重写ToString
        public override string ToString()
            => ToText;
        #endregion
    }
}
