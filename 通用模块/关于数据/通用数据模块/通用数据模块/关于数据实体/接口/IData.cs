using System.ComponentModel;
using System.Design.Direct;

namespace System.DataFrancis
{
    /// <summary>
    /// 所有实现这个接口的类型，
    /// 都可以被当作一个数据进行传递
    /// </summary>
    public interface IData : IDirect, INotifyPropertyChanged
    {
        #region 说明文档
        /*说明文档：
          问：为什么要设计这个类型？
          答：为了抹平不同类型数据的差异，
          如果它们没有被抽象成简单而统一的类型，
          那么获取，传输和访问数据会变得非常复杂

          问：为什么不使用实体类，而是使用这个类型来描述数据？
          答：因为实体类需要事先定义，它带来了三个问题，按照严重性从低到高依次排列：
          1.需要事先定义类型，比较麻烦
          2.无法处理未知格式的数据
          3.实体类不支持绑定，在数据源发生更改时，需要手动更新它们
          4.由于实体类类型繁多，且没有统一基类，
          因此开发有关它们的API非常麻烦，而且无法多态

          问：本类型通过索引器来获取对象的属性，如何保障类型安全？
          答：本类型不是强类型的模型，因此类型安全确实存在一定隐患，但可以通过以下措施缓解：

          #将属性的名称通过常量储存起来，然后在访问属性时传入该常量，可以避免由于拼写错误引发的问题

          #将实体类继承自Entity，它可以同时兼顾IData的灵活和实体类的安全
           
          问：在本类型的早期版本中，
          曾经有一个类似主键的ID属性用来标识数据唯一性，
          但后来设计者认为这个设计是不需要的，予以删除，这是为什么？
          答：因为设计者认为，ID是为了修改和删除数据准备的，
          但是这个操作应该在后台隐式完成，调用者不需要知道数据的ID，
          在具体实现的时候，可以利用一个使用闭包的委托来储存数据ID，
          这样封装性更好，也更简洁方便*/
        #endregion
        #region 关于数据更新
        #region 获取或设置数据绑定
        /// <summary>
        /// 获取或设置数据绑定对象，
        /// 它可以在数据或数据源发生更改时通知对方
        /// </summary>
        IDataBinding? Binding { get; set; }

        /*实现本API请遵循以下规范：
          写入这个属性时，将数据与新的Binding绑定起来，
          并取消和旧Binding的绑定关系（如果它们不为null）*/
        #endregion
        #region 关于删除数据
        #region 删除数据
        /// <summary>
        /// 将这条数据删除
        /// </summary>
        void Delete();

        /*实现本API请遵循以下规范：
          在执行这个方法后，将Binding属性和DeleteEvent事件设为null，
          因为数据只能被删除一次，且被删除后，已经没有更新的必要*/
        #endregion
        #region 删除数据时引发的事件
        /// <summary>
        /// 在数据被删除时，引发这个事件，
        /// 事件参数就是数据本身
        /// </summary>
        event Action<IData>? DeleteEvent;
        #endregion
        #endregion 
        #endregion 
    }
}
