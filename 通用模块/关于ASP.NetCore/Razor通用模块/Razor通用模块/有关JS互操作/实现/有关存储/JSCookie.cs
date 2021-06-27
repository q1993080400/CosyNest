using System;
using System.Collections.Generic;
using System.Design;
using System.Design.Async;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.JSInterop
{
    /// <summary>
    /// 这个类型可以通过JS互操作来索引和修改Cookie
    /// </summary>
    class JSCookie : JSRuntimeBase, IAsyncDictionary<string, string>
    {
        #region 通过JS读写Cookie文本
        #region 读取
        /// <summary>
        /// 通过JS互操作直接读取document.cookie属性
        /// </summary>
        /// <returns></returns>
        private ValueTask<string> GetCookie()
            => JSRuntime.GetProperty<string>("document.cookie");
        #endregion
        #region 写入
        /// <summary>
        /// 通过JS互操作直接写入document.cookie属性
        /// </summary>
        /// <param name="cookie">要写入的Cookie文本</param>
        /// <returns></returns>
        private ValueTask SetCookie(string cookie)
            => JSRuntime.SetProperty("document.cookie", cookie);
        #endregion
        #region 返回最小UTC时间的字符串
        /// <summary>
        /// 返回JS中UTC最小时间的字符串格式，
        /// 在删除Cookie时会用到
        /// </summary>
        private static string MinDate { get; } = "Thu, 01 Jan 1970 00:00:00 GMT";
        #endregion
        #endregion
        #region 关于读取和写入Cookie
        #region 读取或写入值（异步索引器）
        public IAsyncIndex<string, string> IndexAsync { get; }
        #endregion
        #region 读取Cookie且不引发异常
        public async Task<(bool Exist, string? Value)> TryGetValueAsync(string key)
        {
            var kv = await this.Linq(x => x.FirstOrDefault(y => y.Key == key));
            return kv.Equals(default(KeyValuePair<string, string>)) ? (false, null) : (true, kv.Value);
        }
        #endregion
        #endregion
        #region 关于集合
        #region 枚举所有键值对
        public async IAsyncEnumerator<KeyValuePair<string, string>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            foreach (var item in ToolRegex.KeyValuePairExtraction(await GetCookie(), ";"))
            {
                yield return item;
            }
        }
        #endregion
        #region 检查键值对是否存在
        public async Task<bool> ContainsAsync(KeyValuePair<string, string> item)
            => (await TryGetValueAsync(item.Key)) is (true, var value) && Equals(value, item.Value);
        #endregion
        #endregion
        #region 关于添加和删除键值对
        #region 删除指定的键
        public async Task<bool> RemoveAsync(string key)
        {
            if (await this.To<IAsyncDictionary<string, string>>().ContainsKeyAsync(key))
            {
                await SetCookie($"{key}=; expires={MinDate}");
                return true;
            }
            return false;
        }
        #endregion
        #region 删除指定的键值对
        public Task<bool> RemoveAsync(KeyValuePair<string, string> item)
            => RemoveAsync(item.Key);
        #endregion
        #region 全部删除
        public async Task ClearAsync()
        {
            foreach (var key in await this.Linq(x => x.ToArray()))
            {
                await RemoveAsync(key);
            }
        }
        #endregion
        #region 添加键值对
        public Task AddAsync(KeyValuePair<string, string> item)
            => IndexAsync.Set(item.Key, item.Value);
        #endregion
        #endregion
        #region 构造函数
        /// <inheritdoc cref="JSRuntimeBase(IJSRuntime)"/>
        public JSCookie(IJSRuntime jsRuntime)
            : base(jsRuntime)
        {
            IndexAsync = CreateDesign.AsyncIndex<string, string>(
               async key => (await this.Linq(x => x.First(y => y.Key == key))).Value,
               (key, value) => SetCookie($"{key}={value}").AsTask());
        }
        #endregion
    }
}
