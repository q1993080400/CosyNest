
//这个方法是JS互操作的基础方法
function InvokeCode(code) {
    return Function(code)();
}

//注册net方法为JS方法
function RegisterNetMethod(net, jsMethodName, netMethodName) {
    window[jsMethodName] = async function (parameter) {
        await net.invokeMethodAsync(netMethodName, parameter);
    }
}

//获取复制文本
async function ReadCopyText() {
    try {
        let text = await navigator.clipboard.readText();
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

//跳转到指定的元素
function JumpTo(elementID, smooth, scrollingContextCSS) {
    document.querySelector(scrollingContextCSS)?.scrollIntoView(
        {
            behavior: "instant",
            block: "start"
        });
    document.getElementById(elementID)?.scrollIntoView(
        {
            behavior: smooth ? "smooth" : "instant",
            block: "end"
        });
}

//动态加载css文件
function LoadCSS(uri) {
    let head = document.head;
    for (let i in head.childNodes) {
        if (i.href == uri)
            return;
    }
    let link = document.createElement("link");
    link.type = "text/css";
    link.rel = "stylesheet";
    link.href = uri;
    head.insertBefore(link, head.firstChild);
}

//下载文件
function Download(uri, fileName) {
    let downloadDom = document.createElement('a');
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
    let array = Array.from(document.querySelectorAll(`#${id} [${attribute}]`));
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
        let lock = await navigator.wakeLock.request("screen");
        window[key] = lock;
        return true;
    } catch (e) {
        return false;
    }
}

//释放一个唤醒锁
async function ReleaseWakeLock(key) {
    let lock = window[key];
    if (lock)
        await lock.release();
}

const volumeKey = "TotalPlayVolume";

//初始化播放器音量
function InitializationPlayVolume() {
    let volume = localStorage.getItem(volumeKey);
    return parseFloat(volume ?? 0.5);
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
    let player = GetPlayer(playerID);
    return player == null ? null : {
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
            MediumSource: player.childNodes.length == 0 ?
                [player.src] :
                Array.prototype.map.call(player.childNodes, source => source.src)
        }
    };
}

//切换播放器的播放/暂停状态，然后返回播放器的状态
async function SwitchPlayerStatus(playerID) {
    let player = GetPlayer(playerID);
    if (player == null)
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
    let player = GetPlayer(playerID);
    if (player == null)
        return false;
    player.autoplay = stateOperational.autoPlay;
    let source = Array.prototype.map.call(player.childNodes, source => source.src);
    let mediumSource = stateOperational.mediumSource;
    if (JSON.stringify(source) != JSON.stringify(mediumSource)) {
        while (player.firstChild) {
            player.removeChild(player.firstChild);
        }
        for (let i of mediumSource) {
            let newSource = document.createElement("source");
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
        let played = stateOperational.played;
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
    let currentTimeElement = document.getElementById(currentTimeElementID);
    let totalTimeElement = document.getElementById(totalTimeElementID);
    currentTimeElement.textContent = toTimeString(player.currentTime);
    totalTimeElement.textContent = toTimeString(player.duration);
}

//获取播放器的播放进度，并返回一个double
function GetPlayerProgress(player) {
    let duration = player.duration;
    return duration == 0 || Number.isNaN(duration) ?
        0 : player.currentTime / duration;
}

//获取某一元素是否被用户看到
function CheckIntersecting(id) {
    let element = document.getElementById(id);
    if (!element)
        return false;
    const elementRect = element.getBoundingClientRect();
    return elementRect.top < window.innerHeight &&
        elementRect.bottom > 0 &&
        elementRect.left < window.innerWidth &&
        elementRect.right > 0;
}

//观察虚拟化容器
function ObservingVirtualizationContainers(netMethod, endID) {
    let observer = CacheObservation(endID,
        () => new IntersectionObserver(async (entries, observer) => {
            for (let i of entries) {
                if (i.isIntersecting) {
                    try {
                        await netMethod(0);
                    } catch (e) {
                        observer.disconnect();
                        console.error(e);
                    }
                    return;
                }
            }
        }));
    let element = document.getElementById(endID);
    observer.observe(element);
}

//如果页面存在一个观察者的缓存，提取它，否则创建一个新的缓存
function CacheObservation(key, createObserve) {
    let newKey = key + 'Observe'
    let observe = window[newKey];
    if (observe)
        return observe;
    observe = createObserve();
    window[newKey] = observe;
    return observe;
}

//注册观察媒体事件，它检测媒体的可见性，并自动播放暂停媒体
function ObserveVisiblePlay(id) {
    let element = document.querySelectorAll(`#${id} :is(video,audio)`);
    let observe = CacheObservation(id,
        () => new IntersectionObserver(array => {
            for (let i of array) {
                let medium = i.target;
                if (i.isIntersecting) {
                    medium.play();
                }
                else {
                    medium.pause();
                }
            }
        }));
    for (let i of element) {
        observe.observe(i);
    }
    function callback(mutationList, observer) {
        for (let i of mutationList) {
            if (i.type != "childList")
                continue;
            for (let add of i.addedNodes) {
                if (add.tagName == "VIDEO" || add.tagName == "AUDIO") {
                    observe.observe(add);
                }
            }
            for (let removed of i.removedNodes) {
                if (removed.tagName == "VIDEO" || removed.tagName == "AUDIO") {
                    observe.unobserve(removed);
                }
            }
        }
    }
    let observerDOM = CacheObservation(id + 'ObserveDOM', () => new MutationObserver(callback));
    observerDOM.observe(document.getElementById(id),
        {
            subtree: true,
            childList: true
        });
}

//将秒转化为时间字符串
function toTimeString(totalSeconds) {
    if (Number.isNaN(totalSeconds))
        totalSeconds = 0;
    const totalMs = totalSeconds * 1000;
    let position = totalMs >= 3600 * 1000 ?
        11 :
        totalMs >= 600 ?
            14 : 15;
    const result = new Date(totalMs).toISOString().slice(position, 19);
    return result;
}

//将一个SVG标签的文本封装成Blob，再封装成一个Uri
function CreateSVGUri(svgID) {
    let svg = document.getElementById(svgID).innerHTML;
    let blob = new Blob([svg],
        {
            type: "image/svg+xml"
        });
    return URL.createObjectURL(blob);
}

//这是一个读写cookie的小框架

var docCookies = {
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
        return keys().length;
    },
    keys: /* optional method: you can safely remove it! */ function () {
        let aKeys = document.cookie.replace(/((?:^|\s*;)[^\=]+)(?=;|$)|^\s*|\s*(?:\=[^;]*)?(?:\1|$)/g, "").split(/\s*(?:\=[^;]*)?;\s*/);
        for (let nIdx = 0; nIdx < aKeys.length; nIdx++) {
            aKeys[nIdx] = decodeURIComponent(aKeys[nIdx]);
        }
        return aKeys;
    },
    clear: function (sPath, sDomain) {
        let allKey = docCookies.keys();
        for (let i = 0; i < allKey.length; i++) {
            docCookies.removeItem(allKey[i], sPath, sDomain);
        }
    },
    keyAndValue: function () {
        let allKey = docCookies.keys();
        let allKeyValue = Array.from(allKey.map(x => {
            return {
                Key: x,
                Value: docCookies.getItem(key)
            }
        }));
        return allKeyValue;
    },
    tryGetValue: function (key) {
        if (this.hasItem(key)) {
            let value = getItem(key);
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
