using System.IOFrancis;
using System.IOFrancis.FileSystem;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ViewDependencies;

/// <summary>
/// 查找所有模块，并分析它们
/// </summary>
public static class FindModule
{
    #region 公开成员
    #region 获取所有模块
    /// <summary>
    /// 获取所有模块的信息
    /// </summary>
    /// <param name="path">存放所有模块的目录</param>
    /// <returns></returns>
    public static Module[] GetModules(string path)
    {
        var moduleDictionary = new Dictionary<string, Module>();
        var files = CreateIO.Directory(path).SonAll.OfType<IFile>().
            Where(x => x.NameExtension is "csproj");
        var xmlDictionary = files.Select(x =>
            {
                var xml = XElement.Load(x.Path);
                return (x.NameSimple, xml);
            }).ToArray().ToDictionary(true);
        var modules = AnalysisXML(xmlDictionary, moduleDictionary).ToArray();
        return modules;
    }
    #endregion
    #endregion
    #region 内部成员
    #region 解析模块
    /// <summary>
    /// 解析XML文档，并将其转换为模块
    /// </summary>
    /// <param name="xmlCache">用模块的名称索引XML文档的字典</param>
    /// <param name="moduleCache">用模块的名称索引模块的字典</param>
    /// <returns></returns>
    private static IEnumerable<Module> AnalysisXML(IReadOnlyDictionary<string, XElement> xmlCache, Dictionary<string, Module> moduleCache)
    {
        foreach (var (name, _) in xmlCache)
        {
            yield return AnalysisXMLSingle(name, xmlCache, moduleCache);
        }
    }
    #endregion
    #region 解析单个模块
    /// <summary>
    /// 解析单个模块，并返回解析结果
    /// </summary>
    /// <param name="name">要解析的模块的名称</param>
    /// <returns></returns>
    /// <inheritdoc cref="AnalysisXML(IReadOnlyDictionary{string, XElement}, Dictionary{string, Module})"/>
    private static Module AnalysisXMLSingle(string name, IReadOnlyDictionary<string, XElement> xmlCache, Dictionary<string, Module> moduleCache)
    {
        if (moduleCache.TryGetValue(name, out var cache))
            return cache;
        var xml = xmlCache[name];
        var projectReference = xml.Descendants("ProjectReference").Select(x => x.Attribute("Include")?.Value).
            Where(x => x is { }).
            Select(x =>
            {
                var reference = Regex.MatcheSingle(x!) ??
                throw new NotSupportedException($"模块{name}的ProjectReference中的{x}模块引用语法可能不正确");
                return reference["reference"].Match;
            }).ToArray();
        var son = projectReference.Select(x => AnalysisXMLSingle(x, xmlCache, moduleCache)).ToHashSet();
        var module = new Module()
        {
            Name = name,
            Reference = son,
            ReferenceDepth = (son.MaxBy(x => x.ReferenceDepth)?.ReferenceDepth ?? -1) + 1
        };
        moduleCache[name] = module;
        return module;
    }
    #endregion
    #region 用来解析项目引用的正则表达式
    /// <summary>
    /// 解析项目引用的正则表达式
    /// </summary>
    private static IRegex Regex { get; }
        = /*language=regex*/@"(?<reference>[^\\]+).csproj$".Op().Regex();
    #endregion
    #endregion
}
