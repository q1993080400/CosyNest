using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Design.Direct;
using System.Diagnostics.CodeAnalysis;

namespace System.DataFrancis
{
    /// <summary>
    /// 该类型是实现<see cref="IData"/>的可选基类
    /// </summary>
    public abstract record DataBase : IData
    {
        #region 用来储存数据的字典
        /// <summary>
        /// 这个字典被用来储存数据
        /// </summary>
        protected abstract IRestrictedDictionary<string, object?> Data { get; }
        #endregion
        #region 字典的实现
        #region 键值对的元素数量
        public int Count => Data.Count;
        #endregion
        #region 键集合
        public IEnumerable<string> Keys => Data.Keys;
        #endregion
        #region 值集合
        public IEnumerable<object?> Values => Data.Values;
        #endregion
        #region 是否存在元素
        public bool ContainsKey(string key) => Data.ContainsKey(key);
        #endregion
        #region 尝试获取元素
        public bool TryGetValue(string key, [MaybeNullWhen(false)] out object? value)
            => Data.TryGetValue(key, out value);
        #endregion
        #region 枚举键值对集合
        public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
            => Data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
        #endregion
        #endregion
        #region IData的实现
        #region 架构约束
        public abstract ISchema? Schema { get; set; }
        #endregion
        #region 关于读取和写入数据
        #region 辅助方法：通过键写入值
        /// <summary>
        /// 通过键写入值，如果键不存在，会引发异常
        /// </summary>
        /// <param name="columnName">数据的列名</param>
        /// <param name="newValue">数据的新值</param>
        private void SetValueAided(string columnName, object? newValue)
        {
            Data[columnName] = Schema is { } s ? newValue.To(s.Schema[columnName]) : newValue;      //检查架构约束
            this.Changed(PropertyChanged, columnName);
        }
        #endregion
        #region 索引器
        public object? this[string columnName]
        {
            get => Data[columnName];
            set
            {
                SetValueAided(columnName, value);
                Binding?.NoticeUpdateToSource(columnName, value);
            }
        }
        #endregion
        #region 修改数据时引发的事件
        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion
        #endregion
        #region 关于数据更新
        #region 获取或设置数据绑定
        private IDataBinding? BindingField;

        public IDataBinding? Binding
        {
            get => BindingField;
            set
            {
                if (BindingField is { })
                {
                    BindingField.NoticeUpdateToData -= SetValueAided;
                    BindingField.NoticeDeleteToData -= DeleteAided;
                }
                if (value is { })
                {
                    value.NoticeUpdateToData += SetValueAided;
                    value.NoticeDeleteToData += DeleteAided;
                }
                BindingField = value;
            }
        }
        #endregion
        #region 关于删除数据
        #region 辅助方法
        /// <summary>
        /// 删除数据的辅助方法，它不会调用<see cref="IDataBinding.NoticeDeleteToSource"/>，
        /// 可以防止循环调用引发的无限递归
        /// </summary>
        private void DeleteAided()
        {
            if (DeleteEvent is { })
            {
                DeleteEvent(this);
                DeleteEvent = null;
            }
            PropertyChanged = null;
            Binding = null;
        }
        #endregion
        #region 删除数据
        public void Delete()
        {
            Binding?.NoticeDeleteToSource();
            DeleteAided();
        }
        #endregion
        #region 删除数据时引发的事件
        public event Action<IData>? DeleteEvent;
        #endregion
        #endregion
        #endregion
        #region 复制数据
        #region 正式方法
        public IDirect Copy(bool copyValue = true, Type? type = null)
        {
            var copy = type switch
            {
                var t when t is null || t == GetType() => CreateSelf(),
                var t when typeof(IDirect).IsAssignableFrom(t) && t.CanNew() => t.GetTypeData().ConstructorCreate<IDirect>(),
                var t => throw new ArgumentException($"{t}不实现{nameof(IDirect)}，或者不具备公开无参数构造函数，无法复制")
            };
            foreach (var (name, value) in this)
            {
                copy[name] = copyValue ? value : null;
            }
            return copy;
        }
        #endregion
        #region 模板方法
        /// <summary>
        /// 创建一个与本对象相同类型的<see cref="IDirect"/>，
        /// 它的属性字典全部为空
        /// </summary>
        /// <returns></returns>
        protected abstract IDirect CreateSelf();
        #endregion
        #endregion
        #endregion
        #region 重写ToString
        public override string? ToString()
            => this.Join(x => $"{x.Key}：{x.Value}", ";");
        #endregion
    }
}
