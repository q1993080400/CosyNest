
//这个方法是JS互操作的基础方法
function InvokeCode(code) {
    return Function(code)();
}

//动态加载css文件
function LoadCSS(uri) {
    var head = document.head;
    for (var i in head.childNodes) {
        if (i.href == uri)
            return;
    }
    var link = document.createElement("link");
    link.type = "text/css";
    link.rel = "stylesheet";
    link.href = uri;
    head.insertBefore(link, head.firstChild);
}

//创建一个唤醒锁，它阻止屏幕变暗
async function CreateWakeLock(key) {
    await navigator.wakeLock.request("screen").
        then(lock => {
            window[key] = lock;
        });
}

//释放一个唤醒锁
async function ReleaseWakeLock(key) {
    var lock = window[key];
    if (lock)
        await lock.release();
}

const volumeKey = "TotalPlayVolume";

//初始化播放器音量
function InitializationPlayVolume() {
    var volume = localStorage.getItem(volumeKey);
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
    var player = GetPlayer(playerID);
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
    var player = GetPlayer(playerID);
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
    var player = GetPlayer(playerID);
    if (player == null)
        return false;
    player.autoplay = stateOperational.autoPlay;
    var source = Array.prototype.map.call(player.childNodes, source => source.src);
    var mediumSource = stateOperational.mediumSource;
    if (JSON.stringify(source) != JSON.stringify(mediumSource)) {
        while (player.firstChild) {
            player.removeChild(player.firstChild);
        }
        for (var i of mediumSource) {
            var newSource = document.createElement("source");
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
        var played = stateOperational.played;
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
    var currentTimeElement = document.getElementById(currentTimeElementID);
    var totalTimeElement = document.getElementById(totalTimeElementID);
    currentTimeElement.textContent = toTimeString(player.currentTime);
    totalTimeElement.textContent = toTimeString(player.duration);
}

//获取播放器的播放进度，并返回一个double
function GetPlayerProgress(player) {
    var duration = player.duration;
    return duration == 0 || Number.isNaN(duration) ?
        0 : player.currentTime / duration;
}

//观察虚拟化容器
function ObservingVirtualizationContainers(netMethod, endID) {
    var isIntersecting = true;
    var inAdd = false;
    var observe = CacheObservation(endID + 'Observe',
        () => new IntersectionObserver(x => {
            var first = x[0];
            isIntersecting = first.isIntersecting;
            if (inAdd)
                return;
            inAdd = true;
            var intervalID = 0;
            function onInterval() {
                if (isIntersecting) {
                    netMethod(0)
                }
                else {
                    clearInterval(intervalID);
                    inAdd = false;
                }
            }
            onInterval();
            intervalID = setInterval(onInterval, 1000);
        }));
    observe.observe(document.getElementById(endID));
}

//如果页面存在一个观察者的缓存，则将它释放，然后放入新的观察者
//它可以避免观察者被重复初始化
function CacheObservation(key, createObserve) {
    var old = window[key];
    if (old != undefined) {
        old?.disconnect();
    }
    var observe = createObserve();
    window[key] = observe;
    return observe;
}

//注册观察媒体事件，它检测媒体的可见性，并自动播放暂停媒体
function ObserveVisiblePlay(id) {
    var element = document.querySelectorAll(`#${id} :is(video,audio)`);
    var observe = CacheObservation(id + 'Observe',
        () => new IntersectionObserver(array => {
            for (var i of array) {
                var medium = i.target;
                if (i.isIntersecting) {
                    medium.play();
                }
                else {
                    medium.pause();
                }
            }
        }));
    for (var i of element) {
        observe.observe(i);
    }
    function callback(mutationList, observer) {
        for (var i of mutationList) {
            if (i.type != "childList")
                continue;
            for (var add of i.addedNodes) {
                if (add.tagName == "VIDEO" || add.tagName == "AUDIO") {
                    observe.observe(add);
                }
            }
            for (var removed of i.removedNodes) {
                if (removed.tagName == "VIDEO" || removed.tagName == "AUDIO") {
                    observe.unobserve(removed);
                }
            }
        }
    }
    var observerDOM = CacheObservation(id + 'ObserveDOM', () => new MutationObserver(callback));
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
    var position = totalMs >= 3600 * 1000 ?
        11 :
        totalMs >= 600 ?
            14 : 15;
    const result = new Date(totalMs).toISOString().slice(position, 19);
    return result;
}

//将一个SVG标签的文本封装成Blob，再封装成一个Uri
function CreateSVGUri(svgID) {
    var svg = document.getElementById(svgID).innerHTML;
    var blob = new Blob([svg],
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
        var sExpires = "";
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
    keys: /* optional method: you can safely remove it! */ function () {
        var aKeys = document.cookie.replace(/((?:^|\s*;)[^\=]+)(?=;|$)|^\s*|\s*(?:\=[^;]*)?(?:\1|$)/g, "").split(/\s*(?:\=[^;]*)?;\s*/);
        for (var nIdx = 0; nIdx < aKeys.length; nIdx++) {
            aKeys[nIdx] = decodeURIComponent(aKeys[nIdx]);
        }
        return aKeys;
    },
    clear: function (sPath, sDomain) {
        var allKey = docCookies.keys();
        for (var i = 0; i < allKey.length; i++) {
            docCookies.removeItem(allKey[i], sPath, sDomain);
        }
    },
    keyAndValue: function () {
        var allKey = docCookies.keys();
        for (var i = 0; i < allKey.length; i++) {
            var key = allKey[i];
            allKey[i] = {
                Key: key,
                Value: docCookies.getItem(key)
            }
        }
        return allKey;
    }
};
