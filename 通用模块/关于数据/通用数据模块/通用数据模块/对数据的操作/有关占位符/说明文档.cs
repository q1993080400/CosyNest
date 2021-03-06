
/*说明文档：
  问：在表达式树中为什么不直接使用IData，
  而是要使用PlaceholderData和PlaceholderValue这两个类型来间接表示数据？
  答：它们具有以下意义：

  #编写表达式的时候，为比较对象提供方便，举例说明，
  假设调用者的查询条件是：数据表中"ColumnName"这一列的值大于0，
  如果没有这两个类型，表达式应该这样写（参数x的类型是IData）：
  (int)x["ColumnName"] > 0
  由于IData中的索引器返回Object，它没有声明大多数运算符，
  因此必须进行强制转换，这非常麻烦，在复杂的表达式中会更加麻烦，
  有了这两个类型以后，由于它们支持所有关于比较和计算的运算符，
  表达式可以写成以下形式（参数x的类型是PlaceholderData）：
  x => x["ColumnName"] > 0

  #间接访问数据可以限制不合法的表达式，举例说明：
  IData中声明了大量有关复制，绑定的API，
  但是查询只需要关注数据本身的性质，它不需要调用这些方法，
  PlaceholderData中没有声明这些多余的API，它更纯粹，简洁*/