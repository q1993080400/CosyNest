namespace Microsoft.AspNetCore;

#region 说明文档
/*问：为什么需要本类型？
  答：在某些情况下，需要为不同的用途提供基类型相同，
  但是实现不同的服务，标准服务容器在请求服务时只根据类型进行区分，
  无法应对这种情况，因此作者声明了这个委托，
  你可以先将这个委托添加到服务容器中，然后调用它通过键获取真正需要的服务*/
#endregion
#region 委托
/// <summary>
/// 这个委托可以根据键获取服务
/// </summary>
/// <typeparam name="Key">键的类型</typeparam>
/// <typeparam name="Service">服务的类型</typeparam>
/// <param name="key"></param>
/// <returns></returns>
public delegate Service ServiceContainer<in Key, out Service>(Key key);
#endregion