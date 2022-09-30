
/*问：通常，服务不需要知道自己的生存期，
  那么，为什么要声明INamingServiceTransient，
  INamingServiceScoped，INamingServiceSingleton三个接口？
  答：这是因为本接口性质特殊，它是提供服务的服务，很可能被同时注入到多个作用域，
  如果不在类型上将它们分开的话，按照ASP的规则，会请求到最后注入的那个服务，
  这会导致有的服务无法被请求到*/