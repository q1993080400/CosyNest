
//这个方法是JS互操作的基础方法
function InvokeCode(code) {
    return Function(code)();
}

//调用net实例方法
async function InvokeNetInstanceMethod(netObjectInstance, netMethodName, ...arguments) {
    return await netObjectInstance.invokeMethodAsync(netMethodName, ...arguments)
}

//竞争刷新，它可以避免两个对象同时执行刷新
function RreloadCompetition() {
    const key = "4BFD8EA6007C3260F37B07F6281D814F";
    if (window[key])
        return;
    window[key] = true;
    try {
        location.reload();
    } catch (e) {
        window[key] = false;
    }
}

//请求通知权限
async function RequestNotificationPermission() {
    switch (Notification.permission) {
        case "granted":
            return true;
        case "denied":
            return false;
        case "default":
            try {
                const result = await Notification.requestPermission();
                return result === "granted";
            } catch (e) {
                console.log(e);
                return false;
            }
        default:
            return false;
    }
}

//显示通知
function ShowNotification(webNotificationsOptions) {
    if (Notification.permission != "granted")
        return;
    if (webNotificationsOptions.onlyBackend && !document.hidden)
        return;
    const options = {
        body: webNotificationsOptions.body,
        tag: webNotificationsOptions.tag,
        icon: webNotificationsOptions.icon,
        requireInteraction: webNotificationsOptions.requireInteraction,
        renotify: webNotificationsOptions.renotify
    };
    const notification = new Notification(webNotificationsOptions.title, options);
    notification.onclick = () => {
        notification.close();
        const uri = webNotificationsOptions.uri;
        if (uri) {
            window.focus();
            location.href = uri;
        }
    };
}

//向JS事件注册Net方法，然后返回一个可以用来取消注册的对象名称
function RegisterNetMethod(element, type, listener, signalName) {
    const controller = new AbortController();
    element.addEventListener(type, listener,
        {
            signal: controller.signal
        });
    window[signalName] = function () {
        controller.abort();
    };
    return signalName;
}

//向任意JS事件注册Net方法，然后返回一个可以用来取消注册的对象名称
function RegisterEvent(elementSelector, type, netObjectInstance, netMethodName, signalName) {
    const element = document.querySelector(elementSelector);
    return RegisterNetMethod(element, type, async function (...arguments) {
        return await InvokeNetInstanceMethod(netObjectInstance, netMethodName, ...arguments);
    }, signalName);
}

//将net实例方法注册到Document的VisibilityChange事件中，然后返回一个可以用来取消注册的对象名称
function RegisterVisibilityChange(netObjectInstance, netMethodName, signalName) {
    return RegisterNetMethod(document, "visibilitychange", async () => {
        const visibilityState = function () {
            if (document.visibilityState === "prerender")
                return;
            const visibilityState = document.visibilityState;
            switch (visibilityState) {
                case "visible":
                    return 0;
                case "hidden":
                    return 1;
                default:
                    return 2;
            }
        }();
        setTimeout(async () => await InvokeNetInstanceMethod(netObjectInstance, netMethodName, visibilityState), 250);
    }, signalName);
}

//获取剪切板文本
async function ReadClipboardText() {
    try {
        const text = await navigator.clipboard.readText();
        return {
            IsSuccess: true,
            Text: text
        };
    } catch (e) {
        console.error(e);
        return {
            IsSuccess: false,
            Text: null
        };
    }
}

//获取剪切板对象
async function ReadClipboardObject(withObjectURL) {
    try {
        const clipboardItems = await navigator.clipboard.read();
        const array = [];
        for (const clipboardItem of clipboardItems) {
            const types = clipboardItem.types;
            const type = types.length === 0 ?
                null :
                types.find(type => !type.startsWith('application') && !type.startsWith('text')) ?? types[0];
            if (!type)
                return null;
            const clipboardItemObject = await clipboardItem.getType(type);
            const isText = types.includes('text/plain');
            const text = isText ? await clipboardItemObject.text() : null;
            const arrayBuffer = isText ? null : await clipboardItemObject.arrayBuffer();
            const data = new Uint8Array(arrayBuffer);
            const objectURL = !isText && withObjectURL ?
                URL.createObjectURL(clipboardItemObject) : null;
            const arrayItem = {
                type: type,
                size: clipboardItemObject.size,
                data: data,
                text: text,
                objectURL: objectURL
            };
            array.push(arrayItem);
        }
        return array;
    } catch (e) {
        console.error(e);
        return null;
    }
}

//写入剪切板文本
async function WriteClipboardText(text) {
    try {
        await navigator.clipboard.writeText(text);
        return true;

    } catch (e) {
        return false;
    }
}

//跳转到指定的元素
function JumpTo(elementID, smooth, jumpToEnd, scrollingContextCSS) {
    const behavior = smooth ? "smooth" : "instant";
    if (scrollingContextCSS) {
        document.querySelector(scrollingContextCSS)?.scrollIntoView(
            {
                behavior: behavior,
                block: "start"
            });
    }
    document.getElementById(elementID)?.scrollIntoView(
        {
            behavior: behavior,
            block: jumpToEnd ? "end" : "start"
        });
}

//动态加载css文件
function LoadCSS(uri) {
    const head = document.head;
    for (const i of head.childNodes) {
        if (i.href === uri)
            return;
    }
    const link = document.createElement("link");
    link.type = "text/css";
    link.rel = "stylesheet";
    link.href = uri;
    head.insertBefore(link, head.firstChild);
}

//下载文件，可指定文件名
function Download(uri, fileName) {
    const downloadDom = document.createElement('a');
    downloadDom.href = uri;
    downloadDom.download = fileName;
    document.body.appendChild(downloadDom);
    downloadDom.click();
    document.body.removeChild(downloadDom);
}

//弹出小窗口
function ShowSmallWindow(uri, proportion) {
    const width = screen.width;
    const height = screen.height;
    const isVertical = height > width;
    const windowFeatures = `popup,width=${isVertical ? width : width * proportion},height=${isVertical ? height * proportion : height}`;
    open(uri, '_blank', windowFeatures);
}

//固定具有所有特性的粘性定位元素
function FixedSticky(id, attribute) {
    const array = Array.from(document.querySelectorAll(`#${id} [${attribute}]`));
    let top = 0;
    for (let i = 1; i < array.length; i++) {
        let back = array[i - 1];
        let current = array[i];
        top += back.getBoundingClientRect().height;
        current.style.top = `${top}px`;
    }
}

//创建一个唤醒锁，它阻止屏幕变暗
async function CreateWakeLock(key) {
    try {
        const lock = await navigator.wakeLock.request("screen");
        window[key] = lock;
        return true;
    } catch (e) {
        return false;
    }
}

//释放一个唤醒锁
async function ReleaseWakeLock(key) {
    const lock = window[key];
    if (lock)
        await lock.release();
}

const volumeKey = "TotalPlayVolume";

//从缓存中提取播放器音量
function GetPlayVolume() {
    const volume = localStorage.getItem(volumeKey);
    return parseFloat(volume);
}

//记录播放器音量
function RecordPlayVolume(volume) {
    localStorage.setItem(volumeKey, volume);
}

//打开模式对话框
function OpenModelDialog(id) {
    document.getElementById(id).showModal();
}

//根据ID，获取播放器元素
function GetPlayer(playerID) {
    return document.querySelector(`#${playerID}:is(video,audio)`);
}

//获取播放器的状态
function GetPlayerState(playerID) {
    const player = GetPlayer(playerID);
    return player === null ? null : {
        PlayerID: playerID,
        Length: player.duration,
        InPlay: !player.paused,
        MediaName: player.currentSrc,
        RenderPlayerStateOperational: {
            Loop: player.loop,
            Muted: player.muted,
            Played: player.currentTime,
            Volume: player.volume,
            AutoPlay: player.autoplay,
            MediumSource: player.childNodes.length === 0 ?
                [player.src] :
                Array.prototype.map.call(player.childNodes, source => source.src)
        }
    };
}

//切换播放器的播放/暂停状态，然后返回播放器的状态
async function SwitchPlayerStatus(playerID) {
    const player = GetPlayer(playerID);
    if (player === null)
        return null;
    if (player.paused) {
        try {
            await player.play();
        } catch (e) {
            console.log(e);
        }
    }
    else {
        player.pause();
    }
    return GetPlayerState(playerID);
}

//写入播放器的状态，并返回是否成功写入
async function SetPlayerState(playerID, stateOperational) {
    const player = GetPlayer(playerID);
    if (player === null)
        return false;
    player.autoplay = stateOperational.autoPlay;
    const source = Array.prototype.map.call(player.childNodes, source => source.src);
    const mediumSource = stateOperational.mediumSource;
    if (JSON.stringify(source) != JSON.stringify(mediumSource)) {
        while (player.firstChild) {
            player.removeChild(player.firstChild);
        }
        for (const i of mediumSource) {
            const newSource = document.createElement("source");
            newSource.src = i;
            player.appendChild(newSource);
        }
        player.load();
        if (player.autoplay) {
            try {
                await player.play();
            } catch (e) {
                console.log(e);
            }
        }
    }
    else {
        const played = stateOperational.played;
        if (Math.abs(player.currentTime - played) >= 1)
            player.currentTime = played;
    }
    player.loop = stateOperational.loop;
    player.muted = stateOperational.muted;
    player.volume = stateOperational.volume;
    return true;
}

//将这个函数赋值给播放器的ontimeupdate事件，以更新已播放时间和媒体总长度
function UpdatePlayerTime(player, currentTimeElementID, totalTimeElementID) {
    const currentTimeElement = document.getElementById(currentTimeElementID);
    const totalTimeElement = document.getElementById(totalTimeElementID);
    if (currentTimeElement && totalTimeElement) {
        currentTimeElement.textContent = ToTimeString(player.currentTime);
        totalTimeElement.textContent = ToTimeString(player.duration);
    }
}

//获取播放器的播放进度，并返回一个double
function GetPlayerProgress(player) {
    const duration = player.duration;
    return duration === 0 || Number.isNaN(duration) ?
        0 : player.currentTime / duration;
}

//判断具有某一ID的元素是否被用户看到
function CheckIntersecting(id) {
    const element = document.getElementById(id);
    return CheckIntersectingElement(element);
}

//判断某一元素是否被用户看到
function CheckIntersectingElement(element) {
    if (!element)
        return false;
    const elementRect = element.getBoundingClientRect();
    return elementRect.top < window.innerHeight &&
        elementRect.bottom > 0 &&
        elementRect.left < window.innerWidth &&
        elementRect.right > 0;
}

//滚动至某一元素的末尾
function ScrollIntoViewToEnd(id) {
    function ScrollIntoView() {
        document.getElementById(id)?.scrollIntoView({
            behavior: "auto",
            block: "start"
        });
    }
    const element = document.getElementById(id);
    if (!element)
        return;
    ScrollIntoView();
    const parentElement = element.parentElement;
    if (!parentElement)
        return;
    const observer = new ResizeObserver(ScrollIntoView);
    observer.observe(parentElement);
    setTimeout(() => observer.disconnect(), 2000);
}

//观察虚拟化容器
function ObservingVirtualizationContainers(netObjectInstance, netMethodName, endID) {
    const observer = CacheObservation(endID,
        () => new IntersectionObserver(async (entries, observer) => {
            for (const i of entries) {
                if (i.isIntersecting) {
                    try {
                        await InvokeNetInstanceMethod(netObjectInstance, netMethodName);
                    } catch (e) {
                        observer.disconnect();
                        console.error(e);
                    }
                    return;
                }
            }
        }));
    const element = document.getElementById(endID);
    observer.observe(element);
}

//如果页面存在一个观察者的缓存，提取它，否则创建一个新的缓存
function CacheObservation(key, createObserve) {
    const newKey = key + 'Observe'
    let observe = window[newKey];
    if (observe)
        return observe;
    observe = createObserve();
    window[newKey] = observe;
    return observe;
}

//注册观察媒体事件，它为媒体元素附加某些功能
function ObserveVisibleMediaElement(id, options) {
    const container = document.getElementById(id);
    if (!container)
        return;

    //判断是否为媒体元素的方法
    function IsMediaElement(element) {
        return element.tagName === "VIDEO" || element.tagName === "AUDIO";
    }

    const visiblePlay = options.visiblePlay;
    const onlyVolume = options.onlyVolume;
    const onlyVolumeAttribute = "onlyvolume";
    const onlyVisibleVolumeAttribute = "onlyvisiblevolume";


    //设置唯一音量的方法
    function OnlyVolume() {
        if (!onlyVolume)
            return;
        const elements = Array.from(container.querySelectorAll("video,audio"));
        let canPlaySound = true;
        for (const element of elements) {
            if (element.hasAttribute(onlyVolumeAttribute) && canPlaySound &&
                (element.tagName === "AUDIO" || !element.hasAttribute(onlyVisibleVolumeAttribute) || CheckIntersectingElement(element))) {
                element.muted = false;
                canPlaySound = false;
            }
            else {
                element.muted = true;
            }
        }
    }

    const intersectionObserver = new IntersectionObserver(async array => {
        OnlyVolume();
        for (const i of array) {
            const medium = i.target;
            if (visiblePlay) {
                if (i.isIntersecting) {
                    try {
                        await medium.play();
                    } catch (e) {
                    }
                }
                else {
                    medium.pause();
                }
            }
        }
    });

    //用于注册功能的方法
    function RegistrationFunction(elements) {
        OnlyVolume();
        for (const element of elements) {
            if (!IsMediaElement(element))
                continue;
            intersectionObserver.observe(element);
            if (options.globalVolume) {
                //用来保存音量的方法
                function SaveVolume() {
                    RecordPlayVolume(element.volume)
                }
                //用来调整音量的方法
                function ChangeVolume() {
                    const volume = GetPlayVolume();
                    if (!isNaN(volume))
                        element.volume = volume;
                }
                element.addEventListener("click", () => {
                    element.addEventListener("volumechange", SaveVolume)
                    element.addEventListener("timeupdate", ChangeVolume);
                });
                element.addEventListener("focusout", () => {
                    element.removeEventListener("volumechange", SaveVolume);
                    element.removeEventListener("timeupdate", ChangeVolume)
                });
            }
        }
    }

    const elements = container.querySelectorAll("video,audio");
    RegistrationFunction(elements);

    function Callback(mutationList, observer) {
        let canOnlyVolume = true;
        for (const i of mutationList) {
            switch (i.type) {
                case "childList":
                    RegistrationFunction(i.addedNodes);
                    break;
                case "attributes":
                    if (canOnlyVolume) {
                        canOnlyVolume = false;
                        OnlyVolume();
                    }
                    break;
            }
        }
    }

    const mutationObserver = new MutationObserver(Callback);
    mutationObserver.observe(container,
        {
            subtree: true,
            childList: true,
            attributeFilter: onlyVolume ? [onlyVolumeAttribute, onlyVisibleVolumeAttribute] : undefined
        });
}

//将秒转化为时间字符串
function ToTimeString(totalSeconds) {
    if (Number.isNaN(totalSeconds))
        totalSeconds = 0;
    const totalMs = totalSeconds * 1000;
    const position = totalMs >= 3600 * 1000 ?
        11 :
        totalMs >= 600 ?
            14 : 15;
    const result = new Date(totalMs).toISOString().slice(position, 19);
    return result;
}

//将一个SVG标签的文本封装成Blob，再封装成一个Uri
function CreateSVGUri(svgID) {
    const svg = document.getElementById(svgID).innerHTML;
    const blob = new Blob([svg],
        {
            type: "image/svg+xml"
        });
    return URL.createObjectURL(blob);
}

//批量释放对象Url
function DisposableObjectURL(objectURLs) {
    for (const url of objectURLs) {
        URL.revokeObjectURL(url)
    }
}

//将普通Uri转换为BlobUri
async function ToBlobUri(uri) {
    const response = await fetch(uri);
    if (!response.ok)
        return null;
    const blob = await response.blob();
    return URL.createObjectURL(blob);
}

//返回待上传文件的的ObjectURL
function GetUploadFileURL(inputElementID, maxAllowedSize) {
    const files = document.getElementById(inputElementID).files;
    const urls = [];
    for (let i = 0; i < files.length; i++) {
        const file = files.item(i);
        if (file.size <= maxAllowedSize) {
            const url = URL.createObjectURL(file);
            urls.push(url);
        }
    }
    return urls;
}

//注册软键盘发送收尾事件，它允许在按下回车键的时候阻止默认操作，并使文本框失去焦点
function RegisterSoftKeyboardSend(id, blur) {
    const input = document.querySelector(`#${id}:is(textarea,input)`);
    input?.addEventListener('keydown', function (event) {
        if (event.key === 'Enter' && !event.shiftKey) {
            event.preventDefault();
            if (blur)
                this.blur();
        }
    });
}

//当一个textarea标签的值更改时，自动调整它的高度
function AutoGrow(id) {
    const textarea = document.querySelector(`textarea#${id}`);
    if (!textarea)
        return;
    textarea.style.height = 'unset';
    if (textarea.scrollHeight > textarea.clientHeight) {
        textarea.style.height = textarea.scrollHeight + "px";
    }
}

//这是一个读写cookie的小框架

this.docCookies = {
    getItem: function (sKey) {
        return decodeURIComponent(document.cookie.replace(new RegExp("(?:(?:^|.*;)\\s*" + encodeURIComponent(sKey).replace(/[-.+*]/g, "\\$&") + "\\s*\\=\\s*([^;]*).*$)|^.*$"), "$1")) || null;
    },
    setItem: function (sKey, sValue, vEnd, sPath, sDomain, bSecure) {
        if (!sKey || /^(?:expires|max\-age|path|domain|secure)$/i.test(sKey)) { return false; }
        let sExpires = "";
        if (vEnd) {
            switch (vEnd.constructor) {
                case Number:
                    sExpires = vEnd === Infinity ? "; expires=Fri, 31 Dec 9999 23:59:59 GMT" : "; max-age=" + vEnd;
                    break;
                case String:
                    sExpires = "; expires=" + vEnd;
                    break;
                case Date:
                    sExpires = "; expires=" + vEnd.toUTCString();
                    break;
            }
        }
        document.cookie = encodeURIComponent(sKey) + "=" + encodeURIComponent(sValue) + sExpires + (sDomain ? "; domain=" + sDomain : "") + (sPath ? "; path=" + sPath : "") + (bSecure ? "; secure" : "");
        return true;
    },
    removeItem: function (sKey, sPath, sDomain) {
        if (!sKey || !this.hasItem(sKey)) { return false; }
        document.cookie = encodeURIComponent(sKey) + "=; expires=Thu, 01 Jan 1970 00:00:00 GMT" + (sDomain ? "; domain=" + sDomain : "") + (sPath ? "; path=" + sPath : "");
        return true;
    },
    hasItem: function (sKey) {
        return (new RegExp("(?:^|;\\s*)" + encodeURIComponent(sKey).replace(/[-.+*]/g, "\\$&") + "\\s*\\=")).test(document.cookie);
    },
    count: function () {
        return this.keys().length;
    },
    keys: /* optional method: you can safely remove it! */ function () {
        const aKeys = document.cookie.replace(/((?:^|\s*;)[^\=]+)(?=;|$)|^\s*|\s*(?:\=[^;]*)?(?:\1|$)/g, "").split(/\s*(?:\=[^;]*)?;\s*/);
        for (let nIdx = 0; nIdx < aKeys.length; nIdx++) {
            aKeys[nIdx] = decodeURIComponent(aKeys[nIdx]);
        }
        return aKeys;
    },
    clear: function (sPath, sDomain) {
        const allKey = this.keys();
        for (let i = 0; i < allKey.length; i++) {
            this.removeItem(allKey[i], sPath, sDomain);
        }
    },
    keyAndValue: function () {
        const allKey = this.keys();
        const allKeyValue = Array.from(allKey.map(key => {
            return {
                Key: key,
                Value: this.getItem(key)
            }
        }));
        return allKeyValue;
    },
    tryGetValue: function (key) {
        if (this.hasItem(key)) {
            const value = this.getItem(key);
            return {
                Exist: true,
                Value: value
            };
        }
        return {
            Exist: false,
            Value: null
        };
    }
};
