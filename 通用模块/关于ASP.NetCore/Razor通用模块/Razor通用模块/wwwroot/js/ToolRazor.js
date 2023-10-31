
//这个方法是JS互操作的基础方法
function InvokeCode(code) {
    return Function(code)();
}

//动态加载js脚本
function LoadScript(uri) {
    for (var i in document.scripts) {
        if (i.src == uri)
            return;
    }
    var script = document.createElement("script");
    script.type = "text/javascript";
    script.src = uri;
    document.body.appendChild(script);
}

//动态加载css
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

//观察虚拟化容器
function ObservingVirtualizationContainers(netMethod, endID) {
    var observe = new IntersectionObserver(x => {
        if (x[0].isIntersecting)
            netMethod(0);
    });
    CacheObservation(endID + 'Observe', observe);
    observe.observe(document.getElementById(endID));
}

//初始化VideoJS
function InitializationVideoJS(id, op) {
    var play = videojs.getPlayer(id);
    if (play != undefined) {
        if (play.srcHash != op.srcHash) {
            play.src(op.sources);
            play.srcHash = op.srcHash;
        }
        return;
    }
    var element = document.getElementById(id);
    element.style.display = 'none';
    var newPlay = videojs(id, op, function () {
        element.style.display = 'block';
    });
    newPlay.srcHash = op.srcHash;
}

//如果页面存在一个观察者的缓存，则将它释放，然后放入新的观察者
//它可以避免观察者被重复初始化
function CacheObservation(key, observe) {
    var old = window[key];
    if (old != undefined) {
        old?.disconnect();
    }
    window[key] = observe;
}

//注册观察媒体事件，它检测媒体的可见性，并自动播放暂停媒体
function ObserveVisiblePlay(id) {
    var element = document.querySelectorAll(`#${id} :is(video,audio)`);
    var observe = new IntersectionObserver(array => {
        for (var i of array) {
            var medium = i.target;
            if (i.isIntersecting) {
                medium.play();
            }
            else {
                medium.pause();
            }
        }
    });
    CacheObservation(id + 'Observe', observe);
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
    var observerDOM = new MutationObserver(callback);
    CacheObservation(id + 'ObserveDOM', observerDOM);
    observerDOM.observe(document.getElementById(id),
        {
            subtree: true,
            childList: true
        });
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
