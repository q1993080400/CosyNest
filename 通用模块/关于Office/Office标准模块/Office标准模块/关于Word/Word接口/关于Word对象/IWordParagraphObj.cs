namespace System.Office.Word
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个封装Office对象的段落
    /// </summary>
    /// <typeparam name="Obj">Word段落所封装的对象类型</typeparam>
    public interface IWordParagraphObj<out Obj> : IWordParagraph, IOfficeObj<Obj>
    {
        #region 删除Word段落
        /// <summary>
        /// 将这个Word段落删除
        /// </summary>
        new void Delete();
        #endregion
    }
}
