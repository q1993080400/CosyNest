﻿using System.Design.Direct;

namespace System.DataFrancis;

/// <summary>
/// 所有实现这个接口的类型，
/// 都可以被当作一个数据进行传递，
/// 它不仅可以直接读写属性，还支持将改动推送到数据源
/// </summary>
public interface IData : IDirect
{
    #region 说明文档
    /*说明文档：
      问：为什么要设计这个类型？
      答：为了抹平不同类型数据的差异，
      如果它们没有被抽象成简单而统一的类型，
      那么获取，传输和访问数据会变得非常复杂

      问：为什么不使用实体类，而是使用这个类型来描述数据？
      答：因为实体类需要事先定义，它带来了几个问题，按照严重性从低到高依次排列：
      1.需要事先定义类型，比较麻烦
      2.无法处理未知格式的数据
      3.由于实体类类型繁多，且没有统一基类，
      因此开发有关它们的API非常麻烦，而且无法多态

      问：本类型通过索引器来获取对象的属性，如何保障类型安全？
      答：本类型不是强类型的模型，因此类型安全确实存在一定隐患，但可以通过以下措施缓解：
      将属性的名称通过常量储存起来，然后在访问属性时传入该常量，可以避免由于拼写错误引发的问题

      并可通过以下措施完全解决，这是推荐做法：
      将实体类继承自Entity，它可以同时兼顾IData的灵活和实体类的安全

      本接口同时支持强类型和弱类型实体类，这是经过慎重考虑后的结果，
      因为强类型实体类便于使用数据，弱类型实体类便于实现数据源接口

      问：为什么本接口不实现INotifyPropertyChanged？
      答：因为WPF，Winfrom等旧版UI框架需要通过它来通知属性更改，
      但是新版框架，如Blazor等则不需要，本框架无意继续支持旧版框架，
      但需要观望的是，作者有意支持MAUI，但就是不知道MAUI是否依赖这个接口，
      如果为是，在它正式发布后请为本接口实现INotifyPropertyChanged*/
    #endregion
    #region 数据的元数据
    /// <summary>
    /// 获取或写入数据的元数据，
    /// 它被用来标识数据的来源或其他任何需要的信息
    /// </summary>
    object? Metadata { get; set; }

    /*实现本API请遵循以下规范：
      #除以下情况外，写入该属性直接引发异常：
      1.该属性为null
      2.该属性不为null，但是写入null，
      这通常出现在数据被删除后，数据管道注销它的元数据*/
    #endregion
    #region 数据ID的列名
    /// <summary>
    /// 获取或设置数据ID的列名，
    /// 通过它可以获取一个<see cref="Guid"/>，
    /// 它可以用来标志数据的唯一性，如果为<see langword="null"/>，代表不存在标识
    /// </summary>
    string? IDColumnName { get; }

    /*实现本API请遵循以下规范：
    
      #按照约定，标识必须为Guid类型，不能为自增列或其他对象*/
    #endregion
}
