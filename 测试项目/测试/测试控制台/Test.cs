using System.IOFrancis;
using System.IOFrancis.FileSystem;

namespace System;

/// <summary>
/// 这个类型的方法仅用于测试
/// </summary>
static class Test
{
    #region 计算代码行数
    public static int CodeLine(string path = @"C:\CosyNest")
    {
        var readString = CreateIO.ObjReadString();
        return CreateIO.Directory(path).SonAll.OfType<IFile>().
            Where(x => x.NameExtension is "cs" or "html" or "cshtml" or "razor" or "razor.cs" or "css").
            Sum(x => readString(x.GetBitPipe().Read).Linq(c => c.Count()).Result);
    }
    #endregion
    #region 计算类型数量
    public static int TypeCount(string path = @"‪C:\CosyNest")
        => CreateIO.Directory(path).SonAll.OfType<IFile>().
        Where(x => x.NameExtension is "cs").Count();
    #endregion
    #region 打包部署文件
    public static async Task Pack()
    {
        var d = CreateIO.Directory(@"C:\工作\部署");
        foreach (var item in d.SonAll.OfType<IIO>())
        {
            if (item is IFile { NameExtension: not ("aspx" or "dll"), NameFull: not ("Unity.config" or "license.key") })
            {
                try
                {
                    item.Delete();
                }
                catch (Exception)
                {

                }
            }
        }
        CreateIO.Directory(@"C:\工作\部署\App_GlobalResources", false).Delete();
        File.Delete(@"C:\工作\部署.zip");
        await using var io = CreateIO.Compressed(@"C:\工作\部署.zip");
        io.RootDirectory.Add(d);
    }
    #endregion
}
