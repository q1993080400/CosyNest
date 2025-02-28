
//在BlazorWebApp中，如果丢失连接，则自动刷新页面

let lostConnection = false;

document.addEventListener("visibilitychange", () => {
    if (document.visibilityState === "visible" && lostConnection) {
        RreloadCompetition();
    }
});

document.querySelector("body").addEventListener("click", () => {
    if (lostConnection) {
        RreloadCompetition();
    }
});

Blazor.start({
    circuit: {
        reconnectionHandler: {
            onConnectionDown: async () => {
                try {
                    lostConnection = !await Blazor.reconnect();
                } catch (e) {
                    lostConnection = true;
                }
                if (lostConnection) {
                    RreloadCompetition();
                }
            },
            onConnectionUp: () => lostConnection = false
        }
    }
});
