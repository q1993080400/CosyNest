
/*使用JsonSerializer中的API进行转换时，它具有以下奇怪的行为：
 
  假设处于某个场景，它满足以下条件：
  1.JsonSerializerOptions.Converters中具有一个JsonConverter<IDirect>,
  但是它实际上支持多态反序列化，可以转换IDirect的派生接口IData，
  支持多态反序列化表现在：CanConvert(typeof(IData))返回true
  2.要转换的目标是一个泛型类型，例如T[]

  则：如果转换的目标类型是IDirect[]，则函数可以正常调用转换器进行转换，
  但如果目标类型是IData[]，即便转换器的CanConvert(typeof(IData))方法返回true，
  由于它不是JsonConverter<IData>，所以会引发一个异常

  另一个奇怪行为：
  在这种情况下，如果转换的目标泛型类型是基本集合类型，
  则默认转换器的CanConvert方法返回true，但是它实际上不能执行转换

  这个问题很可能是由于微软的设计缺陷所引起的，
  JsonConverter不遵循里氏替换原则，
  所以必须将它转换为JsonConverter<T>才能够完成序列化和反序列化，
  JsonSerializer在底层应该是根据类型来判断是否可执行转换，所以引发了这个异常，
  为此，本框架声明了几个类型，它们会尝试使用一切可用的转换器来进行转换，来解决这个问题*/