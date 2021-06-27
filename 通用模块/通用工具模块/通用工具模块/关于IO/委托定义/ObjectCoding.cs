using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.IOFrancis.Bit
{
    #region 说明文档
    /*问：在本框架的早期版本，
      作者曾经打算使用翻译和验证来抽象大多数的IO操作，
      为什么这种设计现在被放弃了？
      答：因为作者认为，它们存在的问题和指针比较类似，
      那就是太过灵活，用处太多，同时也太过抽象，难以理解，
      作者身边试用的人普遍表示，无法掌握这种技巧
    
      本框架非常强调低耦合，但是作者认为，
      对低耦合正确的理解应该是对同一件事可以用无数种方法来完成，
      而不是某一种方法可以完成无数的事，否则岂不是所有的程序都可以用Main方法来抽象？
      它的职责不够明确，对方便维护没有帮助，只会增加用户的心智负担*/
    #endregion
    #region 读取对象
    /// <summary>
    /// 这个委托可以从二进制管道中读取对象，
    /// 它比直接读取字节数组更加高层
    /// </summary>
    /// <typeparam name="Obj">要读取的对象类型</typeparam>
    /// <param name="bitRead">用来获取二进制数据的管道</param>
    /// <returns></returns>
    public delegate IAsyncEnumerable<Obj> ObjRead<out Obj>(IBitRead bitRead);
    #endregion
    #region 写入对象
    /// <summary>
    /// 这个委托可以向二进制管道写入对象，
    /// 它比直接写入字节数组更加高层
    /// </summary>
    /// <typeparam name="Obj">要写入的对象类型</typeparam>
    /// <param name="bitWrite">要写入对象的二进制管道</param>
    /// <param name="obj">要写入的对象</param>
    /// <returns></returns>
    public delegate Task ObjWrite<in Obj>(IBitWrite bitWrite, Obj obj);
    #endregion
    #region 编码对象
    /// <summary>
    /// 这个委托可以将对象编码为二进制数据
    /// </summary>
    /// <typeparam name="Obj">要编码的对象类型</typeparam>
    /// <param name="obj">要编码的对象</param>
    /// <returns>用来读取编码后二进制数据的管道</returns>
    public delegate IBitRead ObjCoding<in Obj>(Obj obj);
    #endregion
    #region 解码对象
    /// <summary>
    /// 这个委托可以将二进制数据解码为对象
    /// </summary>
    /// <typeparam name="Obj">要解码的对象类型</typeparam>
    /// <param name="data">读取待解码二进制数据的管道</param>
    /// <returns></returns>
    public delegate IAsyncEnumerable<Obj> ObjDecoding<out Obj>(IBitRead data);
    #endregion
}
