using System.Design.Direct;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Performance;

namespace System.Design;

/// <summary>
/// 这个静态类可以用来帮助创建和设计模式有关的类型
/// </summary>
public static class CreateDesign
{
    #region 创建独占任务对象
    /// <summary>
    /// 创建一个<see cref="IQueueTask"/>，
    /// 但是它实际上不进行排队，而是独占执行一个任务，
    /// 在前端开发中，可以用它来避免事件重复触发
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="QueueTaskExclusive(Action?, Func{Task}?)"/>
    public static IQueueTask QueueExclusive(Action? inSynchronizationLocking = null, Func<Task>? inAsynchronousLocking = null)
        => new QueueTaskExclusive(inSynchronizationLocking, inAsynchronousLocking);
    #endregion
    #region 创建单例对象
    private static ICache<Type, object> SingleCache { get; }
        = CreatePerformance.MemoryCache<Type, object>
        ((type, _) => type.GetTypeData().ConstructorCreate<object>());

    /// <summary>
    /// 返回一个类型的实例，
    /// 如果多次调用，不会重复创建
    /// </summary>
    /// <typeparam name="Obj">要返回实例的类型，
    /// 它必须要有无参数构造函数，但不一定要公开</typeparam>
    /// <returns></returns>
    public static Obj Single<Obj>()
        where Obj : class
        => (Obj)SingleCache[typeof(Obj)];
    #endregion
    #region 创建池化对象
    /// <summary>
    /// 创建一个最简单的<see cref="IPool{Obj}"/>实现，
    /// 它实际上不缓存对象，而是每次都创建一个新对象，
    /// 在调用对象的<see cref="IDisposable.Dispose"/>方法时，会将对象销毁
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="Pool{Obj}.Pool(Func{Obj})"/>
    /// <inheritdoc cref="IPool{Obj}"/>
    public static IPool<Obj> PoolSimple<Obj>(Func<Obj> create)
        where Obj : class, IDisposable
        => new Pool<Obj>(create);
    #endregion
    #region 创建IDirect
    #region 创建记事
    /// <summary>
    /// 创建一个记事，并返回
    /// </summary>
    /// <param name="path">记事所在的路径</param>
    /// <returns></returns>
    public static IDirect Notes(string path)
        => new Notes(path);
    #endregion
    #region 创建简易实现
    /// <summary>
    /// 创建一个简易的<see cref="IDirect"/>实现，
    /// 它直接封装了一个字典
    /// </summary>
    /// <returns></returns>
    public static IDirect DirectEmpty()
        => new DirectSimple();
    #endregion
    #endregion
    #region 常用Json序列化器
    /// <summary>
    /// 返回常用的Json序列化器，按照规范，
    /// 所有应用程序都应该添加这些序列化支持，
    /// 如果需要添加或删除本集合的元素，请在本集合被使用前执行这个操作
    /// </summary>
    public static IList<JsonConverter> JsonCommon { get; } = [];
    #endregion
    #region 包含常用Json序列化器的选项
    /// <summary>
    /// 返回一个<see cref="JsonSerializerOptions"/>，
    /// 它包含所有在<see cref="JsonCommon"/>中的转换器
    /// </summary>
    public static JsonSerializerOptions JsonCommonOptions
    {
        get
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(JsonCommon);
            return options;
        }
    }
    #endregion
}
