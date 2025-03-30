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
    /// <inheritdoc cref="ToolWebApi.VerifyParameter{Return}(DataVerify, object)"/>
    public static Return? VerifyParameter<Return>(this ControllerBase controller, object parameter)
        where Return : APIPack, new()
        => ToolWebApi.VerifyParameter<Return>(controller.HttpContext.RequestServices, parameter);
    #endregion
    #region 创建模拟控制器
    /// <summary>
    /// 创建另一个控制器，
    /// 它拥有和当前控制器完全相同的上下文，
    /// 它一般被用于从一个控制器中调用另一个控制器的方法
    /// </summary>
    /// <typeparam name="Controller">要创建的控制器的类型</typeparam>
    /// <param name="controller">当前控制器，它的上下文会被复制到另一个控制器中</param>
    /// <returns></returns>
    public static Controller MakeController<Controller>(this ControllerBase controller)
        where Controller : ControllerBase, new()
        => new()
        {
            ControllerContext = controller.ControllerContext
        };
    #endregion
}
