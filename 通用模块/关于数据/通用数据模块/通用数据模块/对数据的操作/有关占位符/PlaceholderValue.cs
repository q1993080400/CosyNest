namespace System.DataFrancis
{
    /// <summary>
    /// 代表一个值占位符，
    /// 它不是真正从<see cref="IData"/>中获取的值，
    /// 只是为了方便编写表达式而设计的占位符
    /// </summary>
    public sealed class PlaceholderValue
    {
        #region 运算符重载
        public static PlaceholderValue operator +(PlaceholderValue a, dynamic b)
            => new((dynamic?)a.Value + b);
        public static PlaceholderValue operator -(PlaceholderValue a, dynamic b)
            => new((dynamic?)a.Value - b);
        public static PlaceholderValue operator *(PlaceholderValue a, dynamic b)
            => new((dynamic?)a.Value * b);
        public static PlaceholderValue operator /(PlaceholderValue a, dynamic b)
            => new((dynamic?)a.Value / b);
        public static PlaceholderValue operator %(PlaceholderValue a, dynamic b)
            => new((dynamic?)a.Value % b);
        public static PlaceholderValue operator -(PlaceholderValue a)
            => new(-(dynamic?)a.Value);
        public static bool operator ==(PlaceholderValue a, dynamic b)
            => Equals(a.Value, b);
        public static bool operator !=(PlaceholderValue a, dynamic b)
            => !Equals(a.Value, b);
        public static bool operator >=(PlaceholderValue a, dynamic b)
            => (dynamic?)a.Value >= b;
        public static bool operator <=(PlaceholderValue a, dynamic b)
            => (dynamic?)a.Value <= b;
        public static bool operator >(PlaceholderValue a, dynamic b)
            => (dynamic?)a.Value > b;
        public static bool operator <(PlaceholderValue a, dynamic b)
            => (dynamic?)a.Value < b;
        #endregion
        #region 重写的方法
        #region 重写GetHashCode
        public override int GetHashCode()
            => Value?.GetHashCode() ?? 0;
        #endregion
        #region 重写Equals
        public override bool Equals(object? obj)
            => Equals(Value, obj);
        #endregion
        #endregion
        #region 占位符封装的对象
        /// <summary>
        /// 获取占位符封装的数据的值
        /// </summary>
        internal object? Value { get; }
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的值初始化占位符
        /// </summary>
        /// <param name="value">占位符封装的数据的值</param>
        public PlaceholderValue(object? value)
        {
            this.Value = value;
        }
        #endregion
    }
}
