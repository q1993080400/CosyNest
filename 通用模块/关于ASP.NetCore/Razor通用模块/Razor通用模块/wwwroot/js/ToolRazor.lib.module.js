
//在BlazorWebApp中，如果加载完WebAssembly，而且回到页面，则自动刷新

let serverStart = false;
let webAssemblyStart = false;
let webAppStarted = false;

document.addEventListener("visibilitychange", () => {
    if (serverStart && webAssemblyStart && webAppStarted && document.visibilityState === "visible") {
        RreloadCompetition();
    }
});

export function afterWebStarted(blazor) {
    webAppStarted = true;
}

export function afterServerStarted(blazor) {
    serverStart = true;
    const resourceKey = "blazor-resource-hash";
    let intervalID = 0;
    let count = 0;
    intervalID = setInterval(() => {
        count++;
        if (count >= 120) {
            clearInterval(intervalID);
            return;
        }
        for (let i = 0; i < localStorage.length; i++) {
            const key = localStorage.key(i);
            if (key?.startsWith(resourceKey)) {
                webAssemblyStart = true;
                clearInterval(intervalID);
                return;
            }
        }
    }, 500);
}