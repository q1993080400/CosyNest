namespace System.Office
{
    /// <summary>
    /// 这个枚举描述Office对象的对齐方式
    /// </summary>
    public enum OfficeAlignment
    {
        /// <summary>
        /// 左对齐，在Excel单元格垂直对齐方式中，还可以代表靠上对齐
        /// </summary>
        LeftOrTop,
        /// <summary>
        /// 右对齐，在Excel单元格垂直对齐方式中，还可以代表靠下对齐
        /// </summary>
        RightOrBottom,
        /// <summary>
        /// 居中对齐
        /// </summary>
        Center,
        /// <summary>
        /// 两端对齐
        /// </summary>
        Ends,
        /// <summary>
        /// 代表该对齐方式未识别
        /// </summary>
        Unknown,
    }
}
