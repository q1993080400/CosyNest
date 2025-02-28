
//下载文件预览中正在进行预览的文件
function DownloadPreviewFile(id) {
    const elements = document.querySelectorAll(`#${id} [src]`);
    for (const element of elements) {
        if (CheckIntersectingElement(element)) {
            Download(element.src, element.getAttribute("filename"));
            return;
        }
    }
}