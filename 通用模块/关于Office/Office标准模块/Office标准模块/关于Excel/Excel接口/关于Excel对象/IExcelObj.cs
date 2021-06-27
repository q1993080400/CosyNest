using System.Maths;

namespace System.Office.Excel
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个Excel对象
    /// </summary>
    /// <typeparam name="Obj">Excel对象所封装的对象类型</typeparam>
    public interface IExcelObj<out Obj> : IOfficeObj<Obj>
    {
        #region 对象所在的工作表
        /// <summary>
        /// 获取对象所在的工作表
        /// </summary>
        IExcelSheet Sheet { get; }
        #endregion
        #region 对象的位置
        /// <summary>
        /// 获取或设置该Excel对象的位置
        /// </summary>
        IPoint Position { get; set; }
        #endregion
    }
}
