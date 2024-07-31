using System.Design;

namespace System.IOFrancis;

/// <summary>
/// 凡是实现这个接口的类型，都可以视为一个来自文件或流的对象，
/// 它可以从文件或流中加载，也可以保存到文件或流中
/// </summary>
public interface IFromIO : IInstruct, IAsyncDisposable
{
    #region 说明文档
    /*实现本接口请遵循以下规范：
      #注意:下文所述“空白的对象”指的不是null,
      而是一个新创建的默认对象，类似新建Excel文档

      #请根据以下逻辑创建本接口的实现：

      如果指定了路径：

          如果路径存在：
          从该路径加载对象

          如果路径不存在：
          不要引发异常，并创建一个空白的对象，
          但是仍然要写入Path属性，它会影响到默认保存路径

      如果没有指定路径：
      创建一个空白的对象*/
    #endregion
    #region 关于路径
    #region 返回文件的格式
    /// <summary>
    /// 这个属性指示对象的格式，
    /// 具体来说，指的是文件扩展名
    /// </summary>
    string Format { get; }
    #endregion
    #region 返回文件路径
    /// <summary>
    /// 返回该对象的绝对文件路径，
    /// 如果为<see langword="null"/>，代表尚未保存到文件中
    /// </summary>
    string? Path { get; }
    #endregion
    #region 是否存在于硬盘
    /// <summary>
    /// 如果这个属性为<see langword="true"/>，
    /// 代表该对象已经保存到硬盘，或是从硬盘文件中加载的，
    /// 否则代表它只存在于内存中
    /// </summary>
    bool IsExist { get; }
    #endregion
    #endregion
    #region 关于保存
    #region 保存对象
    /// <summary>
    /// 在指定的路径保存对象
    /// </summary>
    /// <param name="path">指定的保存路径，
    /// 如果为<see langword="null"/>，代表原地保存</param>
    Task Save(string? path = null);

    /*在实现本API时，请遵循以下规范：
      #本API抹平了原地保存和另存为的差异，
      如果依赖的底层对象在保存方面需要区分这种差别，
      则应该通过对比Path属性和path参数智能判断应该选择哪一种保存方式

      #如果不需要保存或不能保存，就不执行保存操作

      不需要保存的情况举例：
      该对象未修改，而且保存模式为原地保存

      不能保存的情况举例：
      该对象是一个Excel工作簿，
      而且其中没有任何工作表，保存它会出现异常，
      但如果该工作簿尚未保存到文件中，且没有指定保存路径，
      这种情况下应该引发异常，因为这是由于调用者的疏忽导致*/
    #endregion
    #region 是否自动保存
    /// <summary>
    /// 如果这个值为<see langword="true"/>，
    /// 则在执行<see cref="IAsyncDisposable.DisposeAsync"/>方法时，还会自动保存文件，
    /// 前提是文件的路径不为<see langword="null"/>
    /// </summary>
    bool AutoSave { get; set; }
    #endregion
    #endregion
}
