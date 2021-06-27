using System.IOFrancis;
using System.IOFrancis.FileSystem;
using System.Linq;

namespace System
{
    /// <summary>
    /// 这个类型的方法仅用于测试
    /// </summary>
    static class Test
    {
        #region 计算代码行数
        public static int CodeLine()
        {
            var readString = CreateIO.ObjReadString();
            return CreateIO.Directory(@"‪C:\CosyNest").SonAll.OfType<IFile>().
                Where(x => x.NameExtension is "cs" or "html" or "cshtml" or "razor").
                Sum(x => readString(x.GetBitPipe()).Linq(c => c.Count()).Result);
        }
        #endregion
        #region 计算类型数量
        public static int TypeCount()
            => CreateIO.Directory(@"‪C:\CosyNest").SonAll.OfType<IFile>().
            Where(x => x.NameExtension is "cs").Count();
        #endregion
    }
}
