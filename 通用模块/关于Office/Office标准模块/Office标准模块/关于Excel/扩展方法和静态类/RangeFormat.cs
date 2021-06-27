namespace System.Office.Excel
{
    /// <summary>
    /// 这个静态类储存一些常用的Excel单元格数字格式
    /// </summary>
    public static class RangeFormat
    {
        #region 日期格式
        #region 根据区域发生变化
        /// <summary>
        /// 返回日期的常用格式，
        /// 会根据所在区域的不同发生变化
        /// </summary>
        public static string DateCommon { get; }
        = "mm-dd-yy";

        /*注释：
         1.经过测试，这个格式文本似乎不是月/日/年格式，
         而是会根据区域发生变化，例如在中国为年/月/日格式
         
         2.如果希望单元格的Value为DateTime类型，必须遵循这个格式
         
         3.设置这个格式后，如果是中国区域，底层实现为EPPlus，
         单元格的Text属性会直接变成日期的天数部分，而不是完整日期文本，
         其他区域和实现暂未测试，不知道结果是否相同*/
        #endregion
        #endregion
    }
}
