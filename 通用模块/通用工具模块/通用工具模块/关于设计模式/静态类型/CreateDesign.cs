using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Design.Async;
using System.Design.Direct;
using System.Linq;
using System.Threading.Tasks;

namespace System.Design
{
    /// <summary>
    /// 这个静态类可以用来帮助创建和设计模式有关的类型
    /// </summary>
    public static class CreateDesign
    {
        #region 创建ISchema
        #region 直接创建
        /// <summary>
        /// 指定架构，并创建一个<see cref="ISchema"/>
        /// </summary>
        /// <param name="schema">这个字典指定架构的属性名称和类型</param>
        /// <returns></returns>
        public static ISchema Schema(IReadOnlyDictionary<string, Type> schema)
            => new Schemas(schema);
        #endregion
        #region 快速创建
        /// <summary>
        /// 通过若干描述属性名称和类型的元组创建架构
        /// </summary>
        /// <param name="propertys">这个元组的项分别是属性的名称以及类型</param>
        /// <returns></returns>
        public static ISchema Schema(params (string Name, Type Type)[] propertys)
            => Schema(propertys.ToDictionary(true));
        #endregion
        #region 通过IDirect创建
        /// <summary>
        /// 返回一个<see cref="IDirect"/>的架构，
        /// 如果<see cref="IDirect.Schema"/>未指定，
        /// 则通过它现有的属性和值生成架构
        /// </summary>
        /// <param name="direct">待返回架构的<see cref="IDirect"/></param>
        /// <returns></returns>
        public static ISchema Schema(IDirect direct)
            => direct.Schema ??
            Schema(direct.ToDictionary(x => (x.Key, x.Value?.GetType() ?? typeof(object)), true));
        #endregion
        #endregion
        #region 创建异步属性
        /// <summary>
        /// 使用指定的读取和写入委托创建异步属性
        /// </summary>
        /// <typeparam name="Value">异步属性的值的类型</typeparam>
        /// <param name="getDelegate">用于读取异步属性的委托</param>
        /// <param name="setDelegate">用于写入异步属性的委托</param>
        /// <returns></returns>
        public static IAsyncProperty<Value> AsyncProperty<Value>(Func<Task<Value>> getDelegate, Func<Value, Task> setDelegate)
            => new AsyncProperty<Value>(getDelegate, setDelegate);
        #endregion
        #region 创建异步索引器
        /// <summary>
        /// 使用指定的读取和写入委托创建只有一个参数的异步索引器
        /// </summary>
        /// <typeparam name="P1">索引器的第一个参数</typeparam>
        /// <typeparam name="Value">索引器的类型</typeparam>
        /// <param name="getDelegate">用于读取异步索引器的委托</param>
        /// <param name="setDelegate">用于写入异步索引器的委托</param>
        /// <returns></returns>
        public static IAsyncIndex<P1, Value> AsyncIndex<P1, Value>(Func<P1, Task<Value>> getDelegate, Func<P1, Value, Task> setDelegate)
            => new AsyncIndexP1<P1, Value>(getDelegate, setDelegate);
        #endregion
        #region 创建单例对象
        private static ConcurrentDictionary<Type, object> SingleCache { get; } = new();

        /// <summary>
        /// 返回一个类型的实例，
        /// 如果多次调用，不会重复创建
        /// </summary>
        /// <typeparam name="Obj">要返回实例的类型，
        /// 它必须要有无参数构造函数，但不一定要公开</typeparam>
        /// <returns></returns>
        public static Obj Single<Obj>()
            where Obj : notnull
            => (Obj)SingleCache.TrySetValue(typeof(Obj),
                type => type.GetTypeData().ConstructorCreate<Obj>()).Value;
        #endregion
    }
}
