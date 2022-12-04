using System.Design.Direct;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.DataFrancis.Verify;
using System.ComponentModel;

namespace System.DataFrancis;

/// <summary>
/// 这个静态类可以用来帮助创建一些关于数据的对象
/// </summary>
public static class CreateDataObj
{
    #region 创建IData
    #region 用指定的列名和值
    /// <summary>
    /// 用指定的列名和值创建<see cref="IData"/>
    /// </summary>
    /// <param name="parameters">这个数组的元素是一个元组，
    /// 分别指示列的名称和值</param>
    public static IData Data(params (string Column, object? Value)[] parameters)
        => Data(parameters.ToDictionary(true));
    #endregion
    #region 用指定的列名
    /// <summary>
    /// 用指定的数据列名创建<see cref="IData"/>
    /// </summary>
    /// <param name="columnName">指定的列名，一经初始化不可增减</param>
    public static IData Data(params string[] columnName)
        => Data(columnName.Select(x => (x, (object?)null)).ToArray());
    #endregion
    #region 用指定的字典
    /// <summary>
    /// 用一个键是列名的键值对集合（通常是字典）创建<see cref="IData"/>
    /// </summary>
    /// <param name="dictionary">一个键值对集合，它的元素的键</param>
    /// <param name="copyValue">如果这个值为真，则会复制键值对的值，否则不复制</param>
    public static IData Data(IEnumerable<KeyValuePair<string, object?>> dictionary, bool copyValue = true)
        => new DataRealize(dictionary, copyValue);
    #endregion
    #region 将多条数据合并 
    /// <summary>
    /// 将一条数据和另一些数据合并，并返回合并后的新数据
    /// </summary>
    /// <param name="data">待合并的数据</param>
    /// <param name="dataMerge">待合并的另一些数据，
    /// 如果存在列名相同的数据，则以后面的数据为准</param>
    /// <returns></returns>
    public static IData DataMerge(IEnumerable<KeyValuePair<string, object?>> data, params (string Name, object? Value)[] dataMerge)
        => Data(data.Union
            (dataMerge.Select
            (x => x.ToKV())));
    #endregion
    #region 创建空数据
    /// <summary>
    /// 创建一条空数据，
    /// 它的数据可以稍后再写入
    /// </summary>
    /// <returns></returns>
    public static IData DataEmpty()
        => Data(new Dictionary<string, object?>());
    #endregion
    #endregion
    #region 创建序列化和反序列化对象
    #region 适用于IDirect
    /// <summary>
    /// 返回一个支持序列化和反序列化<see cref="IDirect"/>的对象，
    /// 它支持协变反序列化，除<see cref="IDirect"/>以外，
    /// 还可以反序列化<see cref="IData"/>以及实现<see cref="IDirect"/>，
    /// 并具有无参数构造函数的类型
    /// </summary>
    /// <returns></returns>
    public static JsonConverter JsonDirect { get; } = new JsonConvertFactoryDirect();
    #endregion
    #endregion
    #region 创建数据管道
    #region 使用查询表达式工厂
    /// <summary>
    /// 创建一个<see cref="IDataPipeFrom"/>，
    /// 它使用查询表达式工厂来创建查询，
    /// 通过它可以创建数据直接来自内存的管道
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="DataPipeFromFactory{Entity}"/>
    /// <inheritdoc cref="DataPipeFromFactory{Entity}.DataPipeFromFactory(Func{IQueryable{Entity}})"/>
    public static IDataPipeFrom PipeFromFactory<Entity>(Func<IQueryable<Entity>> factory)
        => new DataPipeFromFactory<Entity>(factory);
    #endregion
    #endregion
    #region 创建数据验证默认委托
    /// <summary>
    /// 对数据进行验证，
    /// 本方法通常会被赋值给<see cref="DataVerify"/>委托，
    /// 但是也可以直接调用
    /// </summary>
    /// <inheritdoc cref="DataVerify"/>
    public static (bool IsSuccess, IReadOnlyList<string> Message) DataVerifyDefault(object obj)
    {
        var message = new List<string>();
        var propertys = obj.GetTypeData().Propertys.
            Where(x => x.IsAlmighty() && x.HasAttributes<VerifyAttribute>() && !x.HasAttributes<NotMappedAttribute>());
        foreach (var item in propertys)
        {
            var describe = item.GetCustomAttribute<DescriptionAttribute>()?.Description;
            var attribute = item.GetCustomAttribute<VerifyAttribute>()!;
            if (attribute.Verify(item.GetValue(obj), describe) is (false, string m) verify)
                message.Add(m);
        }
        return (message.Count is 0, message);
    }
    #endregion
}
