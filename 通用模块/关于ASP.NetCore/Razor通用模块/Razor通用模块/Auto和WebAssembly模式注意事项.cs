
/*在开发Auto和WebAssembly模式时，请注意以下事项：
 
  #在发布时，应该首先完全删除项目，然后重新拉取项目，再进行发布，
  这是为了清除所有缓存，它可以避免应用从Auto模式切换到WebAssembly模式时，
  产生的某些奇怪的问题

  #不要在document.visibilitychange事件中发起Http请求，
  这是因为截止18.3版本，它在Safari浏览器上会出现Bug
 */