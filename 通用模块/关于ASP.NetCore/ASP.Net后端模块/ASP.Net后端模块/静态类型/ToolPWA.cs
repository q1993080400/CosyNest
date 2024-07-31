using System.Design.Direct;
using System.Design;
using System.Text.Json;
using System.TreeObject.Json;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个类型提供对PWA专用的Manifest文件的访问
/// </summary>
public static class ToolPWA
{
    #region PWA键
    #region PWA版本
    /// <summary>
    /// 获取一个键，它表示PWA的版本
    /// </summary>
    public const string KeyVersion = "version";
    #endregion
    #endregion
    #region 获取Manifest文件中的内容
    /// <summary>
    /// 获取Manifest文件中的内容
    /// </summary>
    /// <param name="manifestPath">manifest.webmanifest文件的位置</param>
    /// <returns></returns>
    public static IDirect GetManifest(string manifestPath = @"wwwroot\manifest.webmanifest")
    {
        var text = File.ReadAllText(manifestPath);
        InitializationToolTreeObject.Initialization();
        var options = CreateDesign.JsonCommonOptions();
        return JsonSerializer.Deserialize<IDirect>(text, options)!;
    }
    #endregion
    #region 更新Manifest文件中的内容
    /// <summary>
    /// 更新Manifest文件中的内容
    /// </summary>
    /// <param name="update">一个用于更新manifest对象的委托，
    /// 它的参数就是更新前的manifest对象</param>
    /// <param name="manifestPath">manifest.webmanifest文件的位置</param>
    public static void UpdateManifest(Action<IDirect> update, string manifestPath = @"wwwroot\manifest.webmanifest")
    {
        var manifest = GetManifest(manifestPath);
        update(manifest);
        var options = CreateDesign.JsonCommonOptions();
        options.WriteIndented = true;
        var json = JsonSerializer.Serialize(manifest, options);
        File.WriteAllText(manifestPath, json);
    }
    #endregion
    #region 更新PWA版本
    /// <summary>
    /// 以当日日期作为种子，更新一个PWA的版本
    /// </summary>
    /// <param name="manifestPath">manifest.webmanifest文件的位置</param>
    public static void UpdatePWAVersion(string manifestPath = @"wwwroot\manifest.webmanifest")
    {
#if DEBUG
        UpdateManifest(manifest =>
        {
            var now = DateTime.Now.ToString("yyyy.MM.dd");
            var version = manifest.TryGetValue(KeyVersion).Value?.ToString();
            var index = version?[^4..].To<int>() ?? 0;
            var newVersion = $"{now}.{index + 1:D4}";
            manifest[KeyVersion] = newVersion;
        }, manifestPath);
#endif
    }
    #endregion
}
