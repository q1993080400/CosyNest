using System.NetFrancis.Api;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace System;

public static partial class ExtendWebApi
{
    //这个部分类专门用来声明有关WebApi的扩展方法

    #region 验证WebApi的参数
    /// <param name="controller">控制器实例，函数会从它的服务容器中，
    /// 请求<see cref="DataVerify"/>委托来进行验证</param>
    /// <inheritdoc cref="ToolVerify.VerifyParameter{Return}(DataVerify, object)"/>
    public static Return? VerifyParameter<Return>(this ControllerBase controller, object parameter)
        where Return : APIPack, new()
        => ToolWebApi.VerifyParameter<Return>(controller.HttpContext.RequestServices, parameter);
    #endregion
}
