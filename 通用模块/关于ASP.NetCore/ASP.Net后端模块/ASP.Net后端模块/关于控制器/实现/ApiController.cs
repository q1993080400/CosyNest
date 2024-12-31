namespace Microsoft.AspNetCore.Mvc;

/// <summary>
/// 该类型是API控制器的基类，
/// 它还启用了区分action的路由模板，
/// 减少了一些样板代码
/// </summary>
[Route("api/[controller]/[action]")]
[ApiController]
public abstract class ApiController : ControllerBase
{

}
