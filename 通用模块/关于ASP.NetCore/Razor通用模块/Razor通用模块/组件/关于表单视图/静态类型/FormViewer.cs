using System.NetFrancis.Http;

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// 这个组件是表单视图的帮助类型
/// </summary>
public static class FormViewer
{
    #region 获取将空字符串显示为其他字符的值转换函数
    /// <summary>
    /// 获取一个值转换函数，
    /// 如果值的类型为字符串，
    /// 且值为<see langword="null"/>，
    /// 则将其显示为替代字符串
    /// </summary>
    /// <param name="ifNullText">为<see langword="null"/>时显示的替代字符串</param>
    /// <returns></returns>
    public static Func<Type, object?, object?> RenderTextIfNull(string ifNullText)
        => (type, value) => (type, value) switch
        {
            (var t, null) when t == typeof(string) => ifNullText,
            _ => value
        };
    #endregion
    #region 返回创建属性被修改时调用的函数的函数
    /// <summary>
    /// 这个高阶函数返回一个函数，
    /// 它可以用来给<see cref="FormViewer{Model}.CreatePropertyChangeEvent"/>参数赋值
    /// </summary>
    /// <typeparam name="ServerUpdate">服务端接口的类型</typeparam>
    /// <typeparam name="Parameter"><see cref="IServerUpdatePart{Parameter}"/>的参数类型</typeparam>
    /// <typeparam name="Model">实体类的类型</typeparam>
    /// <param name="http">用来向后端发起请求的Http客户端对象</param>
    /// <param name="createParameter">这个委托传入封装好的属性更改信息，返回向后端请求需要的参数</param>
    /// <returns></returns>
    public static Func<RenderFormViewerPropertyInfoBase<Model>, Func<object?, Task>?> CreatePropertyChangeEvent<ServerUpdate, Parameter, Model>
        (IHttpClient http, Func<ServerUpdateEntityInfo, Parameter> createParameter)
        where ServerUpdate : class, IServerUpdatePart<Parameter>
        where Parameter : class
        where Model : class, IWithID
        => info =>
        {
            var property = info.Property;
            var propertyType = property.PropertyType;
            var propertyName = property.Name;
            var id = info.FormModel.ID;
            return async value =>
            {
                var newValue = value.To(propertyType);
                var info = new ServerUpdateEntityInfo()
                {
                    ID = id,
                    UpdatePropertyInfo =
                    [
                        new()
                        {
                            PropertyName = propertyName,
                            Value = newValue
                        }
                    ]
                };
                var parameter = createParameter(info);
                await http.StrongType<ServerUpdate>().Request(x => x.UpdateProperty(parameter));
            };
        };
    #endregion
}
