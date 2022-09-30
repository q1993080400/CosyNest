using System.IOFrancis;
using System.IOFrancis.Bit;
using System.Maths;

namespace System;

/// <summary>
/// 这个静态类用于演示本框架的IO操作
/// </summary>
public static class DemoIO
{
    #region 需求描述
    /*生成一个长度从256字节到1024字节不等的流或管道，
      里面填满了随机数据，并将它们写入两个文件*/
    #endregion
    #region 使用本框架实现的版本
    /// <summary>
    /// 这是该需求使用本框架实现的版本
    /// </summary>
    /// <returns></returns>
    public static async Task DemoCosyNest()
    {
        #region 用来枚举数据的本地函数
        static IEnumerable<byte> GetData()
        {
            var rand = CreateBaseMath.Random();     //创建随机数生成器
            for (int i = 0, length = rand.RandRange(256, 1024); i < length; i++)    //确定数据的长度，并生成数据
            {
                yield return (byte)rand.RandRange(0, 255);
            }
        }
        #endregion
        IBitRead read = GetData().ToBitRead();        //随机数据流已经生成成功
        IBitWrite file1 = CreateIO.FullDuplexFile("file1.txt").Write;     //创建文件1
        IBitWrite file2 = CreateIO.FullDuplexFile("file2.txt").Write;     //创建文件2
        Task task1 = Task.Run(() => file1.Write(read.Read()));      //将一个管道同时写入两个文件以提高性能
        Task task2 = Task.Run(() => file2.Write(read.Read()));
        await Task.WhenAll(new[] { task1, task2 });

        /*将生成的随机数据写入这两个文件，与原生的Stream相比，它具有以下技术优势：
          1.每次读取read对象返回的都是不同的的数据，
          所以数据不用重新生成，也不用在每次读取完之后将read恢复到开始位置
          2.Read方法是一个纯函数，没有线程安全问题，如你所见，它甚至可以被同时写入两个文件，
          事实上，只需要一个IBitRead对象，就可以满足整个应用的随机数据生成需求
          3.在将IBitRead的数据写入IBitWrite的时候，不需要分割缓冲区，
          但是对性能没有严重影响，因为IBitWrite在底层仍然使用缓冲区来提高性能，只是外界感觉不到
          4.这三个对象都不需要手动释放，它们在底层自动管理这个过程*/
    }
    #endregion
}
