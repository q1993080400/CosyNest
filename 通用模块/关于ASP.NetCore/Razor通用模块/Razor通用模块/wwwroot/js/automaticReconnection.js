/*在_Host.cshtml中引用本文件可以启用断线自动重连功能，
  方法如下，将引用替换为这样的形式：

  <script src="_framework/blazor.server.js" autostart="false"></script>
  <script src="_content/ToolRazor/js/automaticReconnection.js"></script>
  
  */

(() => {
    const maximumRetryCount = 1;

    const startReconnectionProcess = () => {

        let isCanceled = false;

        (async () => {
            for (let i = 0; i < maximumRetryCount; i++) {

                if (isCanceled) {
                    return;
                }

                try {
                    const result = await Blazor.reconnect();
                    if (!result) {
                        // The server was reached, but the connection was rejected; reload the page.
                        location.reload();
                        return;
                    }
                    return;
                } catch {
                }


            }
            location.reload();
        })();

        return {
            cancel: () => {
                isCanceled = true;
            },
        };
    };

    let currentReconnectionProcess = null;

    Blazor.start({
        reconnectionHandler: {
            onConnectionDown: () => currentReconnectionProcess ??= startReconnectionProcess(),
            onConnectionUp: () => {
                currentReconnectionProcess?.cancel();
                currentReconnectionProcess = null;
            },
        }
    });
})();