namespace System.Office.Word
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个Word书签，
    /// 它可以在不断被修改的文本中找到原来的位置
    /// </summary>
    public interface IWordBookmark
    {
        #region 说明文档
        /*说明文档
          需要本类型的原因在于：
          对Word文档的很多操作都需要指定一个位置，
          这个位置是通过字符在全文中的索引来计算的，
          但是问题在于，文本经常被增加，删除和修改，
          这个位置也随之不断发生变化，因此需要一个书签来观察这种变化，
          让文本被改变的时候也能够找到原来的位置
           
          举例说明：
          假设有文本“我不爱你”，
          初始位置在索引2的“爱”，
          那么在把“不”字删除，文本变成“我爱你”以后，
          如果没有书签，索引2会变成“你”，
          但如果有书签，仍然能找到索引1处的“爱”*/
        #endregion
        #region 书签的位置
        /// <summary>
        /// 返回书签所处的位置
        /// </summary>
        int Pos { get; }
        #endregion
        #region 书签所在的文档
        /// <summary>
        /// 返回书签所在的文档
        /// </summary>
        IWordDocument Document { get; }
        #endregion
    }
}
