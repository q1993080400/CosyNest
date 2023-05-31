window.ObserverMediaViewer = function (id) {
    var css = `#${id} div.carousel-item`;
    const targetNode = document.querySelectorAll(css);
    const config = { attributes: true, attributeFilter: ['class'] };
    const Initialization = function (node) {
        var isActive = node.classList.contains('active');
        var videos = node.querySelectorAll('video');
        for (var video of videos) {
            video.loop = true;
            if (isActive) {
                video.play();
            }
            else {
                video.pause();
            }
        }
    }
    const callback = function (mutationsList, observer) {
        for (let mutation of mutationsList) {
            Initialization(mutation.target);
        }
    };

    for (var node of targetNode) {
        if (node.HasObserverMediaViewer)
            continue;
        node.HasObserverMediaViewer = true;
        Initialization(node);
        const observer = new MutationObserver(callback);
        observer.observe(node, config);
    }
}
