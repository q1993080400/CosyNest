using System.Collections;
using System.IOFrancis;
using System.IOFrancis.FileSystem;

using IWshRuntimeLibrary;

using File = System.IO.File;

namespace System.Underlying.PC;

/// <summary>
/// 这个类型可以枚举，添加，删除自启项
/// </summary>
sealed class StartCollections : ICollection<string>
{
    #region 获取自动启动文件夹
    /// <summary>
    /// 该对象是程序/启动文件夹，
    /// 放在该文件夹中的所有文件在开机时都会自动运行
    /// </summary>
    private IDirectory AutomaticStart { get; }
    = CreateIO.Directory($@"C:\Users\{Environment.UserName}\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup");
    #endregion
    #region 关于集合本身
    #region 枚举自启项
    public IEnumerator<string> GetEnumerator()
        => AutomaticStart.SonAll.OfType<IOFrancis.FileSystem.IFile>().
        Where(static x => x.NameFull is not "desktop.ini").        //这个文件是用户个性化设置，它不会被枚举
        Select(static x =>
        {
            var path = x.Path;
            if (path.EndsWith(".lnk"))
            {
                var shell = new WshShell();
                var shortcut = (IWshShortcut)shell.CreateShortcut(path);
                return shortcut.TargetPath;
            }
            return path;
        }).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    #endregion
    #region 自启项数量
    public int Count
    {
        get
        {
            var count = 0;
            foreach (var item in this)
            {
                count++;
            }
            return count;
        }
    }
    #endregion
    #region 是否只读
    public bool IsReadOnly => false;
    #endregion
    #region 是否存在自启项
    public bool Contains(string item)
    {
        foreach (var path in this)
        {
            if (item == path)
                return true;
        }
        return false;
    }
    #endregion
    #region 复制集合元素
    public void CopyTo(string[] array, int arrayIndex)
    {
        foreach (var item in this)
        {
            array[arrayIndex++] = item;
        }
    }
    #endregion
    #endregion
    #region 关于添加和删除自启项
    #region 添加自启项
    public void Add(string item)
    {
        var path = Path.Combine(AutomaticStart.Path, Path.GetFileName(item));
        CreateIO.File(item).Shortcut(path);
    }
    #endregion
    #region 删除自启项
    public bool Remove(string item)
    {
        if (!Contains(item))
            return false;
        var path = Path.Combine(AutomaticStart.Path, Path.GetFileName(item));
        File.Delete(path);
        File.Delete(path + ".lnk");
        return true;
    }
    #endregion
    #region 清除所有自启项
    public void Clear()
        => this.ToArray().ForEach(File.Delete);
    #endregion 
    #endregion
}
