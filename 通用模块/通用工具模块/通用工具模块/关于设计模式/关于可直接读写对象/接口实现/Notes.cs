using System.Collections;
using System.Design.Direct;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace System.Design;

/// <summary>
/// 该类型是<see cref="IDirect"/>的实现，
/// 可以视为一个记事
/// </summary>
sealed class Notes : IDirect
{
    #region 说明文档
    /*问：什么是记事？
      答：记事是一个非常轻量级的配置文件，
      它只支持键值对模式，当写入它的键值对时，
      会立即将其保存在本地，它适用于以下情况：
      需要储存一些非常小，且不会被频繁写入的数据，
      不值得为此使用数据库*/
    #endregion
    #region 接口实现
    #region 复制
    public IDirect Copy(bool copyValue = true, Type? type = null)
    {
        throw new NotImplementedException();
    }
    #endregion
    #region 读写键值对
    public object? this[string key]
    {
        get => Dictionary[key];
        set
        {
            Dictionary[key] = value?.ToString();
            SetNotes();
        }
    }
    #endregion
    #region 检查键值对是否存在
    public bool ContainsKey(string key)
        => Dictionary.ContainsKey(key);
    #endregion
    #region 读取键值对（不会引发异常）
    public bool TryGetValue(string key, [MaybeNullWhen(false)] out object? value)
    {
        var ex = Dictionary.TryGetValue(key, out var text);
        value = text;
        return ex;
    }
    #endregion
    #region 获取键集合
    public IEnumerable<string> Keys => Dictionary.Keys;
    #endregion
    #region 获取值集合
    public IEnumerable<object?> Values => Dictionary.Values;
    #endregion
    #region 获取键值对数量
    public int Count => Dictionary.Count;
    #endregion
    #region 枚举键值对
    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
        => Dictionary.Select(x => new KeyValuePair<string, object?>(x.Key, x.Value)).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    #endregion
    #region 返回Json字符串
    public string Json
        => JsonSerializer.Serialize<IReadOnlyDictionary<string, object?>>(this);
    #endregion
    #endregion
    #region 私有成员
    #region 记事所在的路径
    /// <summary>
    /// 获取用来读取或写入记事的路径
    /// </summary>
    private string Path { get; }
    #endregion
    #region 用来存储记事的字典
    /// <summary>
    /// 获取一个字典，它存储记事的内容
    /// </summary>
    private IDictionary<string, string?> Dictionary { get; }
    #endregion
    #region 辅助方法：写入记事
    /// <summary>
    /// 辅助方法，重新写入记事到文件中
    /// </summary>
    private void SetNotes()
    {
        var text = Dictionary.Join(x => $"{x.Key}={x.Value}", Environment.NewLine);
        ToolIO.WriteAllText(Path, text);
    }
    #endregion
    #endregion 
    #region 构造函数
    /// <summary>
    /// 使用指定的路径初始化对象
    /// </summary>
    /// <param name="path">记事所在的路径</param>
    public Notes(string path)
    {
        Path = path;
        if (File.Exists(Path))
        {
            var text = File.ReadAllText(Path);
            Dictionary = ToolRegex.KeyValuePairExtraction(text, Environment.NewLine).ToDictionary(true)!;
        }
        else Dictionary = new Dictionary<string, string?>();
    }
    #endregion
}
