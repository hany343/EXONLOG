// In wwwroot/js/camera.js
function initializeVideoPlayer(elementId) {
    var player = videojs(elementId, {
        techOrder: ['html5'],
        autoplay: true,
        controls: true
    });

    player.src({
        src: document.getElementById(elementId).querySelector('source').src,
        type: 'application/x-mpegURL'
    });
}