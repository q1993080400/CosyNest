namespace Microsoft.AspNetCore;

/// <summary>
/// 这个静态类可以用来创建和Razor有关的对象
/// </summary>
public static class CreateRazor
{
    #region 创建IHandleEvent
    /// <summary>
    /// 返回一个<see cref="IHandleEvent"/>，
    /// 与默认实现不同的是，它不会刷新组件，
    /// 在某些情况下，使用它可以提高性能
    /// </summary>
    public static IHandleEvent HandleEventInvalid { get; }
        = new HandleEventInvalid();

    /*问：如何使用这个类型来提高性能？
      答：举例说明，假设你需要一个这样的功能：

      点击界面上除了菜单以外的任何一个地方，关闭这个菜单
    
      那么，你可以在声明一个来自菜单的方法M，它用来关闭菜单，
      然后将其注册到界面父组件，并在父组件onclick事件的方法M1中调用它
    
      如果按照常规流程，在onclick执行完毕后，整个界面都会刷新，这是不值得的，
      所以，你可以直接在父组件的onclick事件中传入一个EventCallback，
      它的IHandleEvent参数传入这个接口，MulticastDelegate参数传入M1
    
      这样一来，onclick执行完毕后，不会刷新整个界面，
      你可以在M中手动调用StateHasChanged来仅刷新菜单*/
    #endregion
}
