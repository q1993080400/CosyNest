using System.Text.Json;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Microsoft.AspNetCore;

/// <summary>
/// 这个类型是<see cref="IModelBinder"/>的实现，
/// 可以用来绑定封装了<see cref="IHasPreviewFile"/>的对象
/// </summary>
sealed class HasPreviewFileModelBinder : IModelBinder
{
    #region 尝试绑定模型
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var request = bindingContext.HttpContext.Request;
        if (!request.HasFormContentType)
        {
            await bindingContext.BindingFailed();
            return;
        }
        var form = await request.ReadFormAsync();
        var jsonText = form[IHasPreviewFile.ContentKey].ToString();
        if (jsonText.IsVoid())
        {
            await bindingContext.BindingFailed();
            return;
        }
        var modelType = bindingContext.ModelMetadata.ModelType;
        var bindingModel = JsonSerializer.Deserialize(jsonText, modelType, JsonSerializerOptions.Web);
        if (bindingModel is null)
        {
            await bindingContext.BindingFailed();
            return;
        }
        var uploadFileDictionary = form.Files.ToDictionary(x => x.Name, x => new UploadFile(x));
        var previewFileProperty = HasPreviewFilePropertyNatureState.Get(modelType).PreviewFilePropertyDescribe;
        foreach (var property in previewFileProperty.Values.Where(x => x.IsStrict).Select(x => x.Property))
        {
            #region 替换值的本地函数
            object? ReplaceValue()
            {
                var value = property.GetValue(bindingModel);
                #region 转换值的本地函数
                IHasPreviewFile ConvertValue(IHasPreviewFile previewFile, int index)
                {
                    var key = $"{property.Name}-{index}";
                    return uploadFileDictionary.TryGetValue(key, out var uploadFile) ?
                      new HasUploadFileOnlyServer(uploadFile) :
                      CreateDataObj.PreviewFile(previewFile.CoverUri, previewFile.Uri, previewFile.FileName);
                }
                #endregion
                return value switch
                {
                    IHasPreviewFile previewFile => ConvertValue(previewFile, 0),
                    IEnumerable<IHasPreviewFile> previewFiles => previewFiles.Select(ConvertValue).ToArray(),
                    null => null,
                    _ => throw new NotSupportedException($"无法将{value.GetType()}类型的数据绑定为包含可预览文件的对象")
                };
            }
            #endregion
            var replaceValue = ReplaceValue();
            property.SetValue(bindingModel, replaceValue);
        }
        await bindingContext.BindingSuccess(bindingModel);
    }
    #endregion
}
