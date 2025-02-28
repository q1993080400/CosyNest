using System.Reflection;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 凡是实现这个基类的类型，
/// 都可以作为用来渲染<see cref="FormViewer{Model}"/>的中每个属性的参数
/// </summary>
/// <inheritdoc cref="FormViewer{Model}"/>
public abstract record RenderFormViewerPropertyInfoBase<Model>
    where Model : class
{
    #region 要渲染的模型
    /// <summary>
    /// 获取要渲染的模型
    /// </summary>
    public required Model FormModel { get; init; }
    #endregion
    #region 要渲染的属性
    /// <summary>
    /// 获取要渲染的属性
    /// </summary>
    public required PropertyInfo Property { get; init; }
    #endregion
    #region 是否为集合
    /// <summary>
    /// 获取这个属性是否为集合类型
    /// </summary>
    public bool IsCollection
        => Property.PropertyType.IsCollection();
    #endregion
    #region 分组的名字
    /// <summary>
    /// 获取分组的名字，
    /// 同一组的属性在一起渲染
    /// </summary>
    public required string? GroupName { get; init; }
    #endregion
    #region 属性的值
    /// <summary>
    /// 获取属性的值
    /// </summary>
    public object? Value
    {
        get
        {
            var value = Property.GetValue(FormModel);
            return PropertyValueConvert is null ?
                value :
                PropertyValueConvert(Property, value);
        }
    }
    #endregion
    #region 获取属性的值
    /// <summary>
    /// 将属性转换为指定的值，
    /// 然后返回
    /// </summary>
    /// <typeparam name="Obj">属性的值的类型</typeparam>
    /// <returns></returns>
    public Obj? GetValue<Obj>()
        => Value.To<Obj>();
    #endregion
    #region 值转换函数
    /// <summary>
    /// 这个函数的第一个参数是属性，
    /// 第二个参数是属性的值，
    /// 返回值是渲染的时候应该渲染的值，
    /// 通过它可以实现某些特殊操作，
    /// 例如把空字符串渲染成不明
    /// </summary>
    public required Func<PropertyInfo, object?, object?>? PropertyValueConvert { get; init; }
    #endregion
    #region 值改变时的函数
    /// <summary>
    /// 当值改变时触发的委托，
    /// 它的参数就是数据的新值
    /// </summary>
    public required Func<object?, Task>? OnPropertyChange { get; init; }
    #endregion
    #region 创建属性绑定对象
    #region 直接绑定
    /// <summary>
    /// 创建一个属性绑定对象，
    /// 它允许绑定表单属性
    /// </summary>
    /// <typeparam name="Value">属性的类型</typeparam>
    /// <returns></returns>
    public IBindProperty<Value> BindValue<Value>()
        => IsReadOnly ?
        new BindPropertyInfoStatic<Value>((Value)this.Value!) :
        new BindPropertyInfo<Value>(FormModel, Property);
    #endregion 
    #region 通过桥接绑定
    /// <summary>
    /// 创建一个属性绑定对象，
    /// 它允许通过桥接的方式绑定表单属性，
    /// 能够进行复杂的类型转换
    /// </summary>
    /// <returns></returns>
    /// <inheritdoc cref="BindPropertyInfoAdapter{Adapter, Bottom}"/>
    /// <inheritdoc cref="BindPropertyInfoAdapter{Adapter, Bottom}.BindPropertyInfoAdapter(object, PropertyInfo, Func{Bottom?, Adapter?}, Func{Adapter?, Bottom?})"/>
    public IBindProperty<Adapter> BindValueAdapter<Adapter, Bottom>
        (Func<Bottom?, Adapter?> convert, Func<Adapter?, Bottom?> convertReverse)
        => new BindPropertyInfoAdapter<Adapter, Bottom>(FormModel, Property, convert, convertReverse);
    #endregion
    #endregion
    #region 是否只读
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示仅提供数据显示功能，不提供数据编辑功能
    /// </summary>
    public required bool IsReadOnly { get; init; }
    #endregion
    #region 渲染偏好
    /// <summary>
    /// 获取进行渲染时的偏好，
    /// 如果未指定任何偏好，则为<see langword="null"/>
    /// </summary>
    public required RenderPreference? RenderPreference { get; init; }
    #endregion
    #region 关于可预览文件
    #region 对封装可预览文件的描述
    /// <summary>
    /// 获取对这个属性封装可预览文件的性质的描述，
    /// 如果它没有封装可预览文件，
    /// 则返回<see langword="null"/>
    /// </summary>
    public required IHasPreviewFilePropertyInfo? HasPreviewFilePropertyInfo { get; init; }
    #endregion
    #region 是否正在上传
    /// <summary>
    /// 指示这个属性是否是一个可上传文件的属性，
    /// 且正在执行上传操作，你可以给予用户一些提示，
    /// 告诉它们正在上传
    /// </summary>
    public required bool InUpload { get; init; }
    #endregion
    #region 是否含有可预览文件
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 表示它直接或间接含有可预览文件
    /// </summary>
    public bool HasPreviewFile
        => HasPreviewFilePropertyInfo is { };
    #endregion
    #endregion
}
