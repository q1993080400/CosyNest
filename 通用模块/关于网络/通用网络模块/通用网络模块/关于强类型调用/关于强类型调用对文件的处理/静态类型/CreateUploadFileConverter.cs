using System.DataFrancis;
using System.Text.Json.Serialization.Metadata;

namespace System.NetFrancis;

public static partial class CreateNet
{
    //这个部分类专门用来声明有关转换强类型调用文件的方法

    #region 用来读取文件的转换器
    /// <summary>
    /// 获取一个转换器，它专门用来读取强类型调用中的文件，它只能用来读取，
    /// 而且是有状态的，每次使用时，必须创建一个新的对象
    /// </summary>
    /// <param name="readFile">用来读取文件的委托，
    /// 它的参数是这个文件的中介对象，返回值是读取到的文件</param>
    /// <returns></returns>
    public static Action<JsonTypeInfo> UploadFileResolverModifiersRead(Func<IHasUploadFileMiddle, IHasUploadFileServer> readFile)
        => info =>
        {
            if (info.Kind is not JsonTypeInfoKind.Object)
                return;
            foreach (var property in info.Properties)
            {
                #region 用于转换的本地函数
                IHasPreviewFile? Convert(IHasPreviewFile file)
                => file switch
                {
                    { IsEnable: false } => null,
                    IHasUploadFileMiddle { } uploadFileMiddle => readFile(uploadFileMiddle),
                    IHasUploadFileClient => throw new NotSupportedException($"不能在服务端使用{nameof(IHasUploadFileClient)}接口"),
                    _ => file
                };
                #endregion
                var propertyType = property.PropertyType;
                var set = property.Set;
                if (set is null)
                    continue;
                if (typeof(IEnumerable<IHasPreviewFile>).IsAssignableFrom(propertyType))
                {
                    property.Set = (obj, value) =>
                    {
                        if (value is null)
                        {
                            set(obj, value);
                            return;
                        }
                        var files = value.To<IEnumerable<IHasPreviewFile>>().
                        Select(Convert).WhereNotNull().ToArray();
                        var convertFiles = propertyType.CreateCollection(files);
                        set(obj, convertFiles);
                    };
                    continue;
                }
                if (typeof(IHasPreviewFile).IsAssignableFrom(propertyType))
                {
                    property.Set = (obj, value) =>
                    {
                        if (value is null)
                        {
                            set(obj, value);
                            return;
                        }
                        var file = value.To<IHasPreviewFile>();
                        var setFile = Convert(file);
                        set(obj, setFile);
                    };
                    continue;
                }
            }
        };
    #endregion
    #region 用来写入文件的转换器
    /// <summary>
    /// 获取一个转换器，它专门用来写入强类型调用中的文件，它只能用来写入，
    /// 而且是有状态的，每次使用时，必须创建一个新的对象
    /// </summary>
    /// <param name="writeFile">用来写入文件到其他地方的委托，
    /// 它的参数分别是待写入的文件，以及为这个文件赋予的ID</param>
    /// <returns></returns>
    public static Action<JsonTypeInfo> UploadFileResolverModifiersWrite(Action<IUploadFile, Guid> writeFile)
        => info =>
        {
            if (info.Kind is not JsonTypeInfoKind.Object)
                return;
            foreach (var property in info.Properties)
            {
                #region 用于转换的本地函数
                IHasPreviewFile? Convert(IHasPreviewFile file)
                {
                    switch (file)
                    {
                        case { IsEnable: false }:
                            return null;
                        case IHasUploadFileClient { IsUploadCompleted: true } fileClient:
                            return CreateDataObj.PreviewFile(fileClient.CoverUri, fileClient.Uri, fileClient.FileName, fileClient.ID);
                        case IHasUploadFileClient { IsUploadCompleted: false } fileClient:
                            var uploadFileMiddle = CreateDataObj.UploadFileMiddle(fileClient);
                            writeFile(fileClient.UploadFile, uploadFileMiddle.FileID);
                            return uploadFileMiddle;
                        default:
                            return file;
                    }
                }
                #endregion
                var propertyType = property.PropertyType;
                var get = property.Get;
                if (get is null)
                    continue;
                if (typeof(IEnumerable<IHasPreviewFile>).IsAssignableFrom(propertyType))
                {
                    property.Get = value =>
                    {
                        var propertyValue = get(value);
                        var files = propertyValue.To<IEnumerable<IHasPreviewFile>>();
                        if (files is null)
                            return null;
                        var filterFiles = files.Select(Convert).WhereNotNull().ToArray();
                        return propertyType.CreateCollection(filterFiles);
                    };
                    continue;
                }
                if (typeof(IHasPreviewFile).IsAssignableFrom(propertyType))
                {
                    property.Get = value =>
                    {
                        var propertyValue = get(value);
                        var file = propertyValue.To<IHasPreviewFile>();
                        if (file is null)
                            return null;
                        return Convert(file);
                    };
                    continue;
                }
            }
        };
    #endregion
}
