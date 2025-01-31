
//在BlazorWebApp中，如果丢失连接，则自动刷新页面

let connection = false;

Blazor.start({
    circuit: {
        reconnectionHandler: {
            onConnectionDown: () => {
                connection = true;
                const controller = new AbortController();
                document.addEventListener("visibilitychange", () => {
                    if (!connection) {
                        controller.abort();
                        return;
                    }
                    if (!document.hidden) {
                        controller.abort();
                        location.reload();
                    }
                }, {
                    signal: controller.signal
                });
                document.querySelector("body").addEventListener("click", () => {
                    if (connection)
                        location.reload();
                }, {
                    once: true
                });
            },
            onConnectionUp: () => connection = false
        }
    }
});
